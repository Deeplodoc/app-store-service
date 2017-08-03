﻿using AppStoreService.Core.Entities;
using System.Threading.Tasks;

namespace AppStoreService.Core.Business
{
    public interface IAccountService
    {
        User GetUser(string login, string password);
        Task<bool> ConfirmAccount(string userId, string code);
        Task<User> CreateUserAsync(User item);
        Task SendMailAsync(Email item);
    }
}