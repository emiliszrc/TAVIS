﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

            var existingClient = _tripContext.Clients.FirstOrDefault(c => c.Email == clientRequest.Email);

            if (existingClient != null)
            {
                return existingClient;
            }

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
            return _tripContext.Clients
                .Include(c=>c.Participations)
                .ThenInclude(p=>p.Trip)
                .FirstOrDefault(client => client.Email == requestEmail);
        }

        public Checkin CheckinToVisit(CheckinRequest request)
        {
            var client = _tripContext.Clients.FirstOrDefault(c => c.Id == request.ClientId);
            var visit = _tripContext.Visits.FirstOrDefault(v => v.Id == request.VisitId);
            var trip = _tripContext.Trips.FirstOrDefault(t => t.Id == request.TripId);

            var checkin = new Checkin
            {
                Client = client,
                Visit = visit
            };

            _tripContext.Checkins.Add(checkin);
            _tripContext.SaveChanges();

            return checkin;
        }

        public void RemoveCheckins(string clientId, string tripId)
        {
            var checkins = _tripContext.Checkins.Where(c => c.Client.Id == clientId && c.Visit.Trip.Id == tripId);
            _tripContext.Checkins.RemoveRange(checkins);
            _tripContext.SaveChanges();
        }

        public Checkin PostFeedback(FeedbackRequest request)
        {
            var checkin = _tripContext.Checkins.FirstOrDefault(c => 
                c.Visit.Id == request.VisitId 
                && c.Client.Id == request.UserId);

            checkin.Feedback = request.Text;
            checkin.Rating = request.Rating;
            _tripContext.Checkins.Update(checkin);
            _tripContext.SaveChanges();

            return checkin;
        }

        public Client SetPassword(PasswordRequest passwordRequest)
        {
            var client = _tripContext.Clients.FirstOrDefault(c => c.Id == passwordRequest.Id);

            if (client == null)
            {
                return new Client();
            }

            client.Password = ComputeSha256Hash(passwordRequest.NewPassword);
            client.DefaultPassword = string.Empty;

            _tripContext.Clients.Update(client);
            _tripContext.SaveChanges();

            return client;
        }

        public List<Checkin> GetCheckins(string clientId, string tripId)
        {
            return _tripContext.Checkins
                .Include(c=>c.Visit)
                .Include(c=>c.Visit)
                .Where(c => c.Client.Id == clientId)
                .ToList();
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}