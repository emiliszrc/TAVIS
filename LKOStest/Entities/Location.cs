using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKOStest.Entities
{
    [Table("Locations")]
    public class Location : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string LocationId { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public string Longtitude { get; set; }
        
        public string Latitude { get; set; }

        public virtual List<Visit> Visits { get; set; }
    }
}
