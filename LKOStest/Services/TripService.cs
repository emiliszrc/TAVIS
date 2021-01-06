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
                .ThenInclude(v=>v.Location)
                .Include(i=>i.Creator)
                .FirstOrDefault();
        }

        public List<Trip> GetTrips()
        {
            return tripContext.Trips
                .Include(i => i.Visits).ThenInclude(v=> v.Location)
                .Include(i => i.Creator)
                .ToList();
        }

        public Trip ReorderTripDestinations(string tripId, List<Visit> visitRequests)
        {
            var trip = tripContext.Trips.Where(trip => trip.Id == tripId)
                .Include(i => i.Visits)
                .FirstOrDefault();

            foreach (var visitRequest in visitRequests)
            {
                foreach (var visit in trip.Visits)
                {
                    if (visit.Id == visitRequest.Id)
                    {
                        visit.VisitationIndex = visitRequest.VisitationIndex;
                    }
                }
            }

            tripContext.Trips.Update(trip);
            tripContext.SaveChanges();

            return GetTrip(tripId);
        }

        public Location AddLocation(string tripId, LocationRequest locationRequest)
        {
            var location = new Location
            {
                Address = locationRequest.Address,
                Latitude = locationRequest.Latitude,
                Longtitude = locationRequest.Longtitude,
                LocationId = locationRequest.LocationId,
                Title = locationRequest.Title,
                Type = locationRequest.Type
            };

            tripContext.Locations.Add(location);

            tripContext.SaveChanges();

            return tripContext.Locations.FirstOrDefault(l=> l.Id == location.Id);
        }

        public void RemoveVisitFromTrip(string tripId, string visitId)
        {
            var visit = tripContext.Visits.FirstOrDefault(v => v.Id == visitId);
            tripContext.Trips.FirstOrDefault(t => t.Id == tripId).Visits.Remove(visit);
            tripContext.SaveChanges();
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

    public class LocationRequest
    {
        public string LocationId { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Index { get; set; }

        public string Address { get; set; }

        public string Longtitude { get; set; }

        public string Latitude { get; set; }
    }
}
