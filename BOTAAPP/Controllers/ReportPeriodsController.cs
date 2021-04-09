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
    public class ReportPeriodsController : BOTAController
    {
        //
        // GET: /ReportPeriods/

        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

  
            return RedirectToAction("Reppers", new { id = id });
        }


        BudgetService budservice;
        ProjectService ps;
        UserActionLogService ulog;
        public ReportPeriodsController()
        {
            budservice = new BudgetService();
            ps = new ProjectService();
            ulog = new UserActionLogService();
        
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Insert(int id)
        {
            if (RedirectIfAccessFin())
            {
                ViewData["ProposalID"] = id;
                ViewData["BudgetID"] = id; //suppose to be BudgetID.
                ReportPeriodListR r = new ReportPeriodListR();
                    //because View requires Object ReportPeriod because of it is strongly typed
                return View(r);
            }
            else
            {
                return RedirectToAction("Index", "Error", new { error = "Bugdet can't be changed." });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(ReportPeriodListR repperiod, int id)
        {
            //Insert INTO DB.
            if (RedirectIfAccessFin())
            {
                if (repperiod.PeriodStart.HasValue && repperiod.PeriodStart.HasValue)
                {
                    repperiod.PeriodTitle = repperiod.PeriodStart.Value.ToShortDateString() + "<br />" +
                                            repperiod.PeriodEnd.Value.ToShortDateString();
                }

                repperiod.PaymentStatus = 1;

                bool status = budservice.CreateReportPeriod(repperiod);

                return RedirectToAction("Reppers", new {id = id});
            }
            else
            {
                return RedirectToAction("Index", "Error", new { error = "Bugdet can't be changed." });
            }
        }




        // GET: /ProposalInfo/View/1
        public ActionResult Reppers(int? id)
        {


            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            //Reports Generated Here...
            //report(id.Value); 
            //---------------------

             //LEFT COLUMN. REPPERs
             IEnumerable<ReportPeriodListR> reps= budservice.GetFinPeriods(id.Value);
             ViewData["reps"] = reps;
             
             if (!reps.Any())  //if have not created report periods....tell them to create all report periods
             {
                 ViewData["WarningCreateReps"] = "Step1: Create Report Periods. Step2: Create Budget"; 
             }

            //Create new SelectList.
            List<SelectListItem> PaymentStat = new List<SelectListItem>();
            PaymentStat.Add(new SelectListItem() { Value = "1", Text = "Pending" });
            PaymentStat.Add(new SelectListItem() { Value = "2", Text = "Transfered" });
           // PaymentStat.Add(new SelectListItem() { Value = "3", Text = "Refund" });

            ViewData["PaymentStat"] = PaymentStat;
            ViewData["ProposalID"] = id.Value;
            ViewData["BudgetID"] = id.Value;

            Budget b = budservice.GetBudget(id.Value);
            ViewData["initialbudget"] = b.ContractInitialAmt; // budservice.GetBudgetInitialAmount(id.Value);


            //RIGHT COLUMN PAYMENTS.
            List<BudgetPaymentReport> bpayrep = report(id.Value);
            ViewData["PaymentReports"] = bpayrep; 
      
            //Cancellation calculation.
            ProjectService projs = new ProjectService(); 
            ProjectInfo ProjInfo = projs.GetProjInfoOnly(id.Value);
            decimal SumTransfered = 0;
            //furich...
            foreach (BudgetPaymentReport bpr in bpayrep)
            {
                   SumTransfered = SumTransfered + bpr.PeriodGotMoney; 
            }

            try
            {
                ViewData["Cancellation"] = ProjInfo.AwardedAmt.Value - b.Cancellation - SumTransfered;  // . Awarded Amt - total paid amt
            }
            catch
            {
                ViewData["Cancellation"] = 0;
            }

            //Refund = totaltransfered - totalotchet

            decimal SumSpent = 0;

            //furich...
            foreach (BudgetPaymentReport bpr in bpayrep)
            {
                SumSpent = SumSpent + bpr.MoneySpent;
            }

            try
            {
                ViewData["Refund"] = SumTransfered - SumSpent - b.Returned;  // . Awarded Amt - total paid amt
            }
            catch
            {
                ViewData["Refund"] = 0;
            }



            return View();
        }

           


        // GET: /ProposalInfo/View/1
        public ActionResult ReportPeriods(int? id)
        {


            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            //Reports Generated Here...
            //report(id.Value); 
            //---------------------


            ViewData["reps"] = budservice.GetFinPeriods(id.Value);

            
            //Create new SelectList.
            List<SelectListItem> PaymentStat = new List<SelectListItem>();
            PaymentStat.Add(new SelectListItem() { Value = "1", Text = "Pending" });
            PaymentStat.Add(new SelectListItem() { Value = "2", Text = "Transfered" });

            ViewData["PaymentStat"] = PaymentStat;

                        

            ViewData["ProposalID"] = id.Value;
            ViewData["BudgetID"] = id.Value;

            //Budget bdg = budservice.GetBudget(id.Value); 

            ViewData["initialbudget"] = budservice.GetBudgetInitialAmount(id.Value);


            ViewData["PaymentReports"] = report(id.Value); 

            

            return View();
        }

        public List<BudgetPaymentReport> report(int id)
        {
           int BudgetID = id;

           IEnumerable<ReportPeriodListR> _repperR = budservice.GetFinPeriods(BudgetID);
           IEnumerable<FinArticleCategoryR> _finartcats = budservice.GetFinArticleCategory(BudgetID); 
           BudgetAggregateService budaggs = new BudgetAggregateService(_finartcats.ToList(), _repperR.ToList());
           CatRepView catrepview = budaggs.AccumulateCatRep();
          
           BudgetPaymentReportService budpayreportservice = new BudgetPaymentReportService(_repperR, catrepview);

           int bdinitamt = budservice.GetBudgetInitialAmount(id);

           List<BudgetPaymentReport> bpayrep = budpayreportservice.GenerateReports2(bdinitamt);


           return bpayrep; 
       //     public BudgetAggregateService(List<FinArticleCategoryR> _finartcats, List<ReportPeriodListR> _repperR)
       // {
      //     Budget bd = budservice.GetBudget(BudgetID); 

        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteReportPeriod(int RepperID, int BudgetID)
        {
            if (RedirectIfAccessFin())
            {
            bool status = budservice.DeleteReportPeriod(RepperID, BudgetID);

            return RedirectToAction("Reppers", new { id = BudgetID });
            }
             else
             {
                 return RedirectToAction("Index", "Error", new { error = "Bugdet can't be changed." });
             }
        }


  

        public ReportPeriodListR GetRepper(int RepperID, int BudgetID)
        { 
           IEnumerable<ReportPeriodListR> reppers = budservice.GetFinPeriods(BudgetID);

                foreach (ReportPeriodListR repper in reppers)
                {
                    if (repper.ReportPeriodID == RepperID)   //if found. 
                    {
                        return repper;
                    }
                
                }

                return null; 
        }



        public EventType GetPaymentEventType()
        {
            AppDropDownsService appdrop = new AppDropDownsService();
            IEnumerable<EventType> eventsList = appdrop.GetEventTypeList();


            foreach (EventType evtp in eventsList)
            {
                if (evtp.EventTypeName == "Payment")
                {
                    return evtp; 
                }
            }

            return null; 
        
        }

        public ActionResult ProjectEventAcceptanceMemoGenerator(int RepperID, int BudgetID)
        {

            if (RepperID > 0 && BudgetID > 0)
            {
                ReportPeriodListR repper = GetRepper(RepperID, BudgetID);

                //Create Event.
                ProjectEventService pes = new ProjectEventService();
                ProjectEvent o = new ProjectEvent();
                o.CompletedDate = repper.PaymentDate;
                o.EventDescription = "Letter to Grantee";
                o.ProjectID = BudgetID;
                //o.EventType = GetPaymentEventType();
                o.EventTypeID = GetPaymentEventType().EventTypeID;
                o.ReportPeriodID = RepperID;
                o.SSPOrGrantee = true;
                o.EventStatus = 0;

                pes.Insert(o);

                EventTemplate eh = new EventTemplate();
                TemplateService ts = new TemplateService();

                //Generate Document.
                TemplateDocument td1 = null;
                 td1 = ts.GetTemplateDocument("письмо о принятии отчета");
                if (td1 != null)
                {
                    eh.CreateFromTemplate(td1.TemplateDocsID, BudgetID, o.EventID, RepperID);
                }

            }

            return RedirectToAction("Index", "Events", null);

           // return RedirectToAction("Reppers", new { id = BudgetID });
        }




        public ActionResult ProjectEventPaymentGenerator(int RepperID, int BudgetID)
        {

            if (RepperID > 0 && BudgetID > 0)
            {
                ReportPeriodListR repper = GetRepper(RepperID, BudgetID); 
              
                ProjectEventService pes = new ProjectEventService();
                ProjectEvent o = new ProjectEvent();
                o.CompletedDate = repper.PaymentDate; 
                o.EventDescription = "Payment Request";
                o.ProjectID = BudgetID; 
                //o.EventType = GetPaymentEventType();
                o.EventTypeID = GetPaymentEventType().EventTypeID;
                o.ReportPeriodID = RepperID; 
                o.SSPOrGrantee = true;
                o.EventStatus = 0; 
                pes.Insert(o);

                EventTemplate eh = new EventTemplate();
                TemplateService ts = new TemplateService();

                TemplateDocument td1 = null;
                td1=ts.GetTemplateDocument("Запрос на Оплату");
                if (td1 != null)
                {
                    eh.CreateFromTemplate(td1.TemplateDocsID, BudgetID, o.EventID, RepperID);
                }                              

            }

            return RedirectToAction("Reppers", new { id = BudgetID });
        }


        public bool RedirectIfAccessFin()
        {
            //get project.
            Project propinfo = null;
            if (session != null)
            {
                if (session.ProjectID > 0)
                {
                    propinfo = ps.GetProjectByID(session.ProjectID);


                    //rule! check if project is not closed.
                    if (propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Active" ||
                        propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Inquire" ||
                        propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Proposal")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(List<ReportPeriodListR> repper, int? initialamt,  int? id, string actioncase)
        {
              //get project.
         Project propinfo = null;
         if (session != null)
         {
             if (session.ProjectID > 0)
             {
                 propinfo = ps.GetProjectByID(session.ProjectID);


                 //rule! check if project is not closed.
                 if (propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Active" ||
                     propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Inquire" ||
                     propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Proposal")
                 {

                     switch (actioncase)
                     {

                         case "Insert":
                             return RedirectToAction("Insert");

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
                                         repper[i].PeriodTitle = repper[i].PeriodStart.Value.ToShortDateString() +
                                                                 " <br /> " +
                                                                 repper[i].PeriodEnd.Value.ToShortDateString();
                                     }
                                     catch
                                     {
                                         repper[i].PeriodTitle = "period entered wrong";
                                     }
                                 }
                                 budservice.UpdateReportPeriodList(repper);
                             }

                             budservice.UpdateBudgetInitialAmount(initialamt.Value, id.Value);

                             break;


                     }

                     return RedirectToAction("Reppers", new {id = id});

                 }
                 else
                 {

                     return RedirectToAction("Index", "Error", new { error = "Project is not active, therefore bugdet can't be changed." });
                 }
             }
         }

         return RedirectToAction("Index", "Error", new { error = "Bugdet can't be changed." });
            /*  Organization _org = orgservice.GetOrganizationOfCurrentProposal(id.Value);
              if (_org != null)
              {
                  item.EntityKey = _org.EntityKey;
                
              } */

        }


    }
}
