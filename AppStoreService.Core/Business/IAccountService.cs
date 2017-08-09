using AppStoreService.Core.Business.Models;
using AppStoreService.Core.Entities;
using System.Threading.Tasks;

namespace AppStoreService.Core.Business
{
    public interface IAccountService
    {
        User GetUser(LoginModel model);
        Task<User> ConfirmAccount(string userId, string code);
        Task<User> CreateUserAsync(User item);
        Task SendMailAsync(Email item);
        Task<bool> ChangePassword(string email, string password);
        Task<bool> ForgotPassword(string mail);
        Task UpdateUser(User item);
        Task<User> GetForgotUser(string code);
        Task<User> GetUserByIdAsync(string userId);
    }
}