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

        public string DestinationIndex { get; set; }

        public Review Review { get; set; }
    }
}