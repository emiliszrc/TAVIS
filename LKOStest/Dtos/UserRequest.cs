using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKOStest.Dtos
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
