using System;

namespace AppStoreService.Core.Entities
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime BDay { get; set; }
        public string Enail { get; set; }
        public bool IsConfirm { get; set; }
    }
}