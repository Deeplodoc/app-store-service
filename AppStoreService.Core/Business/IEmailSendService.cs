using AppStoreService.Core.Entities;
using System.Threading.Tasks;

namespace AppStoreService.Core.Business
{
    public interface IEmailSendService
    {
        Task<Email> SendAsync(Email item);
    }
}