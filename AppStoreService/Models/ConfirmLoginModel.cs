using AppStoreService.Core.Entities;

namespace AppStoreService.Models
{
    public class ConfirmLoginModel : User
    {
        public ConfirmLoginModel() { }
        public ConfirmLoginModel(User item)
        {
            Id = item.Id;
            FirstName = item.FirstName;
            LastName = item.LastName;
            Address = item.Address;
            Phone = item.Phone;
            BDay = item.BDay;
            Email = item.Email;
            IsConfirm = item.IsConfirm;
            Login = item.Login;
            Password = item.Password;
            ConfirmCode = item.ConfirmCode;
            ResetPasswordCode = item.ResetPasswordCode;
        }

        public string AccessToken { get; set; }
    }
}