using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;

namespace LKOStest.Entities
{
    public class Client : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string DefaultPassword { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public bool Notified { get; set; } = false;

        public List<Participation> Participations { get; set; }

        public static Client From(ClientRequest clientRequest) => new Client
        {
            Name = clientRequest.FirstName,
            Surname = clientRequest.LastName,
            Email = clientRequest.Email,
            DefaultPassword = clientRequest.DefaultPassword
        };
    }
}
