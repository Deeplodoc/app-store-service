﻿using System;

namespace AppStoreService.Core.Entities
{
    public class User
    {
        public object Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime BDay { get; set; }
        public string Email { get; set; }
        public bool IsConfirm { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmCode { get; set; }
        public string ResetPasswordCode { get; set; }
    }
}