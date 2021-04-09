using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.DataAccess.Impl;

namespace BOTAMVC3.Controllers
{
    public class PopulateFinArtwithCatController : Controller
    {
        //
        // GET: /PopulateFinArtwithCat/

        public ActionResult Index()
        {


            LReportsRepository rr = new LReportsRepository();

            rr.AggregateBudgeByCat(); 

           // Step1: rr.PopulateFinCatIDStrict();
           //  Step2: rr.PopulateFinCatIDNonStrict(); 

          /* after 2 Populations, execute EDIT where 0 and fix some of them by HAND! about 60 of them. cancellation/refund shit!!
           * SELECT     TOP (200) FinArticleCatID, BudgetID, FinArticleCatText, Price, TransferAmt, FinCatID
            FROM         FinArticleCategoryR
            WHERE     (FinCatID = 0)
            ORDER BY FinArticleCatText */

            return View();
        }

    }
}
