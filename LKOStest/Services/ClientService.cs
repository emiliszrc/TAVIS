using System;
using System.Collections.Generic;
using System.Linq;
using LKOStest.Entities;
using Microsoft.EntityFrameworkCore;

namespace LKOStest.Controllers
{
    public class ClientService : IClientsService
    {
        private TripContext _tripContext;

        public ClientService(TripContext tripContext)
        {
            _tripContext = tripContext;
        }

        public List<Client> GetClientsForTrip(string id)
        {
            var clients =  _tripContext.Clients
                .Include(c=>c.Participations)
                .Where(c=>c.Participations.Any(p=>p.Trip.Id == id))
                .ToList();

            return clients;
        }

        public Client GetClientById(string clientId)
        {
            return _tripContext.Clients.FirstOrDefault(client => client.Id == clientId);
        }

        public Client CreateNewClient(ClientRequest clientRequest)
        {
            var client = Client.From(clientRequest);

            _tripContext.Clients.Add(client);

            if (_tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to create new client");
            }

            return client;
        }

        public Client AssignTripToClient(string clientId, string tripId)
        {
            var client = _tripContext.Clients.FirstOrDefault(c => c.Id == clientId);
            var trip = _tripContext.Trips.FirstOrDefault(t => t.Id == tripId);

            var participation = new Participation
            {
                Client = client,
                Trip = trip
            };

            _tripContext.Participations.Add(participation);

            if (_tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to assign trip to client");
            }

            return client;
        }

        public Client GetClientByEmail(string requestEmail)
        {
            return _tripContext.Clients.FirstOrDefault(client => client.Email == requestEmail);
        }

        public Checkin CheckinToVisit(CheckinRequest request)
        {
            throw new NotImplementedException();
        }

        public Checkin PostFeedback(FeedbackRequest request)
        {
            throw new NotImplementedException();
        }
    }
}