using AppStoreService.Core;
using AppStoreService.Core.Business;
using AppStoreService.Core.Entities;
using System.Threading.Tasks;

namespace AppStoreService.Business.Services
{
    public class EmailSendService : IEmailSendService
    {
        private readonly ICreate<Email> _emailCreator;
        private readonly ISend<Email> _emailSender;

        public EmailSendService(ICreate<Email> emailCreator,
            ISend<Email> emailSender)
        {
            _emailCreator = emailCreator;
            _emailSender = emailSender;
        }

        public async Task<Email> SendAsync(Email item)
        {
            await _emailSender.SendAsync(item);
            return await _emailCreator.CreateAsync(item);
        }
    }
}