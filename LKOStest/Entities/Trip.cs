﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKOStest.Entities
{
    public class Trip : BaseEntity
    {
        public Trip()
        {
            Destinations = new List<Destination>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Title { get; set; }

        public virtual List<Destination> Destinations { get; set; }

        public virtual User Creator { get; set; }
    }
}
