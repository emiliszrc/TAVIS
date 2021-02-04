using System.Collections.Generic;
using LKOStest.Entities;

namespace LKOStest.Controllers
{
    public interface IOrganisationService
    {
        Organisation GetOrganisationBy(string organisationId);
        Organisation CreateNew(Organisation organisation);
        Organisation AddUserToOrganisation(string organisationId, string userId);
        Organisation RemoveUserFromOrganisation(string organisationId, string userId);
        List<Organisation> GetOrganisations();
    }
}