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
    public class PreliminaryBudgetController : BOTAController
    {
      //
        // GET: /ProposalInfo/

        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo"); 
        }


        public ActionResult DeleteArticle(int? FinArticleID, int? BudgetID)
        {
           bool status = projservice.DeleteArticle(FinArticleID.Value);
           return RedirectToAction("View", new { id = BudgetID });
        }

        /// <summary>
        /// deletes category
        /// </summary>
        /// <param name="FinArticleCatID"></param>
        /// <param name="BudgetID"></param>
        /// <returns></returns>
        public ActionResult DeleteCat(int? FinArticleCatID, int? BudgetID)
        {
            //Needed FinArticleCatID, BudgetID

           bool deleted =  projservice.DeleteArticleCat(FinArticleCatID.Value, BudgetID.Value);

            return RedirectToAction("View", new { id = BudgetID });

        }

        public ActionResult Insert(int? FinArticleCatID, int? BudgetID)
        {
            //Needed FinArticleCatID, BudgetID

           projservice.InsertNewArticle(FinArticleCatID.Value, BudgetID.Value); 

           return RedirectToAction("View", new { id = BudgetID });
         
        }

        ProjectService projservice;
        public PreliminaryBudgetController()
        {
            projservice = new ProjectService();
        
        }

        // GET: /ProposalInfo/View/1
        public ActionResult View(int? id)
        {
            

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            
         //   Project _project = projservice.GetProposalInfo(id.Value);   //This will happen In MenuITEM Controller orgservice.GetOrganizationGeneralOfCurrentProposal(id.Value);
                        
        //    Budget _budget = projservice.GetBudgetBYID(id.Value);  //LINQ ONE.
              
              Budget _bud2 = projservice.GetBudgetTransactionByID(id.Value); 

              ViewData["BudgetCatList"] = new SelectList(projservice.GetCatList(), "FinArticleCatID", "FinArticleCatName");
            
             //   ViewData["ProgramAreaList"] = new SelectList(projservice.GetProgramAreaList(), "ProgramAreaCodeID", "ProgramAreaText");
         
            
              ViewData["ProposalID"] = id.Value;



              return View(_bud2);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(Project proj, List<FinArticleCategory> FinArticleCategories, 
                                      List<FinancialArticle> FinancialArticles,
                                     /*Project item,/*List<FinancialArticle> finarticle*//*, ICollection<Contact> Contacts*/
                                     int? id, string actioncase, string FinCatInfo, int? FinCatSel, string NewCatName)
        {
            
           


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

         //   proj.Budget.FinArticleCategories = ConvertBOTA.ToEntitySet(FinArticleCategories.AsEnumerable());
            
            switch (actioncase)
            {

                case "Create Category":
                    projservice.InsertNewCatName(NewCatName);
                    break;
                case "Add Category":
                    projservice.AddFinArticleCategory(id.Value, FinCatSel.Value); 
                    break;
                case "Search":
                 //   IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                 //   TempData["searchresult"] = orgs;
                    break;
                case "Update":
                   
                    foreach (var cat in FinArticleCategories)
                    {
                        foreach (var article in FinancialArticles)
                        {
                            if (cat.FinArticleCatID == article.FinArticleCatID)
                            {
                                if (!article.DonorInput.HasValue)
                                {
                                    article.DonorInput = 0; 
                                }
                                if (!article.GranteeInput.HasValue)
                                {
                                    article.GranteeInput = 0; 
                                } 
                                if (article.Info == null)
                                {
                                    article.Info = "no info"; 
                                }

                                cat.FinancialArticles.Add(article);
                            }
                        }

                    }

                    Budget bud = new Budget();
                    bud.BudgetID = id.Value; 
                    bud.FinArticleCategories = ConvertBOTA.ToEntitySet(FinArticleCategories.AsEnumerable());
                    projservice.UpdateProjectBudget(bud); 
                                        
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
