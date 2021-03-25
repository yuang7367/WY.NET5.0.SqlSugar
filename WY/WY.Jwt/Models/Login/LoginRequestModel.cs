using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WY.Jwt.Models.Login
{
    public class LoginRequestModel
    {
        public string UserName { get; set; }

        public string Pwd { get; set; }
    }
}
