using System.Collections.Generic;
using System.Xml.Schema;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Models;
using LKOStest.Services;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Interfaces
{
    public interface ITripService
    {
        public Trip CreateNewTrip(TripRequest tripRequest);
        public Trip AddDestinationToTrip(VisitRequest visitRequest);
        public Trip GetTrip(string tripId);
        public List<Trip> GetTrips();
        public Trip ReorderTripDestinations(string tripId, List<Visit> destinations);
        public Location AddLocation(string tripId, LocationRequest locationRequest);
        public void RemoveVisitFromTrip(string tripId, string visitId);
        public List<Trip> GetTripsByClientId(string clientId);
        public TripValidity ValidateTrip(Trip trip);
        public Visit GetVisit(string visitId);
        public Visit UpdateVisit(string visitId, VisitRequest visitRequest);
    }
}