using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Services;
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
        public IActionResult Post([FromBody] OrganisationRequest organisationRequest)
        {
            try
            {
                var organisation = organisationService.CreateNew(organisationRequest);

                organisation = organisationService.AddUserToOrganisation(organisation.Id, organisationRequest.OwnerId, ContractType.Owner);

                return Ok(organisation);
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

        [HttpPost("{organisationId}/Invite")]
        public IActionResult InviteUserToOrganisation(string organisationId, [FromBody] InviteRequest request)
        {
            try
            {
                var invite = organisationService.InviteToOrganisation(organisationId, request);

                if (invite == null)
                {
                    return StatusCode(500);
                }

                return Ok(invite);
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

        [HttpPost("{organisationId}/SetReviewerCount/{count}")]
        public IActionResult InviteUserToOrganisation(string organisationId, string count)
        {
            try
            {
                var organisation = organisationService.SetOrganisationReviewerCount(organisationId, count);

                if (organisation == null)
                {
                    return StatusCode(500);
                }

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

        [HttpGet("Users/{userId}/Invites")]
        public IActionResult GetInvitesForUser(string userId)
        {
            try
            {
                var invites = organisationService.GetInvitesForUser(userId);

                if (invites == null)
                {
                    return StatusCode(500);
                }

                return Ok(invites);
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

        [HttpPost("Invites/{inviteId}")]
        public IActionResult RespondToInvite(string inviteId, [FromBody] InviteResponse inviteResponse)
        {
            try
            {
                var invites = organisationService.RespondToInvite(inviteId, inviteResponse);

                if (invites == null)
                {
                    return StatusCode(500);
                }

                return Ok(invites);
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

    public class InviteResponse
    {
        public string InviteId { get; set; }
        public string OrganisationId { get; set; }
        public string UserId { get; set; }
        public InviteStatus Status { get; set; }
    }

    public class InviteRequest
    {
        public string OrganisationId { get; set; }
        public string Username { get; set; }
    }
}
