using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Entities;

namespace LKOStest.Models
{
    public class TripValidity
    {
        public IEnumerable<Reason> Reasons { get; set; }

        public bool IsValid { get; set; }
    }

    public class Reason
    {
        public Reason(string code, string text, bool blocker)
        {
            Code = code;
            Text = text;
            IsBlocker = blocker;
            VisitId = string.Empty;
        }

        public Reason(string code, string text, string visitId, bool blocker)
        {
            Code = code;
            Text = text;
            VisitId = visitId;
            IsBlocker = blocker;
        }

        public string Code { get; set; }

        public string Text { get; set; }

        public string VisitId { get; set; }

        public bool IsBlocker { get; set; }
    }
}
