using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LKOStest.Entities
{
    public class Checkin : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public Visit Visit { get; set; }
        public Client Client { get; set; }
        public string Feedback { get; set; }
        public int Rating { get; set; }
    }
}
