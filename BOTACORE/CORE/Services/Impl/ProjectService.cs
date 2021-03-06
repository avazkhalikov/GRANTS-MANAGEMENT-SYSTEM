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
  public class ProjectService
    {

      ProjectRepository projrepository;
      OrganizationRepository orgrep;
      UserSession session;
      public ProjectService()
      {
          session = new UserSession();
          projrepository = new ProjectRepository();
          orgrep = new OrganizationRepository();
      }



      #region 
       public IEnumerable<Project> getAllProjects()
       {
           return projrepository.getAllProjects();
       }

      public bool InsertProjectKey(string KeyWord, int ProjectID)
       {
          return projrepository.InsertProjectKey(KeyWord, ProjectID);
       }

       public IEnumerable<TheKey> GetProjectKeysByID(int ProjID)
       {
           return projrepository.GetProjectKeysByID(ProjID); 
       }

       public bool DeleteProjectKeyByID(int KeyID, int ProjID)
       {
           return projrepository.DeleteProjectKeyByID(KeyID, ProjID);
       }             
  
      #endregion 


      #region  OutCome Statements

      public OutComeStatement GetOutComeStatements(int ProjID)
       {
           return projrepository.GetOutComeStatements(ProjID); 
       }

      

      public bool UpdateOutComeStatements(OutComeStatement outcome)
      {

          return projrepository.UpdateOutComeStatements(outcome);
      }

     #endregion 


      #region FundingSource

      public bool UpdateProjFundingSource(List<FundingSource> fundingSourceList) 
      {
          return projrepository.UpdateProjFundingSource(fundingSourceList);  
      }
 

      public IEnumerable<FundingOrganization> GetFundingOrganizationList()
      {
          return projrepository.GetFundingOrganizationList(); 
      
      }


      public bool CreateProjFundingSource(FundingSource _fundingSource)
      {
         return projrepository.CreateProjFundingSource(_fundingSource); 
      }


      public bool DeleteProjFundingSource(int FundingSourceID)
      {
          return projrepository.DeleteProjFundingSource(FundingSourceID);
      }

      #endregion 


      public IEnumerable<FundingSource> GetProjAllFundingSource(int ProjID)
      {
          return projrepository.GetProjAllFundingSource(ProjID); 
      
      }

      public bool ProjectInfoUpdate(ProjectInfo projInfo) 
      {
          return projrepository.ProjectInfoUpdate(projInfo);  
      }

      
      public bool UpdateReportPeriodList(List<ReportPeriodList> repperlist) 
      {
         return projrepository.UpdateReportPeriodList(repperlist);  
      }


      public bool UpdateBudgetInitialAmount(int initialamt, int budgetID)
      {
         return projrepository.UpdateBudgetInitialAmount(initialamt, budgetID); 
      }

      public bool ProposalStatusUpdate(ProposalStatus item)
      {
          return projrepository.ProposalStatusUpdate(item);
      }

      public bool ProposalInfoUpdate(Project project)
      {
        //  WORK SEE TODO SOMETHING DOES NOT WORK HERE make so onlyAdmin ChangesStatus.
         ProposalStatus propstatus = projrepository.GetProjectByID(project.ProjectID).ProposalStatus;
         
         if(propstatus.ProposalStatusList.ProposalStatusID == 4) //if in db it is closed grant
         {
              if(propstatus.ProposalStatusList.ProposalStatusID != project.ProposalStatus.PropStatusID) //change has been made.
              {
                 //check user role. if addming allow, if not return false.
                  if (session.CurrentUser.RoleID == 2)
                  {
                      ;  //don't do anything.
                  }
                  else
                  {
                      return false;  //other's can't change from closed to anything else.
                  }
              
              }
         }
         
          bool UpdateLabel = false; 
          //if admin and ProjectLabel has changed.
          if (session.CurrentUser.RoleID == 2) // && session.ProjectLabel != project.Label)
          {
              UpdateLabel = true;
              project.OrgID = session.OrgID;
          }
          
          return projrepository.ProposalInfoUpdate(project, UpdateLabel);
          
      }



      #region 
      public IEnumerable<ProposalInfo> SearchProposalKeyword(string str)
      {

          String[] KeyArray = str.Split(' ');
          
          return projrepository.SearchProposalKeyword(KeyArray); 
      }


      public IEnumerable<ProposalInfo> SearchProposalByID(int num)
      {
          return projrepository.SearchProposalByID(num);
      
      }

      public IEnumerable<ProposalInfo> SearchProposalByName(string SearchString)
      {
          return projrepository.SearchProposalByName(SearchString);

      }

      #endregion 
      /// <summary>
      /// 
      /// Used in Proposal Creation.
      /// </summary>
      /// <param name="ProjectID"></param>
      /// <returns></returns>
      private bool UpdateToFirstDefaultProposalStatus(int ProjectID)
      {
          IEnumerable<ProposalStatusList> propstatusList = projrepository.GetProposalStatusList();
          if (propstatusList.Any())
          {
              ProposalStatusList FirstPropStatus = propstatusList.First();
              return projrepository.InsertNewProposalStatusIntoProject(FirstPropStatus.ProposalStatusID, ProjectID); 
          }
          else
          {
              return false;
          
          }
      }


      private bool UpdateToFirstDefaultProgramArea(int ProjectID)
      {
          IEnumerable<ProgramAreaList> propstatusList = projrepository.GetProgramAreaLists();
          if (propstatusList.Any())
          {
              ProgramAreaList ProgramAreaStatus = propstatusList.First();
              return projrepository.InsertNewProgramAreaIntoProject(ProgramAreaStatus.ProgramAreaCodeID, ProjectID); 
          }
          else
          {
              return false;

          }
      }



      private bool UpdateToFirstDefaultGrantType(int ProjectID)
      {
          IEnumerable<GrantTypeList> propstatusList = projrepository.GetGrantTypeLists();
          if (propstatusList.Any())  //if there is any List Item.
          {
              GrantTypeList GrantTypeStatus = propstatusList.First();
              return projrepository.InsertNewGrantTypeIntoProject(GrantTypeStatus.GrantTypeCodeID, ProjectID); 
          }
          else
          {
              return false;

          }
      }



      private bool UpdateToFirstDefaultCompetitionCode(int ProjectID)
      {
          IEnumerable<CompetitionCodeList> CompetitionCode = projrepository.CompetitionCodeLists();
          if (CompetitionCode.Any())
          {
              CompetitionCodeList FirstCompetitionCode = CompetitionCode.First();
              return projrepository.InsertNewCompetitionCodeIntoProject(FirstCompetitionCode.CompetitionCodeID, ProjectID); 
             
          }
          else
          {
              return false;

          }
      }

      private bool InsertDefaultLegalAddressIntoNewProject(int OrgID)
      {
         // int OrgID =  orgrep.GetOrganizationOfCurrentProposal(ProjectID).OrgID; 
          return orgrep.InsertNewOrgAddress(OrgID); 
      }
      

      /// <summary>
      /// This delete basicaly removes the title/UPDATE with BLANK TITLE/ of 
      /// the project after which it won't be shown in search list.
      /// Also, Set. Project->isDeleted = true; 
      /// </summary>
      /// <param name="?"></param>
      public void ProposalDelete(int id)
      { 
         //get the project and update the title.  //only admin can delete.
          if (session.CurrentUser.RoleID == 2)
          {
              bool status = projrepository.ProposalInfoDelete(id);
          }

      }

      public int CreateNewProposal(int OrgID, string projtitle, string projectlabel)
      {
               
          //check if such label exists first.
          if (!projrepository.CheckifSuchProjectLabelExists(projectlabel))
          {
              int ProjectID =  projrepository.CreateNewProposal(OrgID, projtitle, projectlabel);

              if (ProjectID > 0 )
              {
                  //Update to First Selected Values of DropDowns.
                  //1. ProposalStatus
                  bool propstatus = UpdateToFirstDefaultProposalStatus(ProjectID);
                  //2. CompetitionCode
                  bool compcode = UpdateToFirstDefaultCompetitionCode(ProjectID);
                  //3. GrantType
                  bool granttype = UpdateToFirstDefaultGrantType(ProjectID);
                  //4. ProgramArea
                  bool programarea = UpdateToFirstDefaultProgramArea(ProjectID);
                  //Insert default Legal Address...Reports require this.
                  bool LegalAddress = InsertDefaultLegalAddressIntoNewProject(OrgID);
              }

              bool updated = true; // ProposalInfoUpdate(NewProject);
          
              if (updated)
                 {
                     return ProjectID;
                 }
             else
                 {
                     return -2;
                 }
          }
          else
          {
              return -2; //such label already exists. 
          }
      }
      
     

      public string LastEnteredProject() 
      {
          return projrepository.LastEnteredProject() ; 
      }
      public IEnumerable<General> GetAllOrganizationList() 
      {
          return projrepository.GetAllOrganizationList(); 
      }

      public bool DeleteArticleCat(int? FinArticleCatID, int? BudgetID)
      {
          return projrepository.DeleteArticleCat(FinArticleCatID.Value, BudgetID.Value);
      }

      public bool DeleteReportPeriod(int RepperID, int BudgetID)
      {
         return projrepository.DeleteReportPeriod(RepperID, BudgetID); 
      }

      public void UpdateReportPeriodTrans(List<ReportPeriod> ReportPeriods)
      {
          projrepository.UpdateReportPeriodTrans(ReportPeriods); 
      }
            
      public void InsertNewCatName(string NewCatName)
      {

          projrepository.InsertNewCatName(NewCatName);
      }


      public void AddFinArticleCategory(int id, int FinCatSel)
      {

          projrepository.AddFinArticleCategory(id, FinCatSel);
      }


      public IEnumerable<FinArtCatList> GetCatList()
      {
          return projrepository.GetCatList(); 
      }

      public bool DeleteArticle(int FinArticleID)
      {
         return projrepository.DeleteArticle(FinArticleID); 
      }

      public void InsertNewArticle(int FinArticleCatID, int BudgetID)
      {
          projrepository.InsertNewArticle(FinArticleCatID, BudgetID); 
      
      }

      public Budget GetBudgetTransactionByID(int id)
      {
          return projrepository.GetBudgetTransactionByID(id);
      }

      public Budget GetBudgetBYID(int id)
      {
          return projrepository.GetBudgetByID(id);
      }


      public IEnumerable<ReportPeriodList> GetFinPeriods(int BudgetID)
      {
          return projrepository.GetFinPeriods(BudgetID);
      } 

      public bool CreateReportPeriod(ReportPeriodList item)
      {
          return projrepository.CreateReportPeriod(item); 
      }

      public bool UpdateProjectBudget(Budget bud)
      {
           return projrepository.UpdateProjectBudget(bud); 
      }

      public bool UpdateProject(Project proj)
      {
          return projrepository.UpdateProject(proj);
      }

      public IEnumerable<Role> GetOrgStaffRoles()
      {
          return projrepository.GetOrgStaffRoles();
      }

      

      public Project GetProjectByID(int id)
      {
          return projrepository.GetProjectByID(id);
      }

      public GrantType GetGrantType(int id)
      {
          return projrepository.GetGrantType(id);
      }

      public Project GetProposalInfo(int id)
      {
          return projrepository.GetProposalInfo(id); 
      }

      //getAllProject IDs where isDeleted not "true"
      public IEnumerable<Project> GetAllProjectIDs()
      {
          return projrepository.GetAllProjectIDs(); 
      }
      public IEnumerable<Project> GetAllProjectIDs(bool grants)
      {
          return projrepository.GetAllProjectIDs(grants);
      }

      public ProposalInfo GetProposalInfoOnly(int id)
      {
          return projrepository.GetProposalInfoOnly(id);
      }

      public ProjectInfo GetProjInfoOnly(int id)
      {
          return projrepository.GetProjInfoOnly(id);
      }
     
     
      public IEnumerable<GrantTypeList> GrantTypeList()
      {
          return projrepository.GetGrantTypeLists();
      }

      public IEnumerable<LFIndicatorList> LFIndicatorList()
      {
          return projrepository.LFIndicatorLists();
      }

      public IEnumerable<ProjLocationList> ProjLocationList()
      {
          return projrepository.GetProjLocLists();
      }

      public IEnumerable<ProposalStatusList> ProposalStatusList()
      {
          return projrepository.GetProposalStatusList();
      }



      public IEnumerable<CompetitionCodeList> CompetitionCodeList()
      {
          return projrepository.CompetitionCodeLists();
      }


      public IEnumerable<ProgramAreaList> GetProgramAreaList()
      {
          return projrepository.GetProgramAreaLists(); 
      }


    }
}
