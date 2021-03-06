using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.DataAccess;
using StructureMap;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.Impl;

namespace BOTACORE.CORE.Services.Impl
{
  public  class OrganizationService
    {
        private IOrganizationRepository _orgrepository; 
       // private IWebContext _webContext;
        private IUserSession _userSession;
        private IRedirector _redirector; 

      public OrganizationService()
      {
        //  _webContext = ServiceFactory.WebContext();
          _userSession = ServiceFactory.UserSession();
          _redirector = ServiceFactory.Redirector();
          _orgrepository = RepositoryFactory.OrganizationRepository();

      }


      #region OtherFunders Insert Update Delete
     
      public bool UpdateOtherFunders(IEnumerable<OtherFunder> enumotherfunders)
      {
          return _orgrepository.UpdateOtherFunders(enumotherfunders); 
      }
        
      public bool DeleteOrgOtherFunders(int id)
      {
          return _orgrepository.DeleteOrgOtherFunders(id); 
      }
      
      public bool InsertNewOrgOtherFunders(int id)
      {
          return _orgrepository.InsertNewOrgOtherFunders(id);
      }
     
      #endregion 



      #region  Address.
      public bool UpdateOrgAdress(IEnumerable<Address> OrgAddresses)
      {
          return _orgrepository.UpdateOrgAdress(OrgAddresses); 
      }
        
      public bool DeleteOrgContact(int id)
      {
          return _orgrepository.DeleteOrgContact(id); 
      }
      
      public bool InsertNewOrgContact(int id)
      {
          return _orgrepository.InsertNewOrgContact(id);
      }
      #endregion


      public bool DeleteOrgAdress(int id)
      {
          return _orgrepository.DeleteOrgAdress(id); 
      }


      public bool InsertNewOrgAddress(int id)
      {
          return _orgrepository.InsertNewOrgAddress(id); 
      }

      public int CreateNewOrganization(string projorganization)
      {
          return _orgrepository.CreateNewOrganization(projorganization); 
      }

      public IEnumerable<Project> GetAssociatedGrants(int id)
      {
          return _orgrepository.GetAssociatedGrants(id); 
      
      }

      public Organization GetOrganizationOtherFunderByID(int id)
      {
          return _orgrepository.GetOrganizationOtherFunderByID(id); 
      }


      public IEnumerable<Address> GetOrganizationAddressByID(int id)
      {
          return _orgrepository.GetOrganizationAddressByID(id); 
      }



      public Organization GetOrganizationContactByID(int id)
      {
          return _orgrepository.GetOrganizationContactByID(id); 
      }

      public Organization GetOrganizationGeneralByID(int id)
      {
          return _orgrepository.GetOrganizationGeneralByID(id); 
      
      }

      public IEnumerable<Organization> SearchOrganizationByName(string Name)
      {
          return _orgrepository.SearchOrganizationByName(Name); 
      }

      public IEnumerable<LegalStatusList> GetLegalStatusList()
      {
          return _orgrepository.GetLegalStatusList();
      }

      /// <summary>
      /// deletes the organization attached to given proposal.
      /// </summary>
      /// <param name="ProposalID"></param>
      /// <returns></returns>
      public bool DeleteOrganization(int ProposalID)
      {
          return _orgrepository.DeleteOrganization(ProposalID);
      }



      /// <summary>
      /// Updates the info of given Org of given proposal.
      /// </summary>
      /// <param name="org"></param>
      /// <param name="ProposalID"></param>
      /// <returns></returns>
      public bool UpdateOrganization(Organization org, int ProposalID)
      {
         return _orgrepository.UpdateOrganization(org,ProposalID); 
      }
      
      /// <summary>
      /// Gets the Organization of given Proposal
      /// </summary>
      /// <param name="ProposalID"></param>
      /// <returns></returns>
      public Organization GetOrganizationOfCurrentProposal(int ProposalID)
      {
          //Logic: 
          //if User Logged in then return him else redirect him to Login Page.
          //Other Logic is coming soon.

          return _orgrepository.GetOrganizationOfCurrentProposal(ProposalID); 
      }



      public bool UpdateOrganizationGeneral(General gen)
      {
          return _orgrepository.UpdateOrganizationGeneral(gen); 
      }


      public General GetOrganizationGeneralOfCurrentProposal(int ProposalID)
      {
          return _orgrepository.GetOrganizationGeneralOfCurrentProposal(ProposalID); 
      
      }



      public bool UpdateOrganizationContact(IEnumerable<Contact> Contacts)
      {
             return _orgrepository.UpdateOrganizationContact(Contacts); 
      }


      public Organization GetOrganizationContactOfCurrentProposal(int ProposalID)
      {
              return _orgrepository.GetOrganizationContactOfCurrentProposal(ProposalID); 
      }

      public Organization getOrganizationByID(int id)
      {
              return _orgrepository.getOrganizationByID(id); 
      }

      

    }
}
