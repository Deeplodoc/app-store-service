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
        private readonly IFilter<UserResetPassModel, User> _userResetFinder;
        private readonly IEmailSendService _emailService;
        private readonly Configuration _config;

        public AccountService(IFilter<UserLoginFindModel, User> userLoginFilter,
            ICreate<User> userCreator, IEmailSendService emailService,
            IFilter<UserConfirmModel, User> userConfirmFilter,
            IUpdate<User> userUpdater,
            IFilter<UserMailModel, User> userMailFinder,
            IFilter<UserResetCodeModel, User> userResetCodeFinder,
            IFilter<UserByIdModel, User> userByIdFinder,
            IFilter<UserResetPassModel, User> userResetFinder,
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
            _userResetFinder = userResetFinder;
            _config = config;
        }

        public async Task<bool> ChangePassword(string email, string code, string password)
        {
            User user = await _userResetFinder.FilterAsync(new UserResetPassModel
            {
                Email = email,
                Code = code
            });

            if (user != null && user.IsConfirm)
            {
                user.Password = password;
                user.ResetPasswordCode = string.Empty;
                await _userUpdater.UpdateAsync(user);

                return await Task.Run(() =>
                {
                    return true;
                });
            }

            else
            {
                return await Task.Run(() =>
                {
                    return false;
                });
            }
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
            User user = await _userMailFinder.FilterAsync(new UserMailModel
            {
                Email = email
            });

            if (user != null && user.IsConfirm)
            {
                user.ResetPasswordCode = Guid.NewGuid().ToString();
                await _userUpdater.UpdateAsync(user);

                string url = $"{_config.ForgotPasswordUrl}?email={user.Email}&code={user.ResetPasswordCode}";
                string body = $"Чтобы сбросить пароль, перейдите по ссылке по ссылке: <a href='{url}'>Сбросить пароль.</a>";
                Email mail = new Email
                {
                    AddressFrom = _config.EmailFrom,
                    AddressTo = user.Email,
                    Title = "Сброс пароля.",
                    Subject = "",
                    Body = body
                };
                await SendMailAsync(mail);

                return await Task.Run(() =>
                {
                    return true;
                });
            }

            else
            {
                return await Task.Run(() =>
                {
                    return false;
                });
            }
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

        public async Task<bool> Registration(User item)
        {
            User user = await _userCreator.CreateAsync(item);

            if (user != null)
            {
                await SenMailConfirm(user);

                return await Task.Run(() =>
                {
                    return true;
                });
            }

            else
            {
                return await Task.Run(() =>
                {
                    return false;
                });
            }
        }

        public async Task<bool> SendMailConfirm(string userId)
        {
            User user = await _userByIdFinder.FilterAsync(new UserByIdModel
            {
                UserId = userId
            });

            if (user != null)
            {
                user.ConfirmCode = Guid.NewGuid().ToString();
                await _userUpdater.UpdateAsync(user);
                await SenMailConfirm(user);

                return await Task.Run(() =>
                {
                    return true;
                });
            }

            else
            {
                return await Task.Run(() =>
                {
                    return false;
                });
            }
        }

        public async Task UpdateUser(User item)
        {
            await _userUpdater.UpdateAsync(item);
        }

        private async Task SenMailConfirm(User user)
        {
            string url = $"{_config.EmailConfirmUrl}/?userId={user.Id}&code={user.ConfirmCode}";
            string body = $"Подтвердите регистрацию, перейдя по ссылке: <a href='{url}'>Подтвердить аккаунт.</a>";
            Email email = new Email
            {
                AddressFrom = _config.EmailFrom,
                AddressTo = user.Email,
                Title = "Подтверждение почты.",
                Subject = "",
                Body = body
            };
            await _emailService.SendAsync(email);
        }

        private async Task SendMailAsync(Email item)
        {
            await _emailService.SendAsync(item);
        }
    }
}