﻿using AppStoreService.Core;
using AppStoreService.Core.Business;
using AppStoreService.Core.Business.Models;
using AppStoreService.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppStoreService.Controllers
{
    [EnableCors("AllowAll")]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly Configuration _config;
        private readonly IAccountService _accountService;

        public AccountController(Configuration config,
            IAccountService accountService)
        {
            _config = config;
            _accountService = accountService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task Login([FromBody]LoginModel model)
        {
            ClaimsIdentity identity = GetIdentity(model, out User user);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(AuthOptions.LifetimeDays)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                userId = user.Id.ToString()
            };

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task Register([FromBody]User model)
        {
            bool result = await _accountService.Registration(model);

            if (!result)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Пользоатель с таким логином или почтой уже существует.");
            }
        }

        [HttpGet("confirm")]
        public async Task Confirm(string userId, string code)
        {
            User user = await _accountService.ConfirmAccount(userId, code);
            if (user != null)
            {
                await Login(new LoginModel
                {
                    Login = user.Login,
                    Password = user.Password
                });
            }

            else
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Не удалось подтвердить почту, либо почта уже подтверждена.");
            }
        }

        [HttpGet("sendConfirm")]
        public async Task SendConfirm(string userId)
        {
            bool result = await _accountService.SendMailConfirm(userId);

            if (!result)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Пользователь не найден.");
            }
        }

        [AllowAnonymous]
        [HttpGet("forgotPassword")]
        public async Task ForgotPassword(string email)
        {
            bool result = await _accountService.ForgotPassword(email);

            if (!result)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Пользоатель не найден.");
            }
        }

        [AllowAnonymous]
        [HttpGet("resetPassword")]
        public async Task ResetPassword(string email, string code, string newPassword)
        {
            bool result = await _accountService.ChangePassword(email, code, newPassword);
            if (!result)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Пользоатель не найден или email был не подтверждён.");
            }
        }

        [Authorize]
        [HttpPost("editUser")]
        public async Task EditUser(User model)
        {
            await _accountService.UpdateUser(model);
        }

        [AllowAnonymous]
        [HttpGet("getUserByForgotCode")]
        public async Task<User> GetUserByForgotCode(string code)
        {
            return await _accountService.GetForgotUser(code);
        }

        [Authorize]
        [HttpGet("getUserById")]
        public async Task<User> GetUserById(string userId)
        {
            return await _accountService.GetUserByIdAsync(userId);
        }

        private ClaimsIdentity GetIdentity(LoginModel model, out User user)
        {
            user = _accountService.GetUser(model);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}