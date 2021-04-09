using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using System.Text;
using System.Data.Linq;
using BOTAMVC3.Helpers;
namespace BOTAMVC3.Controllers
{
    public class ProjectInfoController : BOTAController
    {
     //
        // GET: /ProposalInfo/
        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo"); 
        }

        ProjectService projservice;
        public ProjectInfoController()
        {
            projservice = new ProjectService();
        
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult view2(/*IEnumerable<ProposalInfo> propinfo*/)
        {

        
            /*
            if (TempData["searchresult"] != null)
            {
                ViewData["Results"] = TempData["searchresult"];
            }
            return View(_organization.General);
            */
            return View();
        }

        // GET: /ProposalInfo/View/1
        public ActionResult View(int? id)
        {
            

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            Project _project = projservice.GetProposalInfo(id.Value);   //This will happen In MenuITEM Controller orgservice.GetOrganizationGeneralOfCurrentProposal(id.Value);

            if (_project.ProjectInfo == null) //no ProjectInfo.
            {
                _project.ProjectInfo = new ProjectInfo(); //give some instance.
            }


            FinanceResults finres = new FinanceResults(id.Value);
           // ViewData["moneyLeft"] = finres.Project_TotalAmountLeftFromAwardAmount();
            ViewData["CashOnHand"] = finres.Project_TotalCashOnHand(); 
            ViewData["MoneyTransfered"] = finres.Project_TotalMoneyTransferedFromAwardAmount();


        //    ViewData["StaffRoleList"] = new SelectList(projservice.GetOrgStaffRoles(), "RoleID", "RoleTitle"); 
         
            
            //   ViewData["ProgramAreaList"] = new SelectList(projservice.GetProgramAreaList(), "ProgramAreaCodeID", "ProgramAreaText");
         
            
            ViewData["ProposalID"] = id.Value;



            //Get Events with Type: Site Visit
            //Get All Events with Type: Narrative Reports

            ProjectEventService evnts = new ProjectEventService(); 
            IEnumerable<ProjectEvent> projevnts = evnts.GetProjectEventList(id.Value);
            int sitevisits = 0;
            int narrativereport = 0;
            int narrativeaccepted = 0;
            int narrativerejected = 0; 

           //   Project Info=> Accepted Narrative Reports Rename to Current Grant Evaluation
           //Count Evaluation not Narrative Report.
            foreach (ProjectEvent projevnt in projevnts)
            {
               
                if (projevnt.EventType.EventTypeName == "Site Visit" || projevnt.EventType.EventTypeName == "Site visit")
                {
                    sitevisits++; 
                }

                //if (projevnt.EventDescription != null)
                //{
                //    if (projevnt.EventDescription.Contains("Site Visit") || projevnt.EventDescription.Contains("Site visit"))
                //    {
                //        sitevisits++;
                //    }
                //}



                if (projevnt.EventType.EventTypeName == "Evaluation Report" || projevnt.EventType.EventTypeName == "Evaluation report")
                    //|| projevnt.EventDescription.Contains("Narrative Report") || projevnt.EventDescription.Contains("Narrative report"))
                {
                    narrativereport++;

                    if (projevnt.ReportStatus.HasValue)
                    {
                        if (projevnt.ReportStatus.Value == 1)
                            narrativeaccepted++;
                        if (projevnt.ReportStatus.Value == 0)
                            narrativerejected++;
                    }
                }
            
            }

            ViewData["NumberOfSiteVisits"] = sitevisits;
            ViewData["NumberOfAcceptedNarrative"] = narrativeaccepted;
            ViewData["NumberOfRejectedNarrative"] = narrativerejected; 

           // EventType evType = new EventType();
          //  evType.EventTypeName
          //  evntTypeSer.GetEventTypeList();


            
            return View(_project);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int? id, Project project)
        {

            //Update Proposal Info.

            projservice.ProjectInfoUpdate(project.ProjectInfo); 

            return RedirectToAction("View", new { id = id });


        }


    }
}
