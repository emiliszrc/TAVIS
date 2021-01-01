using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LKOStest.Entities
{
    public class Visit : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string VisitationIndex { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public Trip Trip { get; set; }
        public Location Location { get; set; }
    }
}
