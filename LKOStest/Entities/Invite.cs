using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKOStest.Entities
{
    public class Invite : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public Organisation Organisation { get; set; }
        public User User { get; set; }
        public InviteStatus Status { get; set; }
    }

    public enum InviteStatus
    {
        New,
        Accepted,
        Rejected,
        RejectedAcknowledged
    }
}
