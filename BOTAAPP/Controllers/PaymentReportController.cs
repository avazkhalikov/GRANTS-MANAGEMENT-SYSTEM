using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using System.Text;
using System.Data.Linq;


namespace BOTAMVC3.Controllers
{
    public class PaymentReportController : Controller
    {
        //
        // GET: /PaymentReport/

        ProjectService projservice;
        public PaymentReportController()
        {
            projservice = new ProjectService();
        
        }

        public ActionResult Index(int? id)
        {

            return RedirectToAction("Search", "ProposalInfo"); 
        }


        public ActionResult report(int? id)
        {

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }


            int BudgetID = id.Value;

            IEnumerable<ReportPeriodList> reppers = projservice.GetFinPeriods(BudgetID);

            Budget _bud2 = projservice.GetBudgetTransactionByID(id.Value);
            
            if(_bud2.ContractInitialAmt.HasValue)
            {

            //We aggregate the budget/transactions.
            FinanceAggregateService aggrv2 = new FinanceAggregateService(_bud2); 
           //AggregateViewHelper aggrv = new AggregateViewHelper(_bud2);
            List<CatRepViewHelper> catrep = aggrv2.GetView(reppers.Count());
            
            CatRepViewHelper footeracum = aggrv2.GetAcumulatedFooter(catrep, reppers.Count());

            //We generate payment reports from aggregated data.
            PaymentReportService paymentreport = new PaymentReportService(catrep, reppers, footeracum); 
           // PaymentReportHelper  paymentrephelper = new PaymentReportHelper(catrep, reppers);
            List<PaymentReport> paymentreports = paymentreport.GenerateReports(_bud2.ContractInitialAmt.Value);

            ViewData["PaymentReports"] = paymentreports;
            ViewData["ProposalID"] = id.Value; 
            
            }
           
            
            return View();

        }

    }
}
