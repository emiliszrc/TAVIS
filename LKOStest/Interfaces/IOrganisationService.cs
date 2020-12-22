using System.Collections.Generic;
using LKOStest.Entities;

namespace LKOStest.Controllers
{
    public interface IOrganisationService
    {
        Organisation GetOrganisationBy(string organisationId);
        void CreateNew(Organisation organisation);
        void AddUserToOrganisation(string organisationId, string userId);
        void RemoveUserFromOrganisation(string organisationId, string userId);
        List<Organisation> GetOrganisations();
    }
}