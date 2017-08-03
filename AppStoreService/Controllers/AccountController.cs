using AppStoreService.Core;
using AppStoreService.Core.Business;
using AppStoreService.Core.Entities;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task Login(string login, string password)
        {
            var identity = GetIdentity(login, login);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
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
                username = identity.Name
            };

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task Register(User model)
        {
            User user = await _accountService.CreateUserAsync(model);

            if (user != null)
            {
                string url = $"{_config.EmailConfirmUrl}/?userId={user.Id}&code={user.ConfirmCode}";
                string body = $"Подтвердите регистрацию, перейдя по ссылке: <a href='{url}'>link</a>";
                Email email = new Email
                {
                    AddressFrom = _config.EmailFrom,
                    AddressTo = model.Email,
                    Title = "Подтверждение почты.",
                    Subject = "",
                    Body = body
                };
                await _accountService.SendMailAsync(email);
            }

            Response.StatusCode = 400;
            await Response.WriteAsync("Пользоатель с таким логином или почтой уже существует.");
            return;
        }

        [HttpGet("confirm")]
        public async Task Confirm(string userId, string code)
        {
            bool result = await _accountService.ConfirmAccount(userId, code);
            if (!result)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Не удалось подтвердить почту.");
                return;
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
                return;
            }
        }

        [AllowAnonymous]
        [HttpGet("resetPassword")]
        public async Task ResetPassword(string email, string newPassword)
        {
            bool result = await _accountService.ChangePassword(email, newPassword);
            if (!result)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Пользоатель не найден или email был не подтверждён.");
                return;
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

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _accountService.GetUser(username, password);
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