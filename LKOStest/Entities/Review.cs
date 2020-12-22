﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LKOStest.Entities
{
    public class Review : BaseEntity
    {
        public Review()
        {
            Comments = new List<Comment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public Trip Trip { get; set; }

        public User User { get; set; }

        public string ApprovalStatus { get; set; }

        public virtual List<Comment> Comments { get; set; } 
    }
}
