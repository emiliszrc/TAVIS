using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace LKOStest.Services
{
    public class OrganisationService : IOrganisationService
    {
        private TripContext tripContext;

        public OrganisationService(TripContext tripContext)
        {
            this.tripContext = tripContext;
        }


        public List<Organisation> GetOrganisations()
        {
            var organisations = tripContext.Organisations
                .Include(o => o.Contracts)
                .ThenInclude(c=>c.User)
                .ToList();

            return organisations.Any() ? organisations : throw new NotFoundException();
        }


        public Organisation GetOrganisationBy(string organisationId)
        {
            var organisation = tripContext.Organisations
                .Include(o=>o.Contracts)
                .ThenInclude(c => c.User)
                .FirstOrDefault(organisation => organisation.Id == organisationId);

            return organisation ?? throw new NotFoundException();
        }


        public Organisation CreateNew(Organisation organisation)
        {
            tripContext.Organisations.Add(organisation);

            if (tripContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to create new organisation");
            }

            return organisation;
        }


        public Organisation AddUserToOrganisation(string organisationId, string userId)
        {
            var user = tripContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new NotFoundException(nameof(user) + " was not found");

            var organisation = tripContext.Organisations.FirstOrDefault(o => o.Id == organisationId);
            if (organisation == null) throw new NotFoundException(nameof(organisation) + " was not found");

            var existingContract = tripContext.Contracts
                .FirstOrDefault(c => c.User.Id == userId && c.Organisation.Id == organisationId);
            if (existingContract != null) throw new ObjectAlreadyExists();

            var contract = new Contract
            {
                User = user,
                Organisation = organisation
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
}
