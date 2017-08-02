using AppStoreService.Core;
using AppStoreService.Core.Business;
using AppStoreService.Core.Entities;
using System.Collections.Generic;

namespace AppStoreService.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ICreate<User> _userCreator;
        private readonly IRead<User> _userReader;
        private readonly IUpdate<User> _userUpdater;
        private readonly IDelete<string> _userRemover;

        public UserService(ICreate<User> userCreator, IRead<User> userReader,
            IUpdate<User> userUpdater, IDelete<string> userRemover)
        {
            _userCreator = userCreator;
            _userReader = userReader;
            _userUpdater = userUpdater;
            _userRemover = userRemover;
        }

        public User CreateUser(User item)
        {
            return _userCreator.Create(item);
        }

        public IEnumerable<User> GetUsers()
        {
            return _userReader.Read();
        }

        public void RemoveUser(string userId)
        {
            _userRemover.Delete(userId);
        }

        public void UpdateUser(User item)
        {
            _userUpdater.Update(item);
        }
    }
}