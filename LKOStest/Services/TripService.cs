﻿using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                .Include(i => i.Destinations)
                .FirstOrDefault();
        }

        public List<Trip> GetTrips()
        {
            return tripContext.Trips
                .Include(i => i.Destinations)
                .ToList();
        }

        public Trip CreateNewTrip(TripRequest tripRequest)
        {
            var trip = new Trip
            {
                Title = tripRequest.Title,
                Destinations = new List<Destination> { }
            };

            tripContext.Trips.Add(trip);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to save trip data");
            }

            return trip;
        }

        public Trip AddDestinationToTrip(string tripId, Destination destination)
        {
            var trip = tripContext.Trips.Where(trip => trip.Id == tripId)
                .Include(i => i.Destinations)
                .FirstOrDefault();

            trip.Destinations.Add(destination);

            tripContext.Trips.Update(trip);
            tripContext.SaveChanges();

            return GetTrip(tripId);
        }
    }
}
