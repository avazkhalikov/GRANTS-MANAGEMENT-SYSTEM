using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;
using System.Data;
using StructureMap;
using BOTACORE.CORE;
using BOTACORE.CORE.DataAccess.DAL;
using log4net;
using log4net.Config;


namespace BOTACORE.CORE.DataAccess.Impl
{

    public static class ConvertBOTA
    {
        public static EntitySet<T> ToEntitySet<T>(this IEnumerable<T> source) where T : class
        {
            var es = new EntitySet<T>();
            es.AddRange(source);
            return es;
        }
    }


  
   public class ProjectRepository : BOTACORE.CORE.DataAccess.Impl.IProjectRepository 
    {
       private string connectString;
       private ISqlCommands sqlcmd;
       protected static readonly ILog log = LogManager.GetLogger(typeof(ProjectRepository));

       public ProjectRepository()
       {
            //string connString = Settings.Default.EntityConnection; 
          //  Context = new BOTADBEntities1();
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();
           sqlcmd = SqlFactory.MSSQL();

       }

       #region Project Keys

       public IEnumerable<Project> getAllProjects()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           return db.Projects; 
       }

       public bool InsertProjectKey(string KeyWord, int ProjectID)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           bool result = true;
           
           TheKey NewKey = new TheKey();
           NewKey.KeyName = KeyWord;
           NewKey.ProjectID = ProjectID; 

           try
           {
                //og..LogManager.GetLogger(Program);
//               catch (Exception e)
               db.TheKeys.InsertOnSubmit(NewKey);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
           //    log4net.Config.XmlConfigurator.Configure();
          //     log.Error("sadi the great", ex);

               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized(); 
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;  //Log4Net.
           }
           return result;
       }

       public IEnumerable<TheKey> GetProjectKeysByID(int ProjID)
       {        
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.TheKeys
                        where ls.ProjectID == ProjID
                        select ls;
           return result;

       }

       public bool DeleteProjectKeyByID(int KeyID, int ProjID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from f in db.TheKeys
                               where f.ProjectID == ProjID && f.KeyID == KeyID
                               select f).FirstOrDefault();

               if (toDelete != null)
               {
                   db.TheKeys.DeleteOnSubmit(toDelete);
                   db.SubmitChanges();
               }
               else
               {
                   return false;
               }
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Delete.", ex);
                result = false;
           }

           return result;
       }

       #endregion 
       public bool DeleteProjectPermanent(int ProjID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from p in db.Projects
                               where p.ProjectID == ProjID
                               select p).FirstOrDefault();

               if (toDelete != null)
               {
                   db.Projects.DeleteOnSubmit(toDelete);
                   db.SubmitChanges();
               }
               else
               {
                   return false;
               }
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "#Delete. Project failure#", ex);
                result = false;
           }

           return result;
       }

       

       #region OutCome Statements.
       public OutComeStatement GetOutComeStatements(int ProjID)
       {
       
           BOTADataContext db = new BOTADataContext(connectString);
           OutComeStatement result=null; 
          
           try
           {
             result = (from OutC in db.OutComeStatements
                             where OutC.ProjectID == ProjID
                             select OutC).First();

             if (result != null)
                 return (OutComeStatement)result;
             else
                 return null; 
           }
           catch (Exception ex)
           {

               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return null;
           }
    
       }



       public bool UpdateOutComeStatements(OutComeStatement outcome)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {

               if (!db.OutComeStatements.Any(l => l.ProjectID == outcome.ProjectID))  // !if exists. 
               {
                   OutComeStatement toInsertOut = new OutComeStatement();
                   toInsertOut.OutcomeE = outcome.OutcomeE;
                   toInsertOut.OutcomeK = outcome.OutcomeK; 
                   toInsertOut.OutcomeR = outcome.OutcomeR;
                   toInsertOut.ProjectID = outcome.ProjectID;
                   db.OutComeStatements.InsertOnSubmit(toInsertOut);

               }
               else
               {
                   db.OutComeStatements.Attach(outcome);
                   db.Refresh(RefreshMode.KeepCurrentValues, outcome);
               }

               db.SubmitChanges();


           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;

       }

       #endregion 


       #region FundingSource

       public bool UpdateProjFundingSource(List<FundingSource> fundingSourceList)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {

               db.FundingSources.AttachAll(fundingSourceList);
               db.Refresh(RefreshMode.KeepCurrentValues, fundingSourceList);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;
       }


       public IEnumerable<FundingOrganization> GetFundingOrganizationList()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.FundingOrganizations
                        select ls;
           return result;

       }


       public bool CreateProjFundingSource(FundingSource _fundingSource)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           bool result = true;
           try
           {
               db.FundingSources.InsertOnSubmit(_fundingSource);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }
           return result;
       }


       public bool DeleteProjFundingSource(int FundingSourceID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from f in db.FundingSources
                               where f.FundingSourceID == FundingSourceID
                               select f).FirstOrDefault();
             
               if (toDelete != null)
               {
                   db.FundingSources.DeleteOnSubmit(toDelete);
                   db.SubmitChanges();
               }
               else
               {
                   return false; 
               }
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;
       }

       #endregion 




       /// <summary>
       /// Get All Funding Sources of given ProjID.
       /// </summary>
       /// <param name="ProjID"></param>
       /// <returns></returns>
       public IEnumerable<FundingSource> GetProjAllFundingSource(int ProjID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           IEnumerable<FundingSource> result;
           result = from f in db.FundingSources
                    where f.ProjectID == ProjID
                    select f;

           return result;

       }



       public bool ProjectInfoUpdate(ProjectInfo projInfo)
       {

           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {

               if (!db.ProjectInfos.Any(l => l.ProjectInfoID == projInfo.ProjectInfoID))  // !if exists. 
               {
                   ProjectInfo info = new ProjectInfo();
                   info.ProjectInfoID = projInfo.ProjectInfoID;
                   info.AcceptedDate = projInfo.AcceptedDate;
                   info.AmtRequested = projInfo.AmtRequested;
                   info.AwardedAmt = projInfo.AwardedAmt;
                   info.ClosedDate = projInfo.ClosedDate;
                   info.EndDate = projInfo.EndDate;
                   info.StartDate = projInfo.StartDate; 

                  // db.ProjectInfos.Attach(projInfo);
                   db.ProjectInfos.InsertOnSubmit(info);

               }
               else
               {
                   db.ProjectInfos.Attach(projInfo);
                   db.Refresh(RefreshMode.KeepCurrentValues, projInfo);
               }

               db.SubmitChanges();

               
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;
       
       
       }




       /// <summary>
       /// Updates Report Periods.
       /// </summary>
       /// <param name="repperlist"></param>
       /// <returns></returns>
      public bool UpdateReportPeriodList(List<ReportPeriodList> repperlist) 
      {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
              
               db.ReportPeriodLists.AttachAll(repperlist);
               db.Refresh(RefreshMode.KeepCurrentValues, repperlist);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
              result = false;
           } 
         
          return result;
      }


       public bool UpdateBudgetInitialAmount(int initialamt, int budgetID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               Budget budget = (from b in db.Budgets
                             where b.BudgetID == budgetID
                             select b).FirstOrDefault();
               
               budget.ContractInitialAmt = initialamt; 

               //db.Budgets.Attach();
               db.Refresh(RefreshMode.KeepCurrentValues, budget);
               db.SubmitChanges();
               return true; 
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return false; 
           }

           //return result;
       }


       public bool RestoreDeletedProject(int id)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               Project pr = (from p in db.Projects
                                where p.ProjectID == id
                                select p).FirstOrDefault();

               if(pr != null)
               pr.isDeleted = null;

               //db.Budgets.Attach();
               db.Refresh(RefreshMode.KeepCurrentValues, pr);
               db.SubmitChanges();
               return true;
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Project is deleted restored", ex);
               return false;
           }

           //return result;
       }

       public bool ProposalStatusUpdate(ProposalStatus item)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               ProposalStatus _prop = new ProposalStatus();
               _prop.ProjectID = item.ProjectID;
               _prop.SelectedDate = DateTime.Now;
               _prop.PropStatusID = item.PropStatusID;

               db.ProposalStatus.Attach(_prop);
               db.Refresh(RefreshMode.KeepCurrentValues, _prop);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }
           return result;
       }

       public bool ProposalInfoDelete(int id)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               ProposalInfo propinfo = (from b in db.ProposalInfos
                                where b.ProjectID == id
                                select b).FirstOrDefault();

               propinfo.Title = null;
               propinfo.TitleE = null;
               propinfo.TitleR = null;
               propinfo.Project.isDeleted = true; 

               //db.Budgets.Attach();
               db.Refresh(RefreshMode.KeepCurrentValues, propinfo);
               db.SubmitChanges();
               return true;
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return false;
           }
       }

       public bool UpdateProjectLabel(Project project)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               /*var oldItem  = (from p in db.ProjectEvents
                              where p.EventID == item.EventID
                              select p).First();*/
            //   db.Projects.Attach(project);
               db.Refresh(RefreshMode.KeepCurrentValues, project);
               db.SubmitChanges();
               result = true; 
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
                result = false;
           }

           return result;
       
       }

       /// <summary>
       /// Updates: ProposalStatus, Competitioncode, GrantType, ProgramArea
       /// Grantee, Title, Pub Statement
       /// </summary>
       /// <param name="project"></param>
       /// <returns></returns>
       public bool ProposalInfoUpdate(Project project, bool UpdateLabel)
       {

           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           
               try
               {

                   // Title/PubStatement.
                   //Project _project = (from p in db.Projects
                   //                    where p.ProjectID == project.ProjectID
                   //                    select p).FirstOrDefault();
                   db.ProposalInfos.Attach(project.ProposalInfo);
                   db.Refresh(RefreshMode.KeepCurrentValues, project.ProposalInfo);

                   //LFIndicator. DropDown Update. 1
                   if (!db.LFIndicators.Any(l => l.ProjectID == project.LFIndicator.ProjectID))  // !if exists. 
                   {
                       InsertNewLFIndicatorIntoProject(project.LFIndicator.LFIndicatorID.Value, project.ProjectID);

                   }
                   else
                   {
                       project.LFIndicator.SelectedDate = DateTime.Now;
                       db.Refresh(RefreshMode.KeepCurrentValues, project.LFIndicator);
                   }


                   //Granttype. DropDown Update. 1
                   if (!db.GrantTypes.Any(l => l.ProjectID == project.GrantType.ProjectID))  // !if exists. 
                   {
                       InsertNewGrantTypeIntoProject(project.GrantType.GrantTypeCodeID.Value, project.ProjectID);

                   }
                   else
                   {
                       project.GrantType.SelectedDate = DateTime.Now;
                       db.Refresh(RefreshMode.KeepCurrentValues, project.GrantType);
                   }


                   //Program Area. DropDown Update. 2
                   if (!db.ProgramAreas.Any(l => l.ProjectID == project.ProgramArea.ProjectID))  // !if exists. 
                   {
                       InsertNewProgramAreaIntoProject(project.ProgramArea.ProgramAreaCodeID.Value, project.ProjectID);
                   }
                   else
                   {
                       project.ProgramArea.SelectedDate = DateTime.Now;
                       db.Refresh(RefreshMode.KeepCurrentValues, project.ProgramArea);
                   }

                   //Competition Code. 3
                   if (!db.CompetitionCodes.Any(l => l.ProjectID == project.CompetitionCode.ProjectID))  // !if exists. 
                   {
                       InsertNewCompetitionCodeIntoProject(project.CompetitionCode.CompetCodeID.Value, project.ProjectID);
                      
                   }
                   else
                   {
                       project.CompetitionCode.SelectedDate = DateTime.Now;
                       db.Refresh(RefreshMode.KeepCurrentValues, project.CompetitionCode);
                   }


                   //Proposal Status. 4
                   if (!db.ProposalStatus.Any(l => l.ProjectID == project.ProposalStatus.ProjectID))  // !if exists. 
                   {
                       InsertNewProposalStatusIntoProject(project.ProposalStatus.PropStatusID, project.ProjectID);
                     
                   }
                   else
                   {
                       project.ProposalStatus.SelectedDate = DateTime.Now;
                       db.Refresh(RefreshMode.KeepCurrentValues, project.ProposalStatus);
                   }


                   //Project Location. 
                   if (!db.ProjLocations.Any(l => l.ProjectID!=null))  // !if exists. 
                   {
                       InsertNewProjectLocationIntoProject(project.ProjLocation.ID, project.ProjectID);
                   }
                   else
                   {
                       if(project.ProjLocation.ID > 0)
                       {
                           if(db.ProjLocations.Any(l=>l.ID != project.ProjLocation.ID))
                           {
                              project.ProjLocation.SelectedDate = DateTime.Now;
                              db.Refresh(RefreshMode.KeepCurrentValues, project.ProjLocation);
                           }
                       }
                   }


                   //Refresh Project Itself.
                   if (UpdateLabel)
                   {                     
                       db.Refresh(RefreshMode.KeepCurrentValues, project.Organization);
                       db.Refresh(RefreshMode.KeepCurrentValues, project);
                   }



                   db.SubmitChanges();
               }
               catch (Exception ex)
               {
                   //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
                   Log.EnsureInitialized();
                   Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
                   result = false;
               }
           
           return result;

       }


     
       public bool InsertNewLFIndicatorIntoProject(int LFIndicatorID, int ProjectID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {
               //Insert 
               LFIndicator lind = new LFIndicator();
               lind.LFIndicatorID = LFIndicatorID;
               lind.ProjectID = ProjectID;
               lind.SelectedDate = DateTime.Now;
               db.LFIndicators.InsertOnSubmit(lind);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;

       }

       public bool InsertNewGrantTypeIntoProject(int GrantTypeCodeID, int ProjectID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {
               //Insert 
               GrantType gtype = new GrantType();
               gtype.GrantTypeCodeID = GrantTypeCodeID;
               gtype.ProjectID = ProjectID;
               gtype.SelectedDate = DateTime.Now;
               db.GrantTypes.InsertOnSubmit(gtype);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;

       }


       public bool InsertNewProgramAreaIntoProject(int ProgramAreaCodeID, int ProjectID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {
               //Insert 
               ProgramArea parea = new ProgramArea();
               parea.ProgramAreaCodeID = ProgramAreaCodeID;
               parea.ProjectID = ProjectID;
               parea.SelectedDate = DateTime.Now;
               db.ProgramAreas.InsertOnSubmit(parea);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;

       }


       public bool InsertNewCompetitionCodeIntoProject(int CompetitionCodeID, int ProjectID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {
               //Insert 
               CompetitionCode compcode = new CompetitionCode();
               compcode.CompetCodeID = CompetitionCodeID;
               compcode.ProjectID = ProjectID;
               compcode.SelectedDate = DateTime.Now;
               db.CompetitionCodes.InsertOnSubmit(compcode);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;

       }


       public bool InsertNewProjectLocationIntoProject(int ProjLocID, int ProjectID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               //Insert 
               ProjLocation projloc = new ProjLocation();

               projloc.ID = ProjLocID;
               projloc.ProjectID = ProjectID; 
               projloc.SelectedDate =  DateTime.Now;
               db.ProjLocations.InsertOnSubmit(projloc);
               db.SubmitChanges();
               
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project Location", ex);
               result = false;
           }

           return result;
       }


       public bool InsertNewProposalStatusIntoProject(int ProposalStatusID, int ProjectID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {
               //Insert 
               ProposalStatus propstatus = new ProposalStatus();
               propstatus.PropStatusID = ProposalStatusID;
               propstatus.ProjectID = ProjectID;
               propstatus.SelectedDate = DateTime.Now;
               db.ProposalStatus.InsertOnSubmit(propstatus);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;

       }


       /// <summary>
       /// Simple Search of Multiple Keywords, that accepts Array and returns List.
       /// </summary>
       /// <param name="KeyArray"></param>
       /// <returns></returns>
       public IEnumerable<ProposalInfo> SearchProposalKeyword(String[] KeyArray) 
       {
           BOTADataContext db = new BOTADataContext(connectString);
           StringBuilder sb = new StringBuilder();
           try
           {
               for (int i = 0; i < KeyArray.Length; i++)
               {
                   sb.Append(" (TheKey.KeyName LIKE N'" + KeyArray[i] + "') ");

                   if (i != KeyArray.Length - 1)
                   {
                       sb.Append(" OR ");
                   }

               }

               string Str = @" SELECT DISTINCT ProposalInfo.*
                          FROM ProposalInfo INNER JOIN
                          TheKey ON ProposalInfo.ProjectID = TheKey.ProjectID
                          WHERE " + sb.ToString();
             
               IEnumerable<ProposalInfo> PropInfos = db.ExecuteQuery<ProposalInfo>(Str);
               return PropInfos;
           }
           catch(Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return null; 
           }

       }


       public IEnumerable<ProposalInfo> SearchProposalByLabel(string sb)
       {
           BOTADataContext db = new BOTADataContext(connectString);
                      
            //           SELECT DISTINCT ProposalInfo.*
            //FROM ProposalInfo INNER JOIN
            //Projects ON ProposalInfo.ProjectID = Projects.ProjectID
            //WHERE  (Projects.Label LIKE N'SSP-2011-01')

           string Str = @" SELECT DISTINCT ProposalInfo.*
                          FROM ProposalInfo INNER JOIN
                          TheKey ON ProposalInfo.ProjectID = Projects.ProjectID
                          WHERE (Projects.Label LIKE N'" + sb.ToString() + "')";

           IEnumerable<ProposalInfo> PropInfos = db.ExecuteQuery<ProposalInfo>(Str);
           return PropInfos;
                
       }

       public IEnumerable<ProposalInfo> SearchProposalByID(int num)
       {
       //    BOTADataContext db = new BOTADataContext(connectString);

       //    IEnumerable<ProposalInfo> result;
          BOTADataContext db = new BOTADataContext(connectString);
          string result =    @" SELECT DISTINCT ProposalInfo.*
                          FROM ProposalInfo
                          WHERE (Projects.Label LIKE N'" + num.ToString() + "')";

          IEnumerable<ProposalInfo> PropInfos = db.ExecuteQuery<ProposalInfo>(result);
          return PropInfos;

          

       }



       public IEnumerable<ProposalInfo> SearchProposalByName(string SearchString)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           IEnumerable<ProposalInfo> result;
           result = from propinfo in db.ProposalInfos
                    where propinfo.TitleR.Contains(SearchString)
                    select propinfo;

           return result;
       }

       
       public bool CheckifSuchProjectLabelExists(string projectlabel)
       {
            BOTADataContext db = new BOTADataContext(connectString);
            try
            {

                // Title/PubStatement.
              /*  Project _project = (from p in db.Projects
                                    where p.Label == projectlabel
                                   select p).FirstOrDefault(); */

                if (db.Projects.Any(l =>l.Label == projectlabel))  // !if exists. 
                {
                    return true; 
                }
                else
                {
                    return false; 
                }
            }

            catch (Exception ex)
            {
                //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
                Log.EnsureInitialized();
                Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
                return true; 
            }

       }

       public int CreateNewProposal(int OrgID, string projtitle, string projectlabel)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           Project proj = new Project();
           proj.OrgID = OrgID;
           ProposalInfo propinfo = new ProposalInfo();
           propinfo.TitleR = projtitle;
           propinfo.RequestDate = DateTime.Now;
           proj.ProposalInfo = propinfo;
           proj.Label = projectlabel; 
           
           try
           {
               
               db.Projects.InsertOnSubmit(proj);
               db.ProposalInfos.InsertOnSubmit(propinfo);
               db.SubmitChanges();

        
               Budget bd = new Budget();
               bd.BudgetID = proj.ProjectID; 
               db.Budgets.InsertOnSubmit(bd);

           
               BankInfo bnk = new BankInfo();
               bnk.BankInfoID = proj.ProjectID;
               db.BankInfos.InsertOnSubmit(bnk);
             
               db.SubmitChanges();

               return proj.ProjectID; 
           }

           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return -1; 
           }
       
        
       }


          
//      SELECT     Label
//FROM         Projects
//WHERE     (isDeleted <> 'True')
//ORDER BY ProjectID DESC
       public string LastEnteredProject()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = (from p in db.Projects
                         orderby p.ProjectID descending
                         select p.Label).FirstOrDefault();
           return result; 
         
       }

       public IEnumerable<General> GetAllOrganizationList()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.Generals
                        select ls;
           return result;
         
       }

       public bool DeleteReportPeriod(int RepperID, int BudgetID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from r in db.ReportPeriodLists
                               where r.ReportPeriodID == RepperID && r.BudgetID == BudgetID
                               select r).First();
               db.ReportPeriodLists.DeleteOnSubmit(toDelete);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;
       }


       public void UpdateReportPeriodTrans(List<ReportPeriod> ReportPeriods)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               /*var oldItem  = (from p in db.ProjectEvents
                              where p.EventID == item.EventID
                              select p).First();*/
               db.ReportPeriods.AttachAll(ReportPeriods); 
               db.Refresh(RefreshMode.KeepCurrentValues, ReportPeriods);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
            //   result = false;
           }
         
          //return result;
       }


       public void InsertNewCatName(string NewCatName)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           FinArtCatList fcatlist = new FinArtCatList();
           fcatlist.FinArticleCatName = NewCatName;
           try
           {
               db.FinArtCatLists.InsertOnSubmit(fcatlist);
               db.SubmitChanges();
           }

           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               
           }
       
       }


       public void AddFinArticleCategory(int id, int FinCatSel)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           FinArticleCategory fcat = new FinArticleCategory();
           fcat.FinArticleCatID = FinCatSel;
           fcat.BudgetID = id;
           try
           {
               db.FinArticleCategories.InsertOnSubmit(fcat);
               db.SubmitChanges();
           }

           catch(Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
           }

       }
       /// <summary>
       /// Drop Down List.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public IEnumerable<FinArtCatList> GetCatList()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.FinArtCatLists
                        select ls;
           return result;
       }

       public bool DeleteArticleCat(int? FinArticleCatID, int? BudgetID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from c in db.FinArticleCategories
                               where c.BudgetID == BudgetID.Value && c.FinArticleCatID == FinArticleCatID
                               select c).First();
               db.FinArticleCategories.DeleteOnSubmit(toDelete);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);

               result = false;
           }

           return result;
                 
           
       }


       public bool DeleteArticle(int FinArticleID)
       {
           BOTADataContext db = new BOTADataContext(connectString);
       
           bool result = true;

           try
           {
               var toDelete = (from a in db.FinancialArticles
                               where a.FinArticleID == FinArticleID
                               select a).First();
               db.FinancialArticles.DeleteOnSubmit(toDelete);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }

           return result;
                 
       
       }


       public void InsertNewArticle(int FinArticleCatID, int BudgetID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

          //create new FinancialArticle;
           FinancialArticle finart = new FinancialArticle();
           finart.FinArticleCatID = FinArticleCatID;
           finart.BudgetID = BudgetID;
           finart.Info = "";
           finart.Price = 0;
           finart.TimePeriod = 0;
           finart.Times = 0;
           finart.TransferAmt = 0;
           finart.Amt = 0;
           finart.ArticleName = ""; 
           
                           
           db.FinancialArticles.InsertOnSubmit(finart);
           db.SubmitChanges();


           IEnumerable<ReportPeriodList> ReportPeriodsList = from ls in db.ReportPeriodLists
                                                             where ls.BudgetID == BudgetID
                                                             select ls;

           List<ReportPeriod> ReportPeriods = new List<ReportPeriod>();

           foreach (ReportPeriodList rep in ReportPeriodsList)
           {
               ReportPeriod repper = new ReportPeriod();
               repper.ReportPeriodID = rep.ReportPeriodID;
               repper.FinArticleID = finart.FinArticleID;
               repper.Amount = 0;
               ReportPeriods.Add(repper);
           }


           db.ReportPeriods.InsertAllOnSubmit(ReportPeriods.AsEnumerable());
           db.SubmitChanges();
           
       }

       public IEnumerable<ReportPeriodList> GetFinPeriods(int BudgetID)
       {

           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.ReportPeriodLists
                        where ls.BudgetID == BudgetID
                        select ls;
           return result;

       }
       

       public bool CreateReportPeriod(ReportPeriodList item)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           bool result = true;
           try
           {               
               db.ReportPeriodLists.InsertOnSubmit(item);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               result = false;
           }
           return result;
       }



       //USERPOLL FoundUPOLL = polls.Find(delegate(USERPOLL curupoll) { return poll._POLLID == curupoll._POLLID; });
       /*
        Convert.ToInt32(dr["FinArticleCatID"]);
        dr["FinArticleCatName"].ToString(); 
        Convert.ToInt32(dr["FinArticleID"]);
        Convert.ToInt32(dr["BudgetID"]); 
        Convert.ToInt32(dr["FinArticleCatID"]); 
        dr["ArticleName"].ToString(); 
        Convert.ToInt32(dr["Price"]); 
        Convert.ToInt32(dr["Amt"]); 
        Convert.ToInt32(dr["Times"]);  
        Convert.ToInt32(dr["TimePeriod"]);  
        dr["Info"].ToString();    
        Convert.ToInt32(dr["TransferAmt"]);   
        Convert.ToInt32(dr["ReportPeriodID"]);
        Convert.ToInt32(dr["Amount"]);   
       */

       /// <summary>
       /// Gets transactions by budget ID.
       /// </summary>
       /// <param name="bid"></param>
       /// <returns></returns>
       public Budget GetBudgetTransactionByID(int bid)
       {
           bool error = false;
           Budget b = null;
           IDataReader dr = sqlcmd.GetBudgetTransactionByID(bid);

           b = new Budget();
           b.BudgetID = bid;

           try
           {
               using (dr)
               {
                   List<FinArticleCategory> categories = new List<FinArticleCategory>();
                   List<ReportPeriod> reportperiods = new List<ReportPeriod>();
                 

                   while (dr.Read())
                   {
                       if (dr["ContractInitialAmt"] != DBNull.Value)
                       {
                           b.ContractInitialAmt = Convert.ToInt32(dr["ContractInitialAmt"]);
                       }
                        
                       //Special Case If No Article Exists
                       if (dr["FinArticleID"] == DBNull.Value || dr["ReportPeriodID"] == DBNull.Value)
                       { 
                          //We create category, if category exists. 
                           //Category
                           if (dr["CatBudget"] != DBNull.Value && dr["FinArticleCatID"] != DBNull.Value)
                           {
                               FinArticleCategory category = new FinArticleCategory();
                               category.BudgetID = Convert.ToInt32(dr["CatBudget"]);
                               category.FinArticleCatID = Convert.ToInt32(dr["FinArticleCatID"]);
                               FinArtCatList catelist = new FinArtCatList();
                               catelist.FinArticleCatID = Convert.ToInt32(dr["FinArticleCatID"]);
                               catelist.FinArticleCatName = dr["FinArticleCatName"].ToString();
                               FinArticleCategory FoundCategory = categories.Find(delegate(FinArticleCategory curcategory) { return category.FinArticleCatID == curcategory.FinArticleCatID; });

                               //Add it to categories.
                               category.FinArtCatList = catelist;
                               categories.Add(category);
                           }
                       
                       }
                  else    //if article exists.
                       {
                      
                       //Article 
                       FinancialArticle article = new FinancialArticle();
                       article.FinArticleID = Convert.ToInt32(dr["FinArticleID"]);
                       article.ArticleName = dr["ArticleName"].ToString();
                       
                       if (dr["Amt"] != DBNull.Value)
                       { article.Amt = Convert.ToInt32(dr["Amt"]); }
                       else { article.Amt = 0; }

                       article.BudgetID = Convert.ToInt32(dr["BudgetID"]);
                       article.FinArticleCatID = Convert.ToInt32(dr["FinArticleCatID"]);
                       article.Info = dr["Info"].ToString();
                       article.Price = Convert.ToInt32(dr["Price"]);

                 
                       if (dr["TimePeriod"] != DBNull.Value)
                       { article.TimePeriod = Convert.ToInt32(dr["TimePeriod"]); }
                       else { article.TransferAmt = 0; }


                       article.Times = Convert.ToInt32(dr["Times"]);
                       if (dr["TransferAmt"] != DBNull.Value)
                       { article.TransferAmt = Convert.ToInt32(dr["TransferAmt"]); }
                       else { article.TransferAmt = 0; }

                                             

                       //Report Period 
                       ReportPeriod rp = new ReportPeriod();
                       rp.ReportPeriodID = Convert.ToInt32(dr["ReportPeriodID"]);
                       rp.FinArticleID = Convert.ToInt32(dr["FinArticleID"]);
                       rp.Amount = Convert.ToInt32(dr["Amount"]);
                       
                 
                       //Category
                       FinArticleCategory category = new FinArticleCategory();
                       category.BudgetID = Convert.ToInt32(dr["BudgetID"]);  
                       category.FinArticleCatID = Convert.ToInt32(dr["FinArticleCatID"]);
                       FinArtCatList catelist = new FinArtCatList();
                       catelist.FinArticleCatID = Convert.ToInt32(dr["FinArticleCatID"]);
                       catelist.FinArticleCatName = dr["FinArticleCatName"].ToString(); 
                       FinArticleCategory FoundCategory = categories.Find(delegate(FinArticleCategory curcategory) { return category.FinArticleCatID == curcategory.FinArticleCatID; });


                        if (FoundCategory == null)  //if no such category exists 
                        {
                            List<FinancialArticle> articles = new List<FinancialArticle>();
                                                                               
                            
                            //add new report period
                            article.ReportPeriods.Add(rp);

                            //add new article
                            articles.Add(article);

                            category.FinArtCatList = catelist;
                            category.FinancialArticles = ConvertBOTA.ToEntitySet(articles.AsEnumerable());
                            categories.Add(category);

                        }
                        else  //if such category already exists.
                        {
                            FinancialArticle FoundArticle = FoundCategory.FinancialArticles.ToList().Find(delegate(FinancialArticle curarticle) { return article.FinArticleID == curarticle.FinArticleID; });
                            if (FoundArticle == null)
                            {                                 
                               
                                //Add new ReportPeriod.
                                
                                article.ReportPeriods.Add(rp); 
                                FoundCategory.FinancialArticles.Add(article);
                                
                            }
                            else  //if article exists
                            {
                                FoundArticle.ReportPeriods.Add(rp); 

                                //if article already exists, it may have report period with transactions.
                               // ReportPeriod FoundRep = FoundArticle.ReportPeriods.ToList().Find(delegate(ReportPeriod currep) { return rp.ReportPeriodID == currep.ReportPeriodID; });

                               // if (FoundRep == null) //article may be without report period.
                              //  {
                              //      FoundArticle.ReportPeriods.Add(rp);  //add new report period to found article.                               
                              //  }
                              //  else //if report exists.
                              //  {
                                //    FoundRep.FinTransactions.Add(trans);                                                                      
                               // }
                                                                

                            }
                             
                        } //founcategory else.
                      }   //else if no article.
                   }  //while

                 
                   b.FinArticleCategories = ConvertBOTA.ToEntitySet(categories.AsEnumerable()); 
               }
           }
           catch(Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               error = true;
           }
           finally
           {
               dr.Close();
           }

           if (error == true)
           {
               return null;
           }
           else
           {
               return b;
           }




           
           
       }

       public bool UpdateProject(Project proj)
       {
           bool result = true;
           //fuck I changed OrgID ...now have to detach things ....Instead I use direct way.

           string UpdateString = @" UPDATE Projects
                        SET  
                        OrgID = '" + proj.OrgID + @"' 
                        WHERE     (ProjectID = " + proj.ProjectID + ") ";
            result = sqlcmd.Update(UpdateString);

            //Update Categories.
            //       result = sqlcmd.Update(sbcat.ToString()); 

            //Update Articles.


            return result;


       }

   

       /// <summary>
       /// Updates Budget Items.
       /// </summary>
       /// <param name="bud"></param>
       /// <returns></returns>
       public bool UpdateProjectBudget(Budget bud)
       {
           bool result = true;

           StringBuilder sbarticle = new StringBuilder();
           StringBuilder sbcat = new StringBuilder();

           foreach (FinArticleCategory cat in bud.FinArticleCategories)
           {
               //TODO: Update Categories.
//               sbcat.Append(@"Update "); 

               foreach (FinancialArticle article in cat.FinancialArticles)
               {
                   //Update Articles.
                   sbarticle.Append(@" UPDATE FinancialArticle
                        SET  
                        ArticleName = '" + article.ArticleName +  @"', Price = " + article.Price + @", 
                        Amt = " + article.Amt + @", Times = " + article.Times + @", TimePeriod = " + article.TimePeriod + @", 
                        GranteeInput = 1, 
                        DonorInput = 1, 
                        Info = '" + article.Info + @"', 
                        TransferAmt = " + article.TransferAmt + @"  
                        WHERE     (FinArticleID = " + article.FinArticleID + ") ");
               }
           }

           //Update Articles 
           result = sqlcmd.Update(sbarticle.ToString());
           
           //Update Categories.
    //       result = sqlcmd.Update(sbcat.ToString()); 

           //Update Articles.


           return result; 
       }


       public Project GetProjectByID(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           Project result = (from pr in db.Projects
                             where pr.ProjectID == id
                             select pr).FirstOrDefault();
           return result;
            
       }
       
       /* join c in db.FinArticleCategories 
                            on b.BudgetID equals c.BudgetID
                            
                            join a in db.FinancialArticles
                            on b.BudgetID equals a.BudgetID
                                      where b.BudgetID == id && c.FinArticleCatID==a.FinArticleCatID */

       /*
SELECT FinArtCatList.*, FinancialArticle.*
FROM Budget INNER JOIN 
     FinArticleCategory ON FinArticleCategory.BudgetID = Budget.BudgetID INNER JOIN
     FinArtCatList ON FinArticleCategory.FinArticleCatID = FinArtCatList.FinArticleCatID INNER JOIN 
     FinancialArticle ON FinArticleCategory.BudgetID = FinancialArticle.BudgetID AND 
     FinArticleCategory.FinArticleCatID = FinancialArticle.FinArticleCatID
WHERE     (Budget.BudgetID = 2)
ORDER BY FinArtCatList.FinArticleCatID
        */
       /* FOR COMPOSITE KEY USE THIS 
         join o in Orders on new {c.CompanyID, c.CustomerID} equals {o.CompanyID, o.CustomerID}
        */
       public Budget GetBudgetByID(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           Budget bud = null; 
           /*
           var result =  from b in db.Budgets
                         join c in db.FinArticleCategories on b.BudgetID equals c.BudgetID
                         join clist in db.FinArtCatLists on c.FinArticleCatID equals clist.FinArticleCatID
                         join a in db.FinancialArticles 
                         on new {c.BudgetID, c.FinArticleCatID} equals new {a.BudgetID, a.FinArticleCatID}                             
                         where b.BudgetID == id
                         select new {clist, a }; */
             
        
            
           
           //Step1: Get All Categories with Names of the Budget.
           IEnumerable<FinArticleCategory> contextcategories = from b in db.Budgets
                                                        join c in db.FinArticleCategories on b.BudgetID equals c.BudgetID
                                                        join clist in db.FinArtCatLists on c.FinArticleCatID equals clist.FinArticleCatID
                                                        where b.BudgetID == id
                                                        select c;
        

           //Step2: Create My Own Category

           List<FinArticleCategory> categories = new List<FinArticleCategory>();

           foreach (FinArticleCategory category in contextcategories)
           { 
               FinArticleCategory newcat = new FinArticleCategory(); 
               newcat.BudgetID = category.BudgetID;
               newcat.FinArticleCatID = category.FinArticleCatID; 
               newcat.FinArtCatList = category.FinArtCatList;
               categories.Add(newcat); 
                  
           }

         
           //Step2: Get Articles For Each Category. 
           foreach (FinArticleCategory cat in categories)
           {
               IEnumerable<FinancialArticle> articles = from a in db.FinancialArticles
                                                        where a.BudgetID == cat.BudgetID
                                                        && a.FinArticleCatID == cat.FinArticleCatID
                                                        select a;
               if (articles != null)
               {
                 //  category.FinancialArticles.Clear(); //clear the old results from query Step1.
                   foreach(FinancialArticle article in articles)
                   {
                       FinancialArticle finart = new FinancialArticle();
                       /*
                       http://geekswithblogs.net/steveclements/archive/2008/04/15/linq-to-sql-property-changed--changing-logging.aspx
                       article.PropertyChanging += delegate(object send, System.ComponentModel.PropertyChangedEventArgs ea)
                       {
                           ;
                       }; 
                       */
          
                        finart.Amt = article.Amt;
                        finart.ArticleName = article.ArticleName;
                        finart.BudgetID = article.BudgetID;
                        //finart.FinArticleCategory = article.FinArticleCategory;
                        finart.FinArticleCatID = article.FinArticleCatID;
                        finart.FinArticleID = article.FinArticleID;
                        finart.Info = article.Info;
                        finart.Price = article.Price;
                        finart.ReportPeriods = article.ReportPeriods;
                        finart.TimePeriod = article.TimePeriod;
                        finart.Times = article.Times;
                        finart.TransferAmt = article.TransferAmt;
                        
                         cat.FinancialArticles.Add(finart); 
                   }
               }

               bud = new Budget();
               bud.BudgetID = id;
               bud.FinArticleCategories = ConvertBOTA.ToEntitySet(categories.AsEnumerable());
           
           }
            

           
           
//                         select clist, a).First();
           return bud;

       }



       public GrantType GetGrantType(int id)
       {
           TheKey k = new TheKey();
          
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               var result = (from pr in db.GrantTypes
                             where pr.ProjectID == id
                             select pr).First();


               if (result != null)
                   return (GrantType)result;
               else
                   return null; 
           }
           catch(Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return null; 
           }

          
       }



       /// <summary>
       /// Project will have ProgramArea, GrantType and other info Loaded.
       /// </summary>
       /// <param name="id"></param>
       /// <returns>Project</returns>
       public Project GetProposalInfo(int id)
       {
           try
           {
               BOTADataContext db = new BOTADataContext(connectString);

               var result = (from pr in db.Projects
                             where pr.ProjectID == id
                             select pr).First();


               return (Project)result;
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "-------------------------------------------", ex);
               return null; 
           }

           /* join org in db.Organizations
                             on pr.OrgID equals org.OrgID
            join compcode in db.CompetitionCodes
                             on pr.ProjectID equals compcode.ProjectID
                             join programarea in db.ProgramAreas
                             on pr.ProjectID equals programarea.ProjectID 
            */

       }

       public ProjectInfo GetProjInfoOnly(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {
               var result = (from pr in db.ProjectInfos
                             where pr.ProjectInfoID == id
                             select pr).First();
               if (result != null)
                   return (ProjectInfo)result;
               else
                   return null; 
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "-------------------------------------------", ex);
               return null;
           }

           
       }

       //getAllProject IDs where isDeleted not "true"
       public IEnumerable<Project>  GetAllProjectIDs()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           /*
           SELECT [t0].*
FROM [dbo].[Projects] AS [t0]
WHERE [t0].[isDeleted] = 0 or [t0].[isDeleted] is null */
           var result = (from pr in db.Projects
                         where pr.isDeleted != false || !pr.isDeleted.HasValue
                         select pr);
           if (result.Any())
               return result;
           else
               return null; 

       }

       //getAllProject IDs where isDeleted not "true"
       public IEnumerable<Project> GetAllProjectIDs(bool GrantsOnly)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           /*
           SELECT [t0].*
FROM [dbo].[Projects] AS [t0]
WHERE [t0].[isDeleted] = 0 or [t0].[isDeleted] is null */
           if (GrantsOnly)
           {
               var result = (from pr in db.Projects
                             where (pr.isDeleted != false || !pr.isDeleted.HasValue) &&
                             (pr.ProposalStatus.ProposalStatusList.ProposalStatusText == "Active"
                             || pr.ProposalStatus.ProposalStatusList.ProposalStatusText == "Closed"
                             || pr.ProposalStatus.ProposalStatusList.ProposalStatusText == "Suspended"
                             || pr.ProposalStatus.ProposalStatusList.ProposalStatusText == "Terminated")                             
                             select pr);

               if (result.Any())
                   return result;
               else
                   return null;
           }
           else
           {
               return null; 
           }

       }

       /// <summary>
       /// Gets all projects with status isDeleted = true.
       /// </summary>
       /// <returns></returns>
       public IEnumerable<Project> GetAllProjectIsDeleted()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           
           var result = (from pr in db.Projects
                         where pr.isDeleted == true 
                         select pr);

           return result; 
           

       }


       public ProposalInfo GetProposalInfoOnly(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               if (id > 0)
               {

                   var result = (from pr in db.ProposalInfos
                                 where pr.ProjectID == id
                                 select pr).First();
                   
                   if (result != null)
                       return result;
                   else
                       return null;
               }
               else
               {
                   return null;
               }
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "-------------------------------------------", ex);
               return null;
           
           }


        

       }


       public IEnumerable<Role> GetOrgStaffRoles()
       {

           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.Roles
                        select ls;
           return result;

       }
  


       /// <summary>
       /// for drop down list
       /// </summary>
       /// <returns></returns>
       public IEnumerable<ProposalStatusList> GetProposalStatusList()
       {
         
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.ProposalStatusLists
                        select ls;
           return result;         

       }


       /// <summary>
       /// for drop down list
       /// </summary>
       /// <returns></returns>
       public IEnumerable<ProgramAreaList> GetProgramAreaLists()
       {

           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.ProgramAreaLists
                        select ls;
           return result;

       }


       /// <summary>
       /// for drop down list
       /// </summary>
       /// <returns></returns>
       public IEnumerable<GrantTypeList> GetGrantTypeLists()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.GrantTypeLists
                        select ls;
           return result;

       }

        /// <summary>
       /// for drop down list
       /// </summary>
       /// <returns></returns>
       public IEnumerable<LFIndicatorList> LFIndicatorLists()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = (from ls in db.LFIndicatorLists
                        select ls).OrderByDescending(k=>k.LFIndicatorID);
           return result;

       }

       
        /// <summary>
       /// for drop down list
       /// </summary>
       /// <returns></returns>
       public IEnumerable<ProjLocationList> GetProjLocLists()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.ProjLocationLists
                        select ls;
           return result;
       }

       
       /// <summary>
       /// for drop down list
       /// </summary>
       /// <returns></returns>
       public IEnumerable<CompetitionCodeList> CompetitionCodeLists()
       {

           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.CompetitionCodeLists
                        select ls;
           return result;

       }


    }
}
