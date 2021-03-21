using System;
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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public Trip Trip { get; set; }

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();

        public virtual List<Reviewer> Reviewers { get; set; }

        public virtual List<Approval> Approvals { get; set; }
    }
}
