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
   public class AppDropDownsService
    {
        private IUserSession _userSession;
        private IRedirector _redirector;
        private AppDropDownsRepository _repdropdown;
        
      public AppDropDownsService()
      {
        //  _webContext = ServiceFactory.WebContext();
          _userSession = ServiceFactory.UserSession();
          _redirector = ServiceFactory.Redirector();
          _repdropdown = new AppDropDownsRepository();

      }


      #region SSPRoles
      public bool UpdateSSPRoles(IEnumerable<RolesSSPStaff> SSPRolesList)
      {
          return _repdropdown.UpdateSSPRoles(SSPRolesList);
      }

      public bool DeleteSSPRoles(int SSPRolesID)
      {
          return _repdropdown.DeleteSSPRoles(SSPRolesID);
      }

      public bool InsertSSPRoles()
      {
          return _repdropdown.InsertSSPRoles();
      }

      #endregion

      #region
      public IEnumerable<LegalStatusList> GetLegalStatusList()
      {
           return _repdropdown.GetLegalStatusList();      
      }

       public bool  UpdateLegalStatus(IEnumerable<LegalStatusList> LegStatusList)
       {
           return _repdropdown.UpdateLegalStatus(LegStatusList); 
       }
       
       public bool DeleteLegalStatus(int LegalStatID)
       {
           return _repdropdown.DeleteLegalStatus(LegalStatID); 
       }

       public bool InsertLegalStatus()
       {
           return _repdropdown.InsertLegalStatus();
       }
      #endregion 

       #region EventType
       public IEnumerable<EventType> GetEventTypeList()
       {
           return _repdropdown.GetEventTypeList();
       }
      
       public bool DeleteEventTypeList(int EventTypeListID)
       {
           return _repdropdown.DeleteEventTypeList(EventTypeListID);
       }


       public bool InsertEventTypeList()
       {
           return _repdropdown.InsertEventTypeList();
       }

    
       public bool EventTypeListEdit(List<EventType> EventTypeList)
       {
           return _repdropdown.EventTypeListEdit(EventTypeList);
       }
       #endregion


       #region LfIndicatorList

       public IEnumerable<LFIndicatorList> GetLfIndicatorList()
       {

           return _repdropdown.LfIndicatorList();
       }
       #endregion 


       #region Budget Category DropDown

       public IEnumerable<FinArtCatListR> GetCatList()
       {

           return _repdropdown.GetCatList();
       }


       public void InsertNewCatName(string NewCatName)
       {

           _repdropdown.InsertNewCatName(NewCatName);
       }


       public bool ArticleCatListEdit(List<FinArtCatListR> FinArtCatListR)
       {
           return _repdropdown.ArticleCatListEdit(FinArtCatListR); 
       }


       public bool DeleteArticleCat(int CatListID)
       {
          return  _repdropdown.DeleteArticleCat(CatListID); 
       }


       #endregion 



       #region IndicatorContentLabelCategory
       public IEnumerable<IndicatorLabelContentCategory> IndicatorLabelContentCategoryList()
       {
           return _repdropdown.IndicatorLabelContentCategoryList();
       }

       public bool DeleteIndicatorLabelContentCategoryList(int indicatorLabelContentCategoryListID)
       {
           return _repdropdown.DeleteIndicatorLabelContentCategoryList(indicatorLabelContentCategoryListID);
       }

       public IndicatorLabelContentCategory GetIndicatorLabelContentCategory(int indicatorLabelContentCategoryID)
       {
           return _repdropdown.GetIndicatorLabelContentCategory(indicatorLabelContentCategoryID);
       }

       public bool InsertIndicatorLabelContentCategoryList()
       {
           return _repdropdown.InsertIndicatorLabelContentCategoryList();
       }


       public bool IndicatorLabelContentCategoryListEdit(List<IndicatorLabelContentCategory> indicatorLabelContentCategoryList)
       {
           return _repdropdown.IndicatorLabelContentCategoryListEdit(indicatorLabelContentCategoryList);
       }
       #endregion

       #region RegionList
       public IEnumerable<RegionList> GetRegionList()
       {
           return _repdropdown.GetRegionList();
       }

       public bool DeleteRegionList(int RegionListID)
       {
           return _repdropdown.DeleteRegionList(RegionListID);
       }


       public bool InsertRegionList()
       {
           return _repdropdown.InsertRegionList();
       }


       public bool RegionListEdit(List<RegionList> RegionList)
       {
           return _repdropdown.RegionListEdit(RegionList);
       }
       #endregion





       #region GrantType
       public IEnumerable<GrantTypeList> GetGrantTypeList()
       {
           return _repdropdown.GetGrantTypeList();
       }
      
       public bool DeleteGrantTypeList(int GrantTypeListID)
       {
           return _repdropdown.DeleteGrantTypeList(GrantTypeListID);
       }


       public bool InsertGrantTypeList()
       {
           return _repdropdown.InsertGrantTypeList();
       }

    
       public bool GrantTypeListEdit(List<GrantTypeList> GrantTypeList)
       {
           return _repdropdown.GrantTypeListEdit(GrantTypeList);
       }
       #endregion



       #region Project Location
       public IEnumerable<ProjLocationList> GetProjectLocationList()
       {
           return _repdropdown.GetProjectLocationList();
       }

       public bool DeleteProjectLocationList(int ProjectLocationListID)
       {
           return _repdropdown.DeleteProjectLocationList(ProjectLocationListID);
       }


       public bool InsertProjectLocationList()
       {
           return _repdropdown.InsertProjectLocationList();
       }


       public bool ProjectLocationListEdit(List<ProjLocationList> ProjectLocationList)
       {
           return _repdropdown.ProjectLocationListEdit(ProjectLocationList);
       }
       #endregion




       #region Program Area List
       public IEnumerable<ProgramAreaList> GetProgramAreaList()
       {
           return _repdropdown.GetProgramAreaList();
       }
      
       public bool DeleteProgramAreaList(int ProgramAreaListID)
       {
           return _repdropdown.DeleteProgramAreaList(ProgramAreaListID);
       }

     
       public bool InsertProgramAreaList()
       {
           return _repdropdown.InsertProgramAreaList();
       }

      
       public bool  ProgramAreaListEdit(List<ProgramAreaList> ProgramAreaList)
       {
           return _repdropdown.ProgramAreaListEdit(ProgramAreaList);
       }
       #endregion


       #region Competition Code
       public IEnumerable<CompetitionCodeList> GetCompetitionCodeList()
       {
           return _repdropdown.GetCompetitionCodeList();
       }
      
       public bool DeleteCompetitionCodeList(int ID)
       {
            return _repdropdown.DeleteCompetitionCodeList(ID);
       }

     
       public bool InsertCompetitionCodeList()
       {
           //Insert
           return _repdropdown.InsertCompetitionCodeList();
       }

 
       public bool CompetitionCodeListEdit(List<CompetitionCodeList> List)
       {
           return _repdropdown.CompetitionCodeListEdit(List);
       }
       #endregion


       #region ProposalStatusList

       public IEnumerable<ProposalStatusList> GetProposalStatusList()
       {
           return _repdropdown.GetProposalStatusList();
       }
     
       public bool DeleteProposalStatusList(int ID)
       {
           //Delete
          return _repdropdown.DeleteProposalStatusList(ID);
       }

    
       public bool InsertProposalStatusList()
       {
           //Insert
           return _repdropdown.InsertProposalStatusList();
       }

    
       public bool ProposalStatusListEdit(List<ProposalStatusList> List)
       {
           return _repdropdown.ProposalStatusListEdit(List);
       }

       public ProposalStatusList ProposalStatusListGetItem(string ProposalStatusText)
       {
           return _repdropdown.ProposalStatusListGetItem(ProposalStatusText);
       }
       #endregion 



    }
}
