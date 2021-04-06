using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LKOStest.Entities
{
    public class Warning : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string WarningCode { get; set; }
        public string WarningText { get; set; }
        public bool IsBlocker { get; set; }
        public Visit Visit { get; set; }
    }
}
