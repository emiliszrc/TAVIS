using System;

namespace LKOStest.Services
{
    public class VisitRequest
    {
        public string TripId { get; set; }
        public string LocationId { get; set; }
        public string VisitationIndex { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
    }
}