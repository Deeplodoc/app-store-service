using AppStoreService.Core;
using AppStoreService.Core.Business;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using System.Threading.Tasks;
using System;
using AppStoreService.Core.Business.Models;

namespace AppStoreService.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly ICreate<User> _userCreator;
        private readonly IUpdate<User> _userUpdater;
        private readonly IFilter<UserLoginFindModel, User> _userLoginFilter;
        private readonly IFilter<UserConfirmModel, User> _userConfirmFilter;
        private readonly IFilter<UserMailModel, User> _userMailFinder;
        private readonly IFilter<UserResetCodeModel, User> _userResetCodeFinder;
        private readonly IFilter<UserByIdModel, User> _userByIdFinder;
        private readonly IEmailSendService _emailService;
        private readonly Configuration _config;

        public AccountService(IFilter<UserLoginFindModel, User> userLoginFilter,
            ICreate<User> userCreator, IEmailSendService emailService,
            IFilter<UserConfirmModel, User> userConfirmFilter,
            IUpdate<User> userUpdater,
            IFilter<UserMailModel, User> userMailFinder,
            IFilter<UserResetCodeModel, User> userResetCodeFinder,
            IFilter<UserByIdModel, User> userByIdFinder,
            Configuration config)
        {
            _userCreator = userCreator;
            _userUpdater = userUpdater;
            _userLoginFilter = userLoginFilter;
            _emailService = emailService;
            _userConfirmFilter = userConfirmFilter;
            _userMailFinder = userMailFinder;
            _userResetCodeFinder = userResetCodeFinder;
            _userByIdFinder = userByIdFinder;
            _config = config;
        }

        public async Task<bool> ChangePassword(string email, string password)
        {
            bool isChange = false;

            User user = await _userMailFinder.FilterAsync(new UserMailModel
            {
                Email = email
            });

            if (user != null && user.IsConfirm)
            {
                user.Password = password;
                await _userUpdater.UpdateAsync(user);
                isChange = true;
            }

            return await Task.Run(() =>
            {
                return isChange;
            });
        }

        public async Task<User> ConfirmAccount(string userId, string code)
        {
            User user = await _userConfirmFilter.FilterAsync(new UserConfirmModel
            {
                UserId = userId,
                Code = code
            });

            if (user != null)
            {
                user.IsConfirm = true;
                user.ConfirmCode = string.Empty;
                await _userUpdater.UpdateAsync(user);
                return user;
            }

            return null;
        }

        public async Task<User> CreateUserAsync(User item)
        {
            User user = await _userMailFinder.FilterAsync(new UserMailModel
            {
                Email = item.Email
            });

            if (user == null)
                return await _userCreator.CreateAsync(item);

            return null;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            bool isForgot = false;
            User user = await _userMailFinder.FilterAsync(new UserMailModel
            {
                Email = email
            });

            if (user != null && user.IsConfirm)
            {
                user.ResetPasswordCode = Guid.NewGuid().ToString();
                await _userUpdater.UpdateAsync(user);

                string url = $"{_config.ForgotPasswordUrl}/?code={user.ResetPasswordCode}";
                string body = $"Чтобы сбросить пароль, перейдите по ссылке по ссылке: <a href='{url}'>link</a>";
                Email mail = new Email
                {
                    AddressFrom = _config.EmailFrom,
                    AddressTo = user.Email,
                    Title = "Сброс пароля.",
                    Subject = "",
                    Body = body
                };
                await SendMailAsync(mail);

                isForgot = true;
            }

            return await Task.Run(() =>
            {
                return isForgot;
            });
        }

        public async Task<User> GetForgotUser(string code)
        {
            return await _userResetCodeFinder.FilterAsync(new UserResetCodeModel
            {
                Code = code
            });
        }

        public User GetUser(LoginModel model)
        {
            return _userLoginFilter.Filter(new UserLoginFindModel
            {
                Login = model.Login,
                Password = model.Password
            });
        }

        public Task<User> GetUserByIdAsync(string userId)
        {
            return _userByIdFinder.FilterAsync(new UserByIdModel
            {
                UserId = userId
            });
        }

        public async Task SendMailAsync(Email item)
        {
            await _emailService.SendAsync(item);
        }

        public async Task UpdateUser(User item)
        {
            await _userUpdater.UpdateAsync(item);
        }
    }
}