using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LKOStest.Entities;
using LKOStest.Interfaces;
using LKOStest.Services;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IClientsService _clientsService;
        private IEmailService _emailService;
        private ITripService _tripService;

        public ClientsController(IClientsService clientsService, IEmailService emailService, ITripService tripService)
        {
            _clientsService = clientsService;
            _emailService = emailService;
            _tripService = tripService;
        }

        [HttpGet("forTrip/{id}")]
        public IActionResult GetForTrip(string id)
        {
            return Ok(_clientsService.GetClientsForTrip(id));
        }
        [HttpGet("{clientId}/GetCheckins/{tripId}")]
        public IActionResult GetCheckins(string clientId, string tripId)
        {
            return Ok(_clientsService.GetCheckins(clientId, tripId));
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(_clientsService.GetClientById(id));
        }

        [HttpPost]
        public Client Post([FromBody] ClientRequest request)
        {
            return _clientsService.CreateNewClient(request);
        }

        [HttpPost]
        [Route("invite")]
        public IActionResult Put([FromBody] ParticipationRequest request)
        {
            var existingClient = _clientsService.GetClientByEmail(request.Email);

            if (existingClient != null)
            {
                if (existingClient.Participations.Any(p => p.Trip.Id == request.TripId))
                {
                    return BadRequest("Client already invited to trip");
                }
                else
                {
                    _clientsService.AssignTripToClient(existingClient.Id, request.TripId);
                    return Ok(_clientsService.GetClientsForTrip(request.TripId));
                }
            }

            var password = PasswordGenerator.GetUniqueKey(4);

            var clientRequest = new ClientRequest
            {
                DefaultPassword = password,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            
            var client = _clientsService.CreateNewClient(clientRequest);
            _clientsService.AssignTripToClient(client.Id, request.TripId);

            return Ok(_clientsService.GetClientsForTrip(request.TripId));
        }

        [HttpPost]
        [Route("sendInviteEmail/byTrip/{tripId}")]
        public IActionResult SendForTripId(string tripId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("sendInviteEmail/byClient/{clientId}/{tripId}/{resend}")]
        public IActionResult SendForClientId(string clientId, string tripId, bool resend)
        {
            if (!resend)
            {
                var emails = _emailService.GetClientEmails(clientId);
                if (emails.Any(e => e.Trip != null && e.Trip.Id == tripId))
                {
                    return BadRequest("Email already sent for trip");
                }
            }

            var client = _clientsService.GetClientById(clientId);
            var trip = _tripService.GetTrip(tripId);

            var emailRequest = new EmailRequest
            {
                GeneratedPassword = client.DefaultPassword,
                Receiver = $"{client.Name} {client.Surname}",
                ReceiverEmail = client.Email,
                SenderEmail = "em.zareckis@gmail.com",
                TripTitle = trip.Title
            };

            var success = false;
            if (string.IsNullOrEmpty(emailRequest.GeneratedPassword))
            {
                success = _emailService.SendAfterLogin(emailRequest);
            }
            else
            {
                success = _emailService.SendEmail(emailRequest);
            }

            var email = new SentEmail
            {
                    ReceiverEmail = emailRequest.ReceiverEmail,
                    SenderEmail = emailRequest.SenderEmail,
                    Success = success,
                    Client = client,
                    Trip = trip
            };

            _emailService.LogSentEmail(email);

            return Ok(success);
        }

        [HttpGet("byEmail/{email}")]
        public IActionResult GetByEmail(string email)
        {
            return Ok(_clientsService.GetClientByEmail(email));
        }

        [HttpPost("{clientId}/PostCheckins/{tripId}")]
        public IActionResult Checkin(string clientId, string tripId, [FromBody] CheckinPosts request)
        {
            _clientsService.RemoveCheckins(clientId, tripId);
            request.Items.ForEach(i=>_clientsService.CheckinToVisit(i));
            return Ok();
        }

        [HttpPost("{clientId}/PostFeedback/{visitId}")]
        public IActionResult Feedback(string clientId, string visitId, [FromBody] FeedbackRequest request)
        {
            _clientsService.PostFeedback(request);
            return Ok();
        }

        [HttpPost("{id}/SetPassword")]
        public IActionResult SetPassword(string id, [FromBody] PasswordRequest passwordRequest)
        {
            return Ok(_clientsService.SetPassword(passwordRequest));
        }
    }

    public class PasswordRequest
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class CheckinPosts
    {
        public List<CheckinRequest> Items { get; set; }
    }

    public class CheckinRequest
    {
        public string TripId { get; set; }
        public string VisitId { get; set; }
        public string ClientId { get; set; }
    }

    public class PasswordGenerator
    {
        internal static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public static string GetUniqueKey(int size)
        {
            var data = new byte[4 * size];
            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        public static string GetUniqueKeyOriginal_BIASED(int size)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }

    public class ParticipationRequest
    {
        public string TripId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public interface IClientsService
    {
        public List<Client> GetClientsForTrip(string id);
        public Client GetClientById(string clientId);
        public Client CreateNewClient(ClientRequest clientRequest);
        public Client AssignTripToClient(string clientId, string tripId);
        public Client GetClientByEmail(string requestEmail);
        public Checkin CheckinToVisit(CheckinRequest request);
        public Checkin PostFeedback(FeedbackRequest request);
        public Client SetPassword(PasswordRequest passwordRequest);
        public List<Checkin> GetCheckins(string clientId, string tripId);
        public void RemoveCheckins(string clientId, string tripId);
    }

    public class FeedbackRequest
    {
        public string TripId { get; set; }
        public string VisitId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
    }

    public class ClientRequest
    {
        public string Email { get; set; }
        public string DefaultPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
