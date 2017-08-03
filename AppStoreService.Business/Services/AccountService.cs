using AppStoreService.Core;
using AppStoreService.Core.Business;
using AppStoreService.Core.Entities;
using AppStoreService.Core.FindersModels;
using System.Threading.Tasks;

namespace AppStoreService.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly ICreate<User> _userCreator;
        private readonly IUpdate<User> _userUpdater;
        private readonly IFilter<UserLoginFindModel, User> _userLoginFilter;
        private readonly IFilter<UserConfirmModel, User> _userConfirmFilter;
        private readonly IEmailSendService _emailService;

        public AccountService(IFilter<UserLoginFindModel, User> userLoginFilter,
            ICreate<User> userCreator, IEmailSendService emailService,
            IFilter<UserConfirmModel, User> userConfirmFilter,
            IUpdate<User> userUpdater)
        {
            _userCreator = userCreator;
            _userUpdater = userUpdater;
            _userLoginFilter = userLoginFilter;
            _emailService = emailService;
            _userConfirmFilter = userConfirmFilter;
        }

        public async Task<bool> ConfirmAccount(string userId, string code)
        {
            bool isConfirm = false;

            User user = await _userConfirmFilter.FilterAsync(new UserConfirmModel
            {
                UserId = userId,
                Code = code
            });

            if (user != null)
            {
                user.IsConfirm = true;
                await _userUpdater.UpdateAsync(user);
                isConfirm = true;
            }

            return await Task.Run(() =>
            {
                return isConfirm;
            });
        }

        public Task<User> CreateUserAsync(User item)
        {
            return _userCreator.CreateAsync(item);
        }

        public User GetUser(string login, string password)
        {
            return _userLoginFilter.Filter(new UserLoginFindModel
            {
                Login = login,
                Password = password
            });
        }

        public async Task SendMailAsync(Email item)
        {
            await _emailService.SendAsync(item);
        }
    }
}