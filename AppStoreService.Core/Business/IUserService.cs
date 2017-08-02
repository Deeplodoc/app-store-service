using AppStoreService.Core.Entities;
using System.Collections.Generic;

namespace AppStoreService.Core.Business
{
    public interface IUserService
    {
        User CreateUser(User item);
        IEnumerable<User> GetUsers();
        void UpdateUser(User item);
        void RemoveUser(string userId);
    }
}