using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Models;

namespace LKOStest.Services
{
    public class TripService : ITripService
    {
        private TripContext tripContext;
        private IValidityFactory validityFactory;

        public TripService(TripContext tripContext, IValidityFactory validityFactory)
        {
            this.tripContext = tripContext;
            this.validityFactory = validityFactory;
        }

        public Trip GetTrip(string tripId)
        {
            return tripContext.Trips.Where(trip => trip.Id == tripId)
                .Include(i => i.Visits)
                .ThenInclude(v => v.Location)
                .Include(i => i.Creator)
                .Include(t => t.Organisation)
                .FirstOrDefault();
        }

        public List<Trip> GetTrips()
        {
            return tripContext.Trips
                .Include(i => i.Visits).ThenInclude(v => v.Location)
                .Include(i => i.Creator)
                .Include(t => t.Organisation)
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

            return tripContext.Locations.FirstOrDefault(l => l.Id == location.Id);
        }

        public void RemoveVisitFromTrip(string tripId, string visitId)
        {
            var visit = tripContext.Visits.FirstOrDefault(v => v.Id == visitId);
            tripContext.Trips.FirstOrDefault(t => t.Id == tripId).Visits.Remove(visit);
            tripContext.SaveChanges();
        }

        public List<Trip> GetTripsByClientId(string clientId)
        {
            var tripIds = tripContext.Participations
                .Where(p => p.Client.Id == clientId)
                .Select(p => p.Trip.Id)
                .ToList();

            return tripIds.Select(GetTrip).ToList();
        }

        public Trip CreateNewTrip(TripRequest tripRequest)
        {
            var creator = tripContext.Users.FirstOrDefault(u => u.Id == tripRequest.CreatorId);
            var organisation = tripContext.Organisations.FirstOrDefault(o => o.Id == tripRequest.OrganisationId);

            var trip = new Trip
            {
                Title = tripRequest.Title,
                Visits = new List<Visit>(),
                Creator = creator,
                Organisation = organisation,
                TripStatus = TripStatus.New
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

        public TripValidity ValidateTrip(Trip trip)
        {
            return validityFactory.ValidateTrip(trip);
        }

        public Visit GetVisit(string visitId)
        {
            return tripContext.Visits
                .Where(v => v.Id.Equals(visitId))
                .Include(v => v.Location)
                .Include(v => v.Trip)
                .FirstOrDefault();
        }

        public Visit UpdateVisit(string visitId, VisitRequest visitRequest)
        {
            var visit = tripContext.Visits.FirstOrDefault(v => v.Id == visitId);
            var x = visitRequest.Arrival.ToLocalTime();
            var z = visitRequest.Arrival.ToUniversalTime();
            var y = visitRequest.Arrival;//.AddHours(3);
            visit.Arrival = visitRequest.Arrival;//.AddHours(3);
            visit.Departure = visitRequest.Departure;//.AddHours(3);
            visit.Location = tripContext.Locations.FirstOrDefault(l => l.Id == visitRequest.LocationId);

            tripContext.Visits.Update(visit);
            tripContext.SaveChanges();

            return GetVisit(visitId);
        }

        public List<Comment> GetComments(string tripId)
        {
            var comments = tripContext.Reviews.Where(r => r.Trip.Id == tripId)
                .Include(r => r.Comments)
                .SelectMany(r => r.Comments)
                .ToList();

            comments.RemoveAll(c => c.ParentComment != null);

            return comments;
        }

        public List<Trip> GetUserTrips(string userId)
        {
            var trips = tripContext.Trips
                .Include(i => i.Visits).ThenInclude(v => v.Location)
                .Include(i => i.Creator)
                .Include(t => t.Organisation)
                .Where(t => t.Creator.Id == userId)
                .ToList();

            return trips;
        }

        public List<Trip> GetOrganisationTrips(string userId)
        {
            var trips = tripContext.Trips
                .Include(i => i.Visits).ThenInclude(v => v.Location)
                .Include(i => i.Creator)
                .Include(t => t.Organisation)
                .ThenInclude(o => o.Contracts)
                .ThenInclude(c => c.User)
                .Where(t => t.Organisation.Contracts.Any(c => c.User.Id == userId) && t.Creator.Id != userId)
                .ToList();

            return trips;
        }

        public void DeleteTrip(string tripId)
        {
            var trip = tripContext.Trips.FirstOrDefault(t => t.Id == tripId);

            tripContext.Trips.Remove(trip);
            tripContext.SaveChanges();
        }

        public Review GetReview(string tripId)
        {
            var review = tripContext.Reviews
                .Include(r => r.Reviewers)
                .FirstOrDefault(r => r.Trip.Id == tripId && r.Status == ReviewStatus.New);

            return review;
        }

        public List<Trip> GetFinalTrips(string userId)
        {
            var trips = tripContext.Trips
                .Include(i => i.Visits).ThenInclude(v => v.Location)
                .Include(i => i.Creator)
                .Include(t => t.Organisation)
                .Where(t=>t.TripStatus == TripStatus.Final)
                .Where(t => t.Organisation.Contracts.Any(c => c.User.Id == userId) || t.Creator.Id == userId)
                .ToList();

            return trips;
        }

        public void RestoreStatuses()
        {
            var trips = tripContext.Trips
                .Include(i => i.Visits).ThenInclude(v => v.Location)
                .Include(i => i.Creator)
                .Include(t => t.Organisation)
                .ThenInclude(o => o.Contracts)
                .ThenInclude(c => c.User)
                .ToList();


            foreach (var trip in trips)
            {
                var reviews = tripContext.Reviews
                    .Where(r => r.Trip.Id == trip.Id)
                    .ToList();

                var inReview = reviews.Any(r => r.Status == ReviewStatus.New);

                if (inReview)
                {
                    trip.TripStatus = TripStatus.InReview;
                    tripContext.Trips.Update(trip);
                    tripContext.SaveChanges();
                }

                var final = reviews.Any(r => r.Status == ReviewStatus.Approved);

                if (final)
                {
                    trip.TripStatus = TripStatus.Final;
                    tripContext.Trips.Update(trip);
                    tripContext.SaveChanges();
                }
            }
        }

        public Trip FinalizeTrip(string tripId, string userId)
        {
            var trip = GetTrip(tripId);

            if (trip.Creator.Id != userId)
            {
                throw new Exception("Finalizer is not the creator");
            }

            trip.TripStatus = TripStatus.Final;
            tripContext.Trips.Update(trip);
            tripContext.SaveChanges();

            return GetTrip(trip.Id);
        }

        public Trip ReuseTrip(string tripId, ReuseRequest reuseRequest)
        {
            var trip = GetTrip(tripId);

            var reusableTrip = trip;

            reusableTrip.Id = null;

            if (reuseRequest.Title != string.Empty)
            {
                reusableTrip.Title = reuseRequest.Title;
            }

            var firstVisit = reusableTrip.Visits.OrderBy(v => v.VisitationIndex).FirstOrDefault();

            var pushTimespan = reuseRequest.StartDate - firstVisit.Arrival;

            foreach (var visit in reusableTrip.Visits)
            {
                visit.Arrival += pushTimespan;
                visit.Departure += pushTimespan;
            }

            reusableTrip.TripStatus = TripStatus.New;

            var creator = tripContext.Users.FirstOrDefault(c=>c.Id == reuseRequest.CreatorId);

            reusableTrip.Creator = creator;

            tripContext.Add(reusableTrip);
            tripContext.SaveChanges();

            return reusableTrip;
        }
    }

    public class TripResponse
    {
        public TripResponse(string id, string title, List<Visit> visits, User creator, Organisation organisation, bool isInActiveReview)
        {
            Id = id;
            Title = title;
            Visits = visits;
            Creator = creator;
            Organisation = organisation;
            IsInActiveReview = isInActiveReview;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public List<Visit> Visits { get; set; }

        public User Creator { get; set; }
        public Organisation Organisation { get; set; }
        public bool IsInActiveReview { get; set; }
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

    public interface IValidityFactory
    {
        public TripValidity ValidateTrip(Trip trip);
    }

    public class ValidityFactory : IValidityFactory
    {
        private ISearchService searchService;
        private IDistanceMatrixService distanceMatrixService;

        public ValidityFactory(ISearchService searchService, IDistanceMatrixService distanceMatrixService)
        {
            this.searchService = searchService;
            this.distanceMatrixService = distanceMatrixService;
        }

        public TripValidity ValidateTrip(Trip trip)
        {
            var reasons = ValidateTripSpecifics(trip);

            foreach (var tripVisit in trip.Visits)
            {
                reasons.AddRange(ValidateTripVisit(tripVisit, trip));
            }

            var validity = new TripValidity
            {
                Reasons = reasons,
                IsValid = GetValidity(reasons)
            };

            return validity;
        }

        private List<Reason> ValidateTripVisit(Visit visit, Trip trip)
        {
            var reasons = new List<Reason>();
            var countryCode = searchService.GetCountryCode(visit.Location.Latitude, visit.Location.Longtitude).CountryCode;
            var countryHolidays = searchService.SearchForHolidays(countryCode, DateTime.UtcNow.Year.ToString());

            //overlapping holidays check
            var hittingHolidays = countryHolidays.Where(h => h.Date > visit.Arrival && h.Date < visit.Departure).ToList();

            if (hittingHolidays.Any())
            {
                foreach (var holiday in hittingHolidays)
                {
                    reasons.Add(new Reason("1",
                        $"Visit is occuring during a public holiday: {holiday.Name}",
                        visit.Id, false));
                }
            }

            //overlapping visits check
            var hittingVisits = trip.Visits.Where(v => visit.Arrival > v.Arrival && visit.Arrival < v.Departure
                                                       || visit.Departure > v.Arrival && visit.Departure < v.Departure
                                                       || visit.Arrival < v.Arrival && visit.Departure > v.Departure).ToList();

            if (hittingVisits.Any())
            {
                foreach (var v in hittingVisits)
                {
                    reasons.Add(new Reason("2",
                        $"Visit is overlapping with another visit: {v.Location.Title}",
                        visit.Id, true));
                }
            }

            if (visit.Departure < visit.Arrival)
            {
                reasons.Add(new Reason("8",
                    $"Departure date is earlier than arrival for {visit.Id}",
                    visit.Id, true));
            }

            //check worktimes
            var visitDaysOfTheWeek = new List<DayOfWeek>();
            var workingDays = searchService.GetAttractionDetails(visit.Location.LocationId).hours;
            var workingDayDictionary = new Dictionary<DayOfWeek, WorkTime>
            {
                {DayOfWeek.Monday, workingDays.week_ranges[0].FirstOrDefault()},
                {DayOfWeek.Tuesday, workingDays.week_ranges[1].FirstOrDefault()},
                {DayOfWeek.Wednesday, workingDays.week_ranges[2].FirstOrDefault()},
                {DayOfWeek.Thursday, workingDays.week_ranges[3].FirstOrDefault()},
                {DayOfWeek.Friday, workingDays.week_ranges[4].FirstOrDefault()},
                {DayOfWeek.Saturday, workingDays.week_ranges[5].FirstOrDefault()},
                {DayOfWeek.Sunday, workingDays.week_ranges[6].FirstOrDefault()}
            };

            for (var date = visit.Arrival; date.Date <= visit.Departure.Date; date = date.AddDays(1))
            {
                visitDaysOfTheWeek.Add(date.DayOfWeek);
            }

            if (visitDaysOfTheWeek.Count() > 1)
            {
                reasons.Add(new Reason("7",
                    $"Spending more than a day in a location",
                    visit.Id, false));
            }

            var worktime = workingDayDictionary.TryGetValue(visit.Arrival.DayOfWeek, out var value) ? value : null;

            if (worktime == null)
            {
                reasons.Add(new Reason("5",
                    $"Location does not operate on the day of arrival",
                    visit.Id, true));
            }
            else
            {
                if ((visit.Arrival.Hour * 60 + visit.Arrival.Minute) < worktime.open_time)
                {
                    reasons.Add(new Reason("4",
                        $"Arriving before worktime: arriving at {visit.Arrival}, opens at {TimeSpan.FromMinutes(worktime.open_time).Hours}:{TimeSpan.FromMinutes(worktime.open_time).Minutes}",
                        visit.Id, false));
                }

                if ((visit.Arrival.Hour * 60 + visit.Arrival.Minute) > worktime.close_time)
                {
                    reasons.Add(new Reason("6",
                        $"Arriving after worktime: arriving at {visit.Arrival}, closes at {TimeSpan.FromMinutes(worktime.close_time).Hours}:{TimeSpan.FromMinutes(worktime.close_time).Minutes}",
                        visit.Id, true));
                }
            }

            //distance between visits check
            var currentVisitationIndex = Convert.ToInt32(visit.VisitationIndex);
            var nextVisitationIndex = currentVisitationIndex - 1;
            var previousVisit = trip.Visits.FirstOrDefault(v => Convert.ToInt32(v.VisitationIndex) == nextVisitationIndex);

            if (previousVisit == null)
            {
                return reasons;
            }

            var distanceMatrix = distanceMatrixService.GetDistance(visit.Location.Address, previousVisit.Location.Address);

            var span = visit.Arrival - previousVisit.Departure;
            var totalMinutes = Convert.ToInt32(span.TotalMinutes);
            var totalActualSeconds = distanceMatrix.rows.FirstOrDefault().elements.FirstOrDefault().duration.value;

            //add mandatory stops
            var breakAmount = totalActualSeconds / 10800;
            var mandatoryActualSpan = span + TimeSpan.FromMinutes(15 * breakAmount);
            
            if (totalActualSeconds / 60 > Convert.ToInt32(mandatoryActualSpan.TotalMinutes))
            {
                reasons.Add(new Reason("3",
                    $"Impossible to drive in time from {previousVisit.Location.Address} to {visit.Location.Address}.",
                    visit.Id, true));
            }

            if (mandatoryActualSpan > TimeSpan.FromHours(16))
            {
                reasons.Add(new Reason("3",
                    $"Cannot drive longer than 16 hours due to regulations. From {previousVisit.Location.Address} to {visit.Location.Address}.",
                    visit.Id, true));
            }

            return reasons;
        }

        private List<Reason> ValidateTripSpecifics(Trip trip)
        {
            return new List<Reason>();
        }

        private bool GetValidity(List<Reason> reasons)
        {
            return !reasons.Any(r => r.IsBlocker);
        }
    }
}
