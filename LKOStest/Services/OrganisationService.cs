using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace LKOStest.Services
{
    public class OrganisationService : IOrganisationService
    {
        private TripContext tripContext;
        private IUserService userService;

        public OrganisationService(TripContext tripContext, IUserService userService)
        {
            this.tripContext = tripContext;
            this.userService = userService;
        }


        public List<Organisation> GetOrganisations()
        {
            var organisations = tripContext.Organisations
                .Include(o => o.Contracts)
                .ThenInclude(c=>c.User)
                .ToList();

            return organisations.Any() ? organisations : throw new NotFoundException();
        }

        public Invite InviteToOrganisation(string organisationId, InviteRequest request)
        {
            var org = tripContext.Organisations.FirstOrDefault(o => o.Id == organisationId);

            var user = userService.GetUserByUsername(request.Username);

            var invite = new Invite
            {
                Organisation = org,
                User = user
            };

            tripContext.Invites.Add(invite);
            tripContext.SaveChanges();

            return invite;
        }

        public List<Invite> GetInvitesForUser(string userId)
        {
            return tripContext.Invites
                .Include(i=>i.User)
                .Include(i=>i.Organisation)
                .Where(i => i.User.Id == userId)
                .ToList();
        }

        public Invite RespondToInvite(string inviteId, InviteResponse inviteResponse)
        {
            var invite = tripContext.Invites
                .Include(i => i.User)
                .Include(i => i.Organisation)
                .FirstOrDefault(i => i.Id == inviteId);

            switch (inviteResponse.Status)
            {
                case InviteStatus.Accepted:
                    AddUserToOrganisation(invite.Organisation.Id, inviteResponse.UserId);
                    invite.Status = InviteStatus.Accepted;
                    break;
                case InviteStatus.Rejected:
                    invite.Status = InviteStatus.Rejected;
                    break;
            }

            tripContext.Invites.Update(invite);
            tripContext.SaveChanges();

            return invite;
        }


        public Organisation GetOrganisationBy(string organisationId)
        {
            var organisation = tripContext.Organisations
                .Include(o=>o.Contracts)
                .ThenInclude(c => c.User)
                .FirstOrDefault(organisation => organisation.Id == organisationId);

            return organisation ?? throw new NotFoundException();
        }


        public Organisation CreateNew(OrganisationRequest organisationRequest)
        {
            var organisation = new Organisation
            {
                Title = organisationRequest.Title,
                Creator = tripContext.Users.FirstOrDefault(u=>u.Id == organisationRequest.OwnerId)
            };

            tripContext.Organisations.Add(organisation);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to create new organisation");
            }

            return organisation;
        }


        public Organisation AddUserToOrganisation(string organisationId, string userId, ContractType contractType = ContractType.Specialist)
        {
            var user = tripContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new NotFoundException(nameof(user) + " was not found");

            var organisation = tripContext.Organisations.FirstOrDefault(o => o.Id == organisationId);
            if (organisation == null) throw new NotFoundException(nameof(organisation) + " was not found");

            var existingContract = tripContext.Contracts
                .FirstOrDefault(c => c.User.Id == userId && c.Organisation.Id == organisationId);
            if (existingContract != null) return null;

            var contract = new Contract
            {
                User = user,
                Organisation = organisation,
                ContractType = contractType
            };

            tripContext.Contracts.Add(contract);
            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to add user to organisation");
            }

            return GetOrganisationBy(organisationId);

        }


        public Organisation RemoveUserFromOrganisation(string organisationId, string userId)
        {
            var contract = tripContext
                .Contracts
                .FirstOrDefault(c => c.Organisation.Id == organisationId
                                     && c.User.Id == userId);

            if (contract == null)
            {
                throw new NotFoundException();
            }

            tripContext.Contracts.Remove(contract);
            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to remove user from organisation");
            }

            return GetOrganisationBy(organisationId);
        }
    }

    public enum ContractType
    {
        Owner,
        Specialist
    }
}
