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
    #region Fin Report By Grant Type Class
    public class FinReportsByGrantTypeView
    {
        /*  Reppers:(
1st payment 	
Дата перечисления	
Сумма принятая по 1  отчету	
 
2nd payment	
Дата перечисления	
Сумма принятая по 2  отчету	
 *
3rd payment	
Дата перечисления	
Сумма принятая по 3  отчету	
 * 
4 rd payment	
Дата перечисления	
Сумма принятая по 4  отчету	
 * 
5 rd payment	
Дата перечисления ) */
             public List<BudgetPaymentReport> bpayrep;
             private ProposalInfo propInfo;
             private ProjectInfo projInfo; 
             public string ProjectID;
             public int DBID; 
             public string NameOfOrganization;
             public decimal InitialRequestedAmount;
             public decimal ApprovedamountbyExperts;
             public decimal SignedAmount; // (revised);
             public DateTime  Signdate;
             public DateTime  Startdate;	
             public DateTime  EndDate;


             public decimal TotalTransfered;//всего перечислено	
             public decimal TotalReported; //Сумма освоенных средств	
             public decimal TotalLeft; //Остаток неиспользованных средств гранта	
             public decimal Cancellation; //(к возврату в Фонд)
            // protected static readonly ILog log = LogManager.GetLogger(typeof(FinReportsByGrantTypeView));
            
          //for public. 
           public FinReportsByGrantTypeView(List<BudgetPaymentReport> _bpayrep, ProposalInfo _propInfo, ProjectInfo _projInfo)
           {
               bpayrep = _bpayrep;
               propInfo = _propInfo;
               projInfo = _projInfo;
               
           }

           //for internal instance create.
           private FinReportsByGrantTypeView()
           {              
         
           }

           public void InflateReport()
           {
                 ProjectID = propInfo.Project.Label;
                 DBID = propInfo.ProjectID; 
                 NameOfOrganization = propInfo.Project.Organization.General.NameRu;
                 //this.checked ? 'True' : 'False'"
                 InitialRequestedAmount = projInfo.AmtRequested.HasValue ? projInfo.AmtRequested.Value : 0;
                 //finrepview.ApprovedamountbyExperts = projInfo.AwardedAmt.Value; //??WTF!!
                 SignedAmount = projInfo.AwardedAmt.HasValue ? projInfo.AwardedAmt.Value : 0; // (revised);
                 Signdate = projInfo.AcceptedDate.HasValue ? projInfo.AcceptedDate.Value :  Convert.ToDateTime("1/1/1900");
                 Startdate = projInfo.StartDate.HasValue ? projInfo.StartDate.Value : Convert.ToDateTime("1/1/1900");
                 EndDate = projInfo.EndDate.HasValue ? projInfo.EndDate.Value : Convert.ToDateTime("1/1/1900");
        
                 TotalTransfered = GetTotalTransfered();//всего перечислено	
                 TotalReported = GetTotalReported(); //Сумма освоенных средств	
                 TotalLeft = GetTotalLeft(); //Остаток неиспользованных средств гранта	
                 Cancellation = GetCancellation(); //(к возврату в Фонд)

               
           }


           private decimal GetTotalTransfered()
           {
               decimal SumTransfered = 0; 

               //furich...
               foreach (BudgetPaymentReport bpr in bpayrep)
               {
                   SumTransfered = SumTransfered + bpr.PeriodGotMoney; 
               }

               return SumTransfered; 
           }

           private decimal GetTotalReported()
           {
               decimal SumSpent = 0;

               //furich...
               foreach (BudgetPaymentReport bpr in bpayrep)
               {
                   SumSpent = SumSpent + bpr.MoneySpent;
               }

               return SumSpent; 
           }

           private decimal GetTotalLeft()
           {
               if (projInfo.AwardedAmt.HasValue)
               {
                   return projInfo.AwardedAmt.Value - TotalReported;
               }
               else
               {
                   return 0;
               }
           }

           private decimal GetCancellation()
           {
               return TotalTransfered - TotalReported;
           } 
           
    }
    #endregion


    public class FinancialReportsController : Controller
    {

        BudgetService budservice;
        ProjectService projservice; 

        public FinancialReportsController()
        {
            budservice = new BudgetService();
            projservice = new ProjectService();
        
        }

        //
        // GET: /FinancialReports/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ReportByGrantTypeList(int? all)
        {    

            //Reports Generated Here...
            //report(id.Value); 
            //---------------------

            List<FinReportsByGrantTypeView> FinRepList = new List<FinReportsByGrantTypeView>(); 
            //getAllProject IDs where isDeleted not "true"
            IEnumerable<Project> ProjectIDs = null;
           
           // <%=ViewData["DisplayLabel"]%>   <%=ViewData["DisplayLink"]%> 
            if (all == 1)
            {
                 ProjectIDs = projservice.GetAllProjectIDs();
                 ViewData["DisplayLabel"] = "Grants Only";
                 ViewData["DisplayLink"] = "?all=0"; 
            }
            else
            {
                ProjectIDs = projservice.GetAllProjectIDs(true);
                ViewData["DisplayLabel"] = "All"; 
                ViewData["DisplayLink"] = "?all=1"; 
            }

            //Sorting:  ViewData["Results"] = propInfo.OrderByDescending(x =>
            //Convert.ToInt32(x.Project.Label.ToString().Split('-').ElementAt(2))).ToList(); 

            IEnumerable<Project> SortedProjectIDs = null;
            SortedProjectIDs = ProjectIDs.OrderByDescending(x => Convert.ToInt32(x.Label.ToString().Split('-').ElementAt(2))).ToList();

            foreach (Project id in SortedProjectIDs)
            {
                try
                {                 

                    ProposalInfo propInfo = projservice.GetProposalInfoOnly(id.ProjectID);
                    ProjectInfo projinfo = projservice.GetProjInfoOnly(id.ProjectID);                    
                    FinReportsByGrantTypeView finrepbygranttype = null;

                    if (projinfo != null && propInfo != null)
                    {
                        //(List<BudgetPaymentReport> _bpayrep, ProposalInfo _propInfo, ProjectInfo _projInfo)
                        //LEFT COLUMN. REPPERs
                        IEnumerable<ReportPeriodListR> reps = budservice.GetFinPeriods(id.ProjectID);
                        //RIGHT COLUMN PAYMENTS.
                        List<BudgetPaymentReport> _bpayrep = report(id.ProjectID);
                        finrepbygranttype = new FinReportsByGrantTypeView(_bpayrep, propInfo, projinfo);
                        finrepbygranttype.InflateReport();
                    }

                    if (projinfo != null && propInfo != null && finrepbygranttype != null)
                    {
                        FinRepList.Add(finrepbygranttype);
                    }
                }
                catch (Exception ex)
                {
                    ; //no adding.
                    //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
                  //  Log.EnsureInitialized();
                 //   Log.Error(typeof(ProjectRepository), "-------------------------------------------", ex);
                 //   return null;

                }

            }

            ViewData["FinRepList"] = FinRepList; 

            //   ViewData["PaymentReports"] = report(id.Value);



            return View();
        }



        // GET: /ProposalInfo/View/1
        //Social Service, Replication ...
        public ActionResult ReportByGrantType(int? id)
        {


            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            //Reports Generated Here...
            //report(id.Value); 
            //---------------------

            //LEFT COLUMN. REPPERs
            IEnumerable<ReportPeriodListR> reps = budservice.GetFinPeriods(id.Value);
                              
            //RIGHT COLUMN PAYMENTS.
            List<BudgetPaymentReport> _bpayrep = report(id.Value);
            ProposalInfo propInfo = projservice.GetProposalInfoOnly(id.Value); 
            ProjectInfo projinfo = projservice.GetProjInfoOnly(id.Value); 
         
            //(List<BudgetPaymentReport> _bpayrep, ProposalInfo _propInfo, ProjectInfo _projInfo)
            FinReportsByGrantTypeView finrepbygranttype = new FinReportsByGrantTypeView(_bpayrep, propInfo, projinfo);
            finrepbygranttype.InflateReport();
           
            ViewData["finrepbygranttype"] = finrepbygranttype; 

         //   ViewData["PaymentReports"] = report(id.Value);



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
    }
}
