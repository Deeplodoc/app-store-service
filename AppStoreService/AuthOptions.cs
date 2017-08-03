using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AppStoreService
{
    public class AuthOptions
    {
        public const string Issuer = "AppStoreService";
        public const string Audience = "AppStoreServiceHost";
        const string Key = "abrakadabra_supapupakey!12345";
        public const int LifetimeDays = 7;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}