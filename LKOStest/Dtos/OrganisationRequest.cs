using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LKOStest.Dtos
{
    public class OrganisationRequest
    {
        public string Title { get; set; }
        public int RequiredApprovals { get; set; }
        public string OwnerId { get; set; } 
    }
}
