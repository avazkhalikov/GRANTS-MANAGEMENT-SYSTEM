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
using System.Web.Script.Serialization;

namespace BOTAMVC3.Controllers
{
    public class BudgetController : BOTAController
    {
        //
        // GET: /Budget/

        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo");
        }

        BudgetService budservice;
        UserActionLogService ulog;
        ProjectService ps;
        public BudgetController()
        {
            budservice = new BudgetService();
            ulog = new UserActionLogService();
            ps = new ProjectService();
        
        }


        /// <summary>
        /// Inserts into Drop Down list a new Category.
        /// </summary>
        /// <param name="FinCatName"></param>
        /// <param name="BudgetID"></param>
        /// <returns></returns>
        public ActionResult InsertFinCatDropDown(string NewCatName, int? id)
        {
            //Needed FinArticleCatID, BudgetID

            budservice.InsertNewCatName(NewCatName); 
            return RedirectToAction("View", new { id = id });

        }

        /// <summary>
        /// Inserts a new Financial Category.
        /// </summary>
        /// <param name="FinCatSel"></param>
        /// <param name="BudgetID"></param>
        /// <returns></returns>
        public ActionResult InsertFinCat(int? FinCatSel, int? id)
        {
            //Needed FinArticleCatID, BudgetID

            if (RedirectIfAccessFin())
            {
                int status = budservice.AddFinArticleCategory(id.Value, FinCatSel.Value);

                if (status == -2) //Report Periods don't exist.
                {
                    return RedirectToAction("View", new {id = id, errorcode = -2});

                    // return RedirectToAction("reppers", "ReportPeriods", new { id = id });
                }

                return RedirectToAction("View", new {id = id});
             }
            else
            {
                return RedirectToAction("Index", "Error", new { error = "Bugdet can't be changed." });
            }

        }


        public ActionResult UpdateFinCat(List<ReportPeriodR> repper, List<FinArticleCategoryR> finartcat,
            List<FinArtCatListR> FinArtCatListR, int? id, string infoBox, string Cancellation, string Returned)
        {
            Decimal CancellationInt = 0;
            Decimal ReturnedInt = 0;
            Decimal.TryParse(Cancellation, out CancellationInt);
            Decimal.TryParse(Returned, out ReturnedInt);

            //trim it all and get only 50 chars. Limit!
            infoBox = infoBox.Trim();
            
            if(infoBox.Length > 50)
            infoBox = infoBox.Substring(0, 50);

            //get project.
            Project propinfo = null; 
         if(session != null)
         {
            if(session.ProjectID > 0)
            {
             propinfo = ps.GetProjectByID(session.ProjectID);
          

            //rule! check if project is not closed.
            if (propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Active" || 
                propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Inquire" ||
                propinfo.ProposalStatus.ProposalStatusList.ProposalStatusText == "Proposal")
            {
                //Needed FinArticleCatID, BudgetID
                try
                {
                    budservice.UpdateCategoriesValues(finartcat);
                }
                catch (Exception ex)
                {
                   return RedirectToAction("Index", "Error", new { error = ex.ToString() });
                }

                try
                {
                    budservice.UpdateReportPeriodTrans(repper);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", new { error = ex.ToString() });
                }


                try
                {
                    budservice.UpdateBudget(id.Value, infoBox, CancellationInt, ReturnedInt);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", new { error = ex.ToString() });
                }



                //Log Action.
                try
                {
                    UserActionLog ulogact = new UserActionLog();
                    ulogact.Action = "Update";
                    ulogact.Date = DateTime.Now;
                    string output = null;
                    //JavaScriptSerializer jss = new JavaScriptSerializer();
                    //foreach (ReportPeriodR rr in repper)
                    //{
                    //    output = output + jss.Serialize(finartcat);
                    //}

                    foreach (ReportPeriodR rr in repper)
                    {
                        output = output + " amount: " + rr.Amount;
                    }

                    ulogact.ProjectLabel = session.ProjectLabel;
                    ulogact.ProjectID = session.ProjectID;
                    ulogact.Data = output;
                    ulogact.Section = "Budget";
                    ulogact.UserName = session.CurrentUser.username;
                    ulog.Insert(ulogact);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Error", new { error = ex.ToString() });
                }


              

                
            }
            else
            {
               return RedirectToAction("Index", "Error", new { error = "Project is not active, therefore bugdet can't be changed." });
              
            }
         }
       }
            
            return RedirectToAction("View", new { id = id });

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

        public ActionResult DeleteCatArticle(int? FinArticleID, int? BudgetID)
        {
            if (RedirectIfAccessFin())
            {
                bool status = false;
                if (BudgetID != null && FinArticleID != null)
                {
                    status = budservice.DeleteArticleCat(FinArticleID.Value, BudgetID.Value);
                }

                return RedirectToAction("View", new {id = BudgetID});
            }
            else
            {
                return RedirectToAction("Index", "Error", new { error = "Bugdet can't be changed." });
            }
        }


        // GET: /ProposalInfo/View/1
        public ActionResult View(int? id, int? errorcode)
        {


            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            if (errorcode.HasValue)
            {
                if (errorcode.Value == -2)
                {
                    ViewData["Error"] = "You must Insert Report Periods First!";
                }
            }

            int BudgetID = id.Value;
            //   Project _project = projservice.GetProposalInfo(id.Value);   //This will happen In MenuITEM Controller orgservice.GetOrganizationGeneralOfCurrentProposal(id.Value);

            //    Budget _budget = projservice.GetBudgetBYID(id.Value);  //LINQ ONE.

            IEnumerable<ReportPeriodListR> _repperR = budservice.GetFinPeriods(BudgetID);
            IEnumerable<FinArticleCategoryR> _finartcats = budservice.GetFinArticleCategory(BudgetID);
            BudgetAggregateService budaggs = new BudgetAggregateService(_finartcats.ToList(), _repperR.ToList());
            CatRepView catrepview = budaggs.AccumulateCatRep();

            ViewData["RepView"] = catrepview; 
           
          //  IEnumerable<FinArticleCategoryR> catr = budservice.GetFinArticleCategory(id.Value);// projservice.GetBudgetTransactionByID(id.Value);
            ViewData["ArtCat"] = _finartcats;

            Budget b = budservice.GetBudget(id.Value); 
            ViewData["InfoBox"] = b.InfoBox;
            ViewData["Cancellation"] = b.Cancellation;
            ViewData["Returned"] = b.Returned;
            // ViewData["BudgetCatList"] = new SelectList(projservice.GetCatList(), "FinArticleCatID", "FinArticleCatName");

            //   ViewData["ProgramAreaList"] = new SelectList(projservice.GetProgramAreaList(), "ProgramAreaCodeID", "ProgramAreaText");

       
            
            ViewData["BudgetCatList"] = new SelectList(budservice.GetCatList(), "FinArticleCatListID", "FinArticleCatName");

            ViewData["BudgetID"] = id.Value;

            ViewData["PeriodList"] = new SelectList(budservice.GetFinPeriods(id.Value), "ReportPeriodID", "PeriodTitle");

            ViewData["RepPeriodList"] = budservice.GetFinPeriods(id.Value); 

           

            return View();
        }

    }
}
