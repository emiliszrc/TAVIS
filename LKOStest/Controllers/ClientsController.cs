using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Entities;
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

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return _clientsService.GetClients();
        }

        [HttpGet("{id}")]
        public Client Get(string id)
        {

            return _clientsService.GetClientById(id);
        }

        [HttpPost]
        public Client Post([FromBody] ClientRequest request)
        {
            return _clientsService.CreateNewClient(request);
        }

        [HttpPut("{id}")]
        public Client Put(string id, [FromBody] TripAssignRequest request)
        {
            return _clientsService.AssignTripToClient(id, request.TripId);
        }
    }

    public class TripAssignRequest
    {
        public string TripId { get; set; }
    }

    public interface IClientsService
    {
        public List<Client> GetClients();
        public Client GetClientById(string clientId);
        public Client CreateNewClient(ClientRequest clientRequest);
        public Client AssignTripToClient(string clientId, string tripId);
    }

    public class ClientService : IClientsService
    {
        private TripContext _tripContext;

        public ClientService(TripContext tripContext)
        {
            _tripContext = tripContext;
        }

        public List<Client> GetClients()
        {
            return _tripContext.Clients.ToList();
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
    }

    public class ClientRequest
    {
        public string firstName;
        public string lastName;
    }
}
