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
    public class TransactionController : BOTAController
    {
     //
        // GET: /ProposalInfo/

        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo"); 
        }

        ProjectService projservice;
        public TransactionController()
        {
            projservice = new ProjectService();
        
        }


        public ActionResult aggregated(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            int BudgetID = id.Value;
            IEnumerable<ReportPeriodList> reppers = projservice.GetFinPeriods(BudgetID);


            SelectList selList = null;
            if (reppers.Count() > 0)
            {
                selList = new SelectList(reppers, "ReportPeriodID", "PeriodTitle", reppers.First());
            }

           
            
            ViewData["PeriodList"] = selList;
            ViewData["ProposalID"] = id.Value;

            Budget _bud2 = projservice.GetBudgetTransactionByID(id.Value);
            //
            //AggregateViewHelper aggrv = new AggregateViewHelper(_bud2);
            FinanceAggregateService aggrv2 = new FinanceAggregateService(_bud2); 

            List<CatRepViewHelper> catrep = aggrv2.GetView(reppers.Count());
            ViewData["CatRepView"] = catrep;
            CatRepViewHelper footeracum = aggrv2.GetAcumulatedFooter(catrep, reppers.Count());
            ViewData["FooterCum"] = footeracum; 

            //  Project _project = projservice.GetProposalInfo(id.Value);   //This will happen In MenuITEM Controller orgservice.GetOrganizationGeneralOfCurrentProposal(id.Value);

            return View(_bud2);
            
       }

        // GET: /ProposalInfo/View/1
        public ActionResult View(int? id, int? SelectedPeriodID)
        {
            

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }



            Budget _bud2 = projservice.GetBudgetTransactionByID(id.Value); 
        
            //  Project _project = projservice.GetProposalInfo(id.Value);   //This will happen In MenuITEM Controller orgservice.GetOrganizationGeneralOfCurrentProposal(id.Value);

            int BudgetID = id.Value;

            IEnumerable<ReportPeriodList> reppers = projservice.GetFinPeriods(BudgetID);

            SelectList selList = null; 
            if (reppers.Count() > 0)
            {
                selList = new SelectList(reppers, "ReportPeriodID", "PeriodTitle", reppers.First());
            }

            ViewData["PeriodList"] = selList; 
            ViewData["ProposalID"] = id.Value;

            //Set the selectedID.

            if (SelectedPeriodID.HasValue)
            {
                ViewData["SelectedPeriodID"] = SelectedPeriodID.Value;
            }
            else
            {
                if (reppers.Count() > 0)
                {
                    ViewData["SelectedPeriodID"] = reppers.First().ReportPeriodID;
                }
                else
                {
                    ViewData["SelectedPeriodID"] = 0;
                }
            }


            
            return View(_bud2);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(String PeriodListSel, Project proj, 
            List<FinArticleCategory> FinArticleCategories, 
            List<FinancialArticle> FinancialArticles,
            List<ReportPeriod> ReportPeriods,
            /*Project item,/*List<FinancialArticle> finarticle*//*, ICollection<Contact> Contacts*/ int? id, string actioncase)
        {

            if (!string.IsNullOrEmpty(PeriodListSel) && actioncase != "Update")
            {
                return RedirectToAction("View", new { id = id.Value, SelectedPeriodID = PeriodListSel });
            }


            /*
            foreach (var item in FinancialArticles)
            {
                for (int i = 0; i < FinArticleCategories.Count; i++)
                {
                    if (item.FinArticleCatID == FinArticleCategories[i].FinArticleCatID)
                    {
                        FinArticleCategories[i].FinancialArticles.Add(item);
                    }
                }
            }
            */

           
            
            switch (actioncase)
            {

                case "Search":
                 //   IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                 //   TempData["searchresult"] = orgs;
                    break;
                case "Update":

                    projservice.UpdateReportPeriodTrans(ReportPeriods); 
                    //Categories.Articles.Transactions.
                    /*
                    foreach (var cat in FinArticleCategories)
                    {
                        foreach (var article in FinancialArticles)
                        {
                            if (cat.FinArticleCatID == article.FinArticleCatID)
                            {
                                foreach (var transaction in FinTransactions)
                                {
                                    if (article.FinArticleID == transaction.FinArticleID)
                                    {
                                        article.ReportPeriods.Add(transaction); 
                                    }

                                }
                                
                                cat.FinancialArticles.Add(article);
                            }
                        }

                    }
                    
                    proj.Budget.FinArticleCategories = ConvertBOTA.ToEntitySet(FinArticleCategories.AsEnumerable());
                    projservice.UpdateProjectBudget(proj);  */
                    break;

                case "Delete":
                    // Delete organization GeneralInfo.
                    //  orgservice.DeleteOrganization(h_contactid);
                    break;

            }

            return RedirectToAction("View", new { id = id });

            /*  Organization _org = orgservice.GetOrganizationOfCurrentProposal(id.Value);
              if (_org != null)
              {
                  item.EntityKey = _org.EntityKey;
                
              } */

        }

    }
}
