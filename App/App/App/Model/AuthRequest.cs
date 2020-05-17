using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model
{
    public class AuthRequest : User
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
