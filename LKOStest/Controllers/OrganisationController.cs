using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationController : ControllerBase
    {
        private IOrganisationService organisationService;

        public OrganisationController(IOrganisationService organisationService)
        {
            this.organisationService = organisationService;
        }

        [HttpGet("{organisationId}")]
        public Organisation Get(string organisationId)
        {
            return organisationService.GetOrganisationBy(organisationId);
        }

        [HttpGet("")]
        public List<Organisation> Get()
        {
            return organisationService.GetOrganisations();
        }

        [HttpPost]
        public void Post([FromBody] Organisation organisation)
        {
            organisationService.CreateNew(organisation);
        }

        [HttpPost("{organisationId}/{userId}")]
        public void Put(string organisationId, string userId)
        {
            organisationService.AddUserToOrganisation(organisationId, userId);
        }

        [HttpDelete("{organisationId}/{userId}")]
        public void Delete(string organisationId, string userId)
        {
            organisationService.RemoveUserFromOrganisation(organisationId, userId);
        }
    }
}
