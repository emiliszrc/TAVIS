using System.Collections.Generic;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Services;

namespace LKOStest.Interfaces
{
    public interface ITripService
    {
        public Trip CreateNewTrip(TripRequest tripRequest);
        public Trip AddDestinationToTrip(VisitRequest visitRequest);
        public Trip GetTrip(string tripId);
        public List<Trip> GetTrips();
        public Trip ReorderTripDestinations(string tripId, List<Location> destinations);
    }
}