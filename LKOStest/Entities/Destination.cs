using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKOStest.Entities
{
    [Table("Destinations")]
    public class Destination : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string LocationId { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public string Longtitude { get; set; }
        
        public string Latitude { get; set; }
}
}
