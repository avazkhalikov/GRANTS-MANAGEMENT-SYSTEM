using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using BOTAMVC3.Helpers;
using BOTACORE.CORE.Services;

namespace BOTAMVC3.Controllers
{
    public class SSSPStaffController : Controller
    {
        //
        // GET: /SSPStaff/
        AppDropDownsService appddservice;
        SSPStaffService staffservice;
        IUserSession session;
        UserActionLogService ulog;
        



        public SSSPStaffController()
        {
            appddservice = new AppDropDownsService();
            staffservice = new SSPStaffService();
            session = new UserSession();
            ulog = new UserActionLogService();
          
        }


        #region Access   ===========


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AccessList()
        {
            //Insert
            var List = staffservice.GetPageAccessList();
            return View(List);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateAccess()
        {
            //Insert           
   
             
            ViewData["RolesList"] = new SelectList(staffservice.GetALLSSPRoles(), "RoleID", "RoleName");

            PageAccess pa = new PageAccess();
            return View(pa);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAccess(PageAccess pa, int? SelectedID)
        {
            if (SelectedID.HasValue)
                pa.RoleId = SelectedID.Value;

            staffservice.CreatePageAccess(pa); 
            return RedirectToAction("AccessList");
        }

        

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteAccess(int? ID)
        {
            PageAccess pa = null;
            if (ID.HasValue)
            {
                pa = staffservice.GetPageAccessByID(ID.Value);
               
            }
            return View(pa);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAccess(PageAccess pa)
        {
            if (pa.ID > 0)
            {
                bool updated = staffservice.DeletePageAccess(pa.ID);
            }

            return RedirectToAction("AccessList");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditAccess(int? ID)
        {
            PageAccess pa = null;
            if (ID.HasValue)
            {
                pa = staffservice.GetPageAccessByID(ID.Value);

            }
            return View(pa);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditAccess(PageAccess pa)
        {
            if (pa.ID > 0)
            {
                bool updated = staffservice.UpdatePageAccess(pa);
            }

            return RedirectToAction("AccessList");
        }

        #endregion ====================================

        public ActionResult LogsView()
        {
           IEnumerable<UserActionLog> ulogs = ulog.GetUserAction();
           return View(ulogs);
        }

        public ActionResult LogsDetails2(int? id)
        {
           UserActionLog ulogsingle = ulog.getUlogByID(id.Value);
            return View(ulogsingle);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Accounts()
        {           
           IEnumerable<SSPStaff> sspstafflist =  staffservice.GetSSPStaffList();
           ViewData["SSPStaffList"] = sspstafflist;
           ViewData["SSPStaffRoles"] = staffservice.GetALLSSPRoles(); 
           return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditAccount(int? ID)
        {
            SSPStaff staff = null; 
            if (ID.HasValue)
            {
                
                staff = staffservice.GetAccountByID(ID.Value);
                ViewData["SSPStaffRoles"] = new SelectList(staffservice.GetALLSSPRoles(), "RoleID", "RoleName");
                
            }

             return View(staff);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditAccount(SSPStaff Account)
        {
            if (Account.SSPStaffID > 0)
            {
                bool updated = staffservice.UpdateSSPAaccount(Account); 
            }

           return  RedirectToAction("EditAccount", new {ID = Account.SSPStaffID }); 
        }


            
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteAccount(int? ID)
        {
            SSPStaff staff = null;
            if (ID.HasValue)
            {
                staff = staffservice.GetAccountByID(ID.Value);
                ViewData["SSPStaffRoles"] = staffservice.GetALLSSPRoles();
            }
            return View(staff);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAccount(SSPStaff Account)
        {
            if (Account.SSPStaffID > 0)
            {
                bool updated = staffservice.DeleteSSPAccount(Account.SSPStaffID); 
            }

            return RedirectToAction("EditAccount", new { ID = Account.SSPStaffID });
        }
        


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AdminView()
        {
             if(session.LoggedIn)
            {
                    if (session.CurrentUser.RolesSSPStaff.RoleName == "Admin")
                    {
                        return View();
                    }
                    else
                    {
                       return RedirectToAction("Search", "ProposalInfo");
                    }                    
                
            }


            return RedirectToAction("Login", "SSPStaff");
          

        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult NewAccount()
        {
            
            SSPStaff spstaff = new SSPStaff();


            ViewData["RolesList"] = new SelectList(staffservice.GetALLSSPRoles(), "RoleID", "RoleName");

            return View(spstaff);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Logout()
        {
            staffservice.Logout(); 
            return RedirectToAction("Login", "SSPStaff");
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(string UserName, string Password)
        {
            SSPStaffService staffservice = new SSPStaffService();

            string LoginStatus = staffservice.Login(UserName, Password);

            if (LoginStatus == "success")
            {
                if (session.CurrentUser.RolesSSPStaff.RoleName == "Admin")
                {
                    return RedirectToAction("AdminView", "SSPStaff");
                }
                else
                {
                    return RedirectToAction("Index", "Tasks");
                }
            }
            else
            { 
               return View();            
            }            

            
        
        }


        /// <summary>
        /// Updates/Creates Given Account
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(SSPStaff staff, int? SelectedID)
        {
            
            SSPStaffService staffservice = new SSPStaffService();

            if(SelectedID.HasValue)
            staff.RoleID = SelectedID.Value; 

            bool Created =  staffservice.CreateNewAccount(staff);
                     
            if(Created) 
            ViewData["AccountCreated"] = "Success"; 
            else
            ViewData["AccountCreated"] = "Failed";

           return RedirectToAction("Login");

        }



        #region DROP DOWNS

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AppDropDowns()
        {

          ViewData["EventType"] = appddservice.GetEventTypeList();
          ViewData["OrgLegalStatus"] = appddservice.GetLegalStatusList();
          ViewData["GrantType"] = appddservice.GetGrantTypeList();
          ViewData["ProgramArea"] = appddservice.GetProgramAreaList();
          ViewData["CompetitionCode"] = appddservice.GetCompetitionCodeList();
          ViewData["ProposalStatus"] = appddservice.GetProposalStatusList();
          ViewData["SSPRoles"] = staffservice.GetALLSSPRoles();
          ViewData["Regions"] = appddservice.GetRegionList();
          ViewData["GetBudgetCatList"] = appddservice.GetCatList();
          ViewData["ProjectLocationList"] = appddservice.GetProjectLocationList();
          
          return View();
        }
        #endregion 

        #region BudgetDropDown

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertNewCatName(string NewCatName)
        {

            appddservice.InsertNewCatName(NewCatName);
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ArticleCatListEdit(List<FinArtCatListR> FinArtCatListR)
        {
            if (FinArtCatListR.Count() > 0)
            {
                bool Updated = appddservice.ArticleCatListEdit(FinArtCatListR);
            }
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteArticleCat(int? CatListID)
        {
             //Delete
            if (CatListID.HasValue)
            {
                bool deleted = appddservice.DeleteArticleCat(CatListID.Value);
            }

            return RedirectToAction("AppDropDowns");
        }
       
        #endregion 





        #region SSP Roles Edit
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteSSPRoles(int? SSPRolesID)
        {
            //Delete
            if (SSPRolesID.HasValue)
            {
                bool deleted = appddservice.DeleteSSPRoles(SSPRolesID.Value);
            }

            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertSSPRoles()
        {
            //Insert
            bool Inserted = appddservice.InsertSSPRoles();
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SSPRolesEdit(List<RolesSSPStaff> SSPRolesList)
        {
            if (SSPRolesList.Count() > 0)
            {
                bool Updated = appddservice.UpdateSSPRoles(SSPRolesList);
            }
            return RedirectToAction("AppDropDowns");
        }
        #endregion 







        #region Legal Status Edit
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteLegalStatus(int? LegalStatID)
        { 
            //Delete
            if (LegalStatID.HasValue)
            {
                bool deleted = appddservice.DeleteLegalStatus(LegalStatID.Value);
            }
            
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertLegalStatus()
        {
            //Insert
            bool Inserted = appddservice.InsertLegalStatus();
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LegalStatusEdit(List<LegalStatusList> LegStatusList)
        {
            if (LegStatusList.Count() > 0)
            {
                bool Updated = appddservice.UpdateLegalStatus(LegStatusList);
            }
            return RedirectToAction("AppDropDowns");
        }
        #endregion 


        #region EventType
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteEventTypeList(int? EventTypeID)
        {
            //Delete
            if (EventTypeID.HasValue)
            {
                bool deleted = appddservice.DeleteEventTypeList(EventTypeID.Value);
            }

            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertEventTypeList()
        {
            //Insert
            bool Inserted = appddservice.InsertEventTypeList();
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EventTypeListEdit(List<EventType> EventTypeList)
        {
            if (EventTypeList.Count() > 0)
            {
                bool Updated = appddservice.EventTypeListEdit(EventTypeList);
            }

            return RedirectToAction("AppDropDowns");
        }
        #endregion 
        

       
        #region RegionList
              
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteRegionList(int? RegionListID)
        {
            //Delete
            if (RegionListID.HasValue)
            {
                bool deleted = appddservice.DeleteRegionList(RegionListID.Value);
            }

            return RedirectToAction("AppDropDowns");

            
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertRegionList()
        {
            //Insert
            bool Inserted = appddservice.InsertRegionList();
            return RedirectToAction("AppDropDowns");           
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RegionListEdit(List<RegionList> RegionList)
        {
            if (RegionList.Count() > 0)
            {
                bool Updated = appddservice.RegionListEdit(RegionList);
            }
            return RedirectToAction("AppDropDowns");

            
        }
        #endregion
                 


        #region GrantType
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteGrantTypeList(int? GrantTypeListID)
        {
            //Delete
            if (GrantTypeListID.HasValue)
            {
                bool deleted = appddservice.DeleteGrantTypeList(GrantTypeListID.Value);
            }

            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertGrantTypeList()
        {
            //Insert
            bool Inserted = appddservice.InsertGrantTypeList();
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GrantTypeListEdit(List<GrantTypeList> GrantTypeList)
        {
            if (GrantTypeList.Count() > 0)
            {
                bool Updated = appddservice.GrantTypeListEdit(GrantTypeList);
            }
            return RedirectToAction("AppDropDowns");
        }
        #endregion 


        #region Project Location List
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteProjectLocationList(int? ProjectLocationListID)
        {
            //Delete
            if (ProjectLocationListID.HasValue)
            {
                bool deleted = appddservice.DeleteProjectLocationList(ProjectLocationListID.Value);
            }

            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertProjectLocationList()
        {
            //Insert
            bool Inserted = appddservice.InsertProjectLocationList();
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProjectLocationListEdit(List<ProjLocationList> ProjectLocationList)
        {
            if (ProjectLocationList.Count() > 0)
            {
                bool Updated = appddservice.ProjectLocationListEdit(ProjectLocationList);
            }
            return RedirectToAction("AppDropDowns");
        }
        #endregion




        #region Program Area List
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteProgramAreaList(int? ProgramAreaListID)
        {
            //Delete
            if (ProgramAreaListID.HasValue)
            {
                bool deleted = appddservice.DeleteProgramAreaList(ProgramAreaListID.Value);
            }

            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertProgramAreaList()
        {
            //Insert
            bool Inserted = appddservice.InsertProgramAreaList();
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProgramAreaListEdit(List<ProgramAreaList> ProgramAreaList)
        {
            if (ProgramAreaList.Count() > 0)
            {
                bool Updated = appddservice.ProgramAreaListEdit(ProgramAreaList);
            }
            return RedirectToAction("AppDropDowns");
        }
        #endregion 


        #region Competition Code
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteCompetitionCodeList(int? ID)
        {
            //Delete
            if (ID.HasValue)
            {
                bool deleted = appddservice.DeleteCompetitionCodeList(ID.Value);
            }

            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertCompetitionCodeList()
        {
            //Insert
            bool Inserted = appddservice.InsertCompetitionCodeList();
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompetitionCodeListEdit(List<CompetitionCodeList> List)
        {
            if (List.Count() > 0)
            {
                bool Updated = appddservice.CompetitionCodeListEdit(List);
            }
            return RedirectToAction("AppDropDowns");
        }
        #endregion 


        #region ProposalStatusList
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteProposalStatusList(int? ID)
        {
            //Delete
            if (ID.HasValue)
            {
                bool deleted = appddservice.DeleteProposalStatusList(ID.Value);
            }

            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult InsertProposalStatusList()
        {
            //Insert
            bool Inserted = appddservice.InsertProposalStatusList();
            return RedirectToAction("AppDropDowns");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProposalStatusListEdit(List<ProposalStatusList> List)
        {
            if (List.Count() > 0)
            {
                bool Updated = appddservice.ProposalStatusListEdit(List);
            }
            return RedirectToAction("AppDropDowns");
        }
        #endregion 



    }
}
