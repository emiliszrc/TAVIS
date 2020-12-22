using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LKOStest.Controllers;
using LKOStest.Entities;

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
            return tripContext.Organisations.ToList();
        }

        public Organisation GetOrganisationBy(string organisationId)
        {
            return tripContext.Organisations.FirstOrDefault(organisation => organisation.Id == organisationId);
        }


        public void CreateNew(Organisation organisation)
        {
            tripContext.Organisations.Add(organisation);
            tripContext.SaveChanges();
        }

        public void AddUserToOrganisation(string organisationId, string userId)
        {
            var user = tripContext.Users.FirstOrDefault(user => user.Id == userId);
            tripContext.Organisations.FirstOrDefault(organisation => organisation.Id == organisationId)?.Users.Add(user);
            tripContext.SaveChanges();
        }

        public void RemoveUserFromOrganisation(string organisationId, string userId)
        {
            var user = tripContext.Users.FirstOrDefault(user => user.Id == userId);
            tripContext.Organisations.FirstOrDefault(organisation => organisation.Id == organisationId).Users.Remove(user);
            tripContext.SaveChanges();
        }
    }
}
