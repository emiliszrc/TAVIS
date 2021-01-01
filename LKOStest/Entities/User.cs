using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Dtos;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace LKOStest.Entities
{
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public static User From(UserRequest user)
        {
            return new User
            {
                Username = user.Username,
                Password = user.Password
            };
        }

        public Organisation Organisation { get; set; }
    }
}
