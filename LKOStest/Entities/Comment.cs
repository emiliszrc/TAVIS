﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKOStest.Entities
{
    public class Comment : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Text { get; set; }

        public Location Location { get; set; }

        public Comment ParentComment { get; set; }

        public Review Review { get; set; }

        public virtual List<Comment> ChildComments { get; set; }
    }
}