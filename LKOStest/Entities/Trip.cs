using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKOStest.Entities
{
    public class Trip : BaseEntity
    {
        public Trip()
        {
            Visits = new List<Visit>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Title { get; set; }

        public virtual List<Visit> Visits { get; set; }

        public virtual User Creator { get; set; }
    }
}
