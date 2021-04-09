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
    public class ReportPeriodController : BOTAController
    {
        // GET: /ProposalInfo/

        public ActionResult Index(int? id)
        {
            if(!id.HasValue)
            {
               return RedirectToAction("Search", "ProposalInfo");             
            }
            
            return RedirectToAction("ReportPeriods", new {id=id.Value});
        }





        ProjectService projservice;
        Project _project; 
        public ReportPeriodController()
        {
            projservice = new ProjectService();
        
        }

        // GET: /ProposalInfo/View/1
        public ActionResult ReportPeriods(int? id)
        {
            

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            //this is suppose to get the BudgetID of current Project.
            
            //GetBudgetIDofProject(id);
            _project = projservice.GetProposalInfo(id.Value);   //This will happen In MenuITEM Controller orgservice.GetOrganizationGeneralOfCurrentProposal(id.Value);

            //Create new SelectList.
            List<SelectListItem> PaymentStat = new List<SelectListItem>();
            PaymentStat.Add(new SelectListItem() { Value = "1", Text = "Pending" });
            PaymentStat.Add(new SelectListItem() { Value = "2", Text = "Transfered" });

            ViewData["PaymentStat"] = PaymentStat;
                     

            //   ViewData["ProgramAreaList"] = new SelectList(projservice.GetProgramAreaList(), "ProgramAreaCodeID", "ProgramAreaText");
         
            
            ViewData["ProposalID"] = id.Value;
            ViewData["BudgetID"] = _project.Budget.BudgetID;
            ViewData["initialbudget"] = _project.Budget.ContractInitialAmt; 
            
            return View(_project.Budget.ReportPeriodLists);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Insert(int id)
        {

            ViewData["ProposalID"] = id;
            ViewData["BudgetID"] = id;  //suppose to be BudgetID.
            ReportPeriodList r = new ReportPeriodList();   //because View requires Object ReportPeriod because of it is strongly typed
            return View(r);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteReportPeriod(int RepperID, int BudgetID)
        {
            bool status = projservice.DeleteReportPeriod(RepperID, BudgetID);

            return RedirectToAction("ReportPeriods", new { id = BudgetID });
        
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(ReportPeriodList repperiod, int id)
        {
            //Insert INTO DB.
           
          if (repperiod.PeriodStart.HasValue && repperiod.PeriodStart.HasValue)
          {
              repperiod.PeriodTitle = repperiod.PeriodStart.Value.ToShortDateString() + "<br />" + repperiod.PeriodEnd.Value.ToShortDateString();
          }
                
           bool status = projservice.CreateReportPeriod(repperiod);

           return RedirectToAction("ReportPeriods", new { id=id});
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(List<ReportPeriodList> repper, int? initialamt, /**Project item,/*List<FinancialArticle> finarticle*//*, ICollection<Contact> Contacts*/ int? id, string actioncase)
        {

                       
            switch (actioncase)
            {

                case "Insert":
                    return RedirectToAction("Insert");
                    //   IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                    //   TempData["searchresult"] = orgs;
            

                case "Search":
                 //   IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                 //   TempData["searchresult"] = orgs;
                    break;
                case "Update":

                    if (!initialamt.HasValue)
                    {
                        initialamt = 0; 
                    }

                   //Update Initial Amount 
                    if (repper != null)
                    {
                        for (int i = 0; i < repper.Count(); i++)
                        {
                            try
                            {
                                repper[i].PeriodTitle = repper[i].PeriodStart.Value.ToShortDateString() + " <br /> " + repper[i].PeriodEnd.Value.ToShortDateString();
                            }
                            catch
                            {
                                repper[i].PeriodTitle = "period entered wrong"; 
                            }
                        }
                        projservice.UpdateReportPeriodList(repper);
                    }
                   
                    projservice.UpdateBudgetInitialAmount(initialamt.Value, id.Value);          

                          //projservice.UpdateProjectBudget(proj); 
                    break;

                case "Delete":
                    // Delete organization GeneralInfo.
                    //  orgservice.DeleteOrganization(h_contactid);
                    break;

            }

            return RedirectToAction("ReportPeriods", new { id = id });

            /*  Organization _org = orgservice.GetOrganizationOfCurrentProposal(id.Value);
              if (_org != null)
              {
                  item.EntityKey = _org.EntityKey;
                
              } */

        }

    }
}
