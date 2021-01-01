using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LKOStest.Services
{
    public class TripService : ITripService
    {
        private TripContext tripContext;

        public TripService(TripContext tripContext)
        {
            this.tripContext = tripContext;
        }

        public Trip GetTrip(string tripId)
        {
            return tripContext.Trips.Where(trip => trip.Id == tripId)
                .Include(i => i.Visits)
                .FirstOrDefault();
        }

        public List<Trip> GetTrips()
        {
            return tripContext.Trips
                .Include(i => i.Visits)
                .Include(i => i.Creator)
                .ToList();
        }

        public Trip ReorderTripDestinations(string tripId, List<Location> destinations)
        {
            var trip = tripContext.Trips.Where(trip => trip.Id == tripId)
                .Include(i => i.Visits)
                .FirstOrDefault();

            foreach (var destination in destinations)
            {
                foreach (var visit in trip.Visits)
                {
                    if (visit.Id == destination.Id)
                    {
                        visit.VisitationIndex = destination.Index;
                    }
                }
            }

            tripContext.Trips.Update(trip);
            tripContext.SaveChanges();

            return GetTrip(tripId);
        }

        public Trip CreateNewTrip(TripRequest tripRequest)
        {
            var creator = tripContext.Users.FirstOrDefault(u => u.Id == tripRequest.CreatorId); 

            var trip = new Trip
            {
                Title = tripRequest.Title,
                Visits = new List<Visit>(),
                Creator = creator
            };

            tripContext.Trips.Add(trip);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to save trip data");
            }

            return trip;
        }

        public Trip AddDestinationToTrip(VisitRequest visitRequest)
        {
            var trip = tripContext.Trips.Where(trip => trip.Id == visitRequest.TripId)
                .Include(i => i.Visits)
                .FirstOrDefault();

            visitRequest.VisitationIndex = trip.Visits.Count.ToString();

            var visit = new Visit
            {
                Arrival = visitRequest.Arrival,
                Departure = visitRequest.Departure,
                Location = tripContext.Locations.FirstOrDefault(l => l.Id == visitRequest.LocationId),
                Trip = trip,
                VisitationIndex = visitRequest.VisitationIndex
            };

            trip.Visits.Add(visit);

            tripContext.Trips.Update(trip);
            tripContext.SaveChanges();

            return GetTrip(visitRequest.TripId);
        }
    }

    public class VisitRequest
    {
        public string TripId { get; set; }
        public string LocationId { get; set; }
        public string VisitationIndex { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
    }
}
