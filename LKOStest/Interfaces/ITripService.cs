using System.Collections.Generic;
using LKOStest.Dtos;
using LKOStest.Entities;

namespace LKOStest.Interfaces
{
    public interface ITripService
    {
        public Trip CreateNewTrip(TripRequest tripRequest);
        public Trip AddDestinationToTrip(string tripId, Destination destination);
        public Trip GetTrip(string tripId);
        public List<Trip> GetTrips();
    }
}