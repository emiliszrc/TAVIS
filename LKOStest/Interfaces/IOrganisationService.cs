using System.Collections.Generic;
using LKOStest.Dtos;
using LKOStest.Entities;
using LKOStest.Services;

namespace LKOStest.Controllers
{
    public interface IOrganisationService
    {
        Organisation GetOrganisationBy(string organisationId);
        Organisation CreateNew(OrganisationRequest organisation);
        Organisation AddUserToOrganisation(string organisationId, string userId, ContractType contractType = ContractType.Specialist);
        Organisation RemoveUserFromOrganisation(string organisationId, string userId);
        List<Organisation> GetOrganisations();
        Invite InviteToOrganisation(string organisationId, InviteRequest request);
        List<Invite> GetInvitesForUser(string userId);
        Invite RespondToInvite(string inviteId, InviteResponse inviteResponse);
    }
}