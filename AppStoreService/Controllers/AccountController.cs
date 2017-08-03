using AppStoreService.Core;
using AppStoreService.Core.Business;
using AppStoreService.Core.Entities;
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

        [HttpPost("login")]
        public async Task Login(User model)
        {
            var identity = GetIdentity(model.Login, model.Password);
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
        public async Task Register(User model)
        {
            User user = await _accountService.CreateUserAsync(model);
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

        [HttpGet("confirm")]
        public async Task<bool> Confirm(string userId, string code)
        {
            return await _accountService.ConfirmAccount(userId, code);
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