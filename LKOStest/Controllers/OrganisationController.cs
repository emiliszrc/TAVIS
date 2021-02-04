using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LKOStest.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace LKOStest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganisationController : ControllerBase
    {
        private IOrganisationService organisationService;
        private IConfiguration configuration;

        public OrganisationController(IOrganisationService organisationService, IConfiguration configuration)
        {
            this.organisationService = organisationService;
            this.configuration = configuration;
        }

        [HttpGet("{organisationId}")]
        public IActionResult Get(string organisationId)
        {
            try
            {
                var organisation = organisationService.GetOrganisationBy(organisationId);

                return Ok(organisation);
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var organisations = organisationService.GetOrganisations();

                return Ok(organisations);
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Organisation organisationRequest)
        {
            try
            {
                var organisation = organisationService.CreateNew(organisationRequest);

                return Created($"{configuration.GetSection("Hostname").Value}/organisation/{organisation.Id}", "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("{organisationId}/Users/{userId}")]
        public IActionResult AddUserToOrganisation(string organisationId, string userId)
        {
            try
            {
                var organisation = organisationService.AddUserToOrganisation(organisationId, userId);

                if (organisation == null)
                {
                    return StatusCode(500);
                }

                return Created($"{configuration.GetSection("Hostname").Value}/organisation/{organisationId}", "");
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{organisationId}/Users/{userId}")]
        public IActionResult Delete(string organisationId, string userId)
        {
            try
            {
                var organisation = organisationService.RemoveUserFromOrganisation(organisationId, userId);

                if (organisation == null)
                {
                    return StatusCode(500);
                }

                return Created($"{configuration.GetSection("Hostname").Value}/organisation/{organisationId}", "");
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}
