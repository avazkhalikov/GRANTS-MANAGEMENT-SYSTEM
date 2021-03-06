using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.DataAccess
{
    public interface IOrganizationRepository
    {
        Organization getOrganizationByID(int id);
        IEnumerable<LegalStatusList> GetLegalStatusList();
        bool DeleteOrganization(int ProposalID);
        bool UpdateOrganization(Organization org, int ProposalID);
        Organization GetOrganizationOfCurrentProposal(int ProposalID);
        General GetOrganizationGeneralOfCurrentProposal(int ProposalID);
        bool UpdateOrganizationGeneral(General gen);
        Organization GetOrganizationContactOfCurrentProposal(int ProposalID);
        IEnumerable<Organization> SearchOrganizationByName(string Name);
        Organization GetOrganizationGeneralByID(int id);
        Organization GetOrganizationContactByID(int id);
        IEnumerable<Address> GetOrganizationAddressByID(int id);
        Organization GetOrganizationOtherFunderByID(int id);
        IEnumerable<Project> GetAssociatedGrants(int id);
        int CreateNewOrganization(string projorganization);
        bool InsertNewOrgAddress(int id);
        bool DeleteOrgAdress(int id);
        bool UpdateOrgAdress(IEnumerable<Address> OrgAddresses);
        bool DeleteOrgContact(int id);
        bool InsertNewOrgContact(int id);
        bool UpdateOrganizationContact(IEnumerable<Contact> Contacts);
        bool DeleteOrgOtherFunders(int id);
        bool UpdateOtherFunders(IEnumerable<OtherFunder> enumotherfunders);
        bool InsertNewOrgOtherFunders(int id);
               
    }
}
