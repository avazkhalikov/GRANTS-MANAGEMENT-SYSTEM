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
using BOTACORE.CORE.DataAccess.Impl;
using System.Data;


namespace BOTAMVC3.Controllers
{
    public class DeliveryReportController : Controller
    {
        public ActionResult UiTableTest()
        {
            return View(); 
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult index()
        {
            FinReportFilter frepf = new FinReportFilter();
            frepf.IsAllTransfered = true;
            frepf.IsAwardedAmount = true;
            frepf.isProjectName = false;
            frepf.isOrganizationName = true;
            frepf.IsRefund = true;
            frepf.IsUsedAmount = true;
            frepf.IsStatus = true;
            frepf.IsCancellation = true;
            frepf.IsGrantType = true;
            frepf.IsCompetitionCode = true;
            frepf.IsArea = true;
            frepf.IsCashOnHand = true;

            //Status is fixed for all Grants.
            List<string> status = new List<string>();
            status.Add("4"); //closed 
            status.Add("6"); //terminated.

            LReportsRepository rep = new LReportsRepository();
            IQueryable<Project> prj = rep.GetResults3(frepf, null, null, null, null, status, null, null, null); //1. gets resulting project list after filtering.

            List<Project> prjList = prj.ToList();
            ViewData["prj"] = prj;


                      
            return View(frepf);
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult partial(FinReportFilter frepf, List<String> Area, List<String> gtype, List<String> compete, int? id)
        //{            
        //    //Status is fixed for all Grants.
        //    List<string> status = new List<string>();
        //    status.Add("4"); //closed 
        //    status.Add("6"); //terminated.

        //    LReportsRepository rep = new LReportsRepository();
        //    IQueryable<Project> prj = rep.GetResults3(frepf, null, null, null, null, status, null, null, null); //1. gets resulting project list after filtering.

        //    List<Project> prjList = prj.ToList();
        //    ViewData["prj"] = prj;

        //    var dType = new Dictionary<AmountTypes, IEnumerable<DRContainer>>();

        //    if (id.Value == 1) //first reports
        //    {
        //        IEnumerable<DRContainer> DRModel = GeneratePartialReports(prj, AmountTypes.Refund);
        //        dType.Add(AmountTypes.Refund, DRModel);
        //        DRModel = GeneratePartialReports(prj, AmountTypes.AllTransfered);
        //        dType.Add(AmountTypes.AllTransfered, DRModel);
        //        DRModel = GeneratePartialReports(prj, AmountTypes.CashOnHand);
        //        dType.Add(AmountTypes.CashOnHand, DRModel);
        //        DRModel = GeneratePartialReports(prj, AmountTypes.UsedAmount);
        //        dType.Add(AmountTypes.UsedAmount, DRModel);
        //        DRModel = GeneratePartialReports(prj, AmountTypes.Cancellation);
        //        dType.Add(AmountTypes.Cancellation, DRModel);
        //    }

        //    ViewData["dType"] = dType; 
        //    return  PartialView(dType);
        //}


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult partial2(FinReportFilter frepf, List<String> Area, List<String> gtype, List<String> compete, int? id)
        {
            //Status is fixed for all Grants.
            List<string> status = new List<string>();
            status.Add("4"); //closed 
            status.Add("6"); //terminated.

            LReportsRepository rep = new LReportsRepository();
            IQueryable<Project> prj = rep.GetResults3(frepf, null, null, null, null, status, null, null, null); //1. gets resulting project list after filtering.

            List<Project> prjList = prj.ToList();
            ViewData["prj"] = prj;

            var dType = new Dictionary<AmountTypes, IEnumerable<DRContainer>>();
            if (id.HasValue && id.Value == 1)
            {
                IEnumerable<DRContainer> DRModel = GeneratePartialReports(prj, AmountTypes.Refund);
                dType.Add(AmountTypes.Refund, DRModel);
                DRModel = GeneratePartialReports(prj, AmountTypes.AllTransfered);
                dType.Add(AmountTypes.AllTransfered, DRModel);
                DRModel = GeneratePartialReports(prj, AmountTypes.CashOnHand);
                dType.Add(AmountTypes.CashOnHand, DRModel);
                DRModel = GeneratePartialReports(prj, AmountTypes.UsedAmount);
                dType.Add(AmountTypes.UsedAmount, DRModel);
                DRModel = GeneratePartialReports(prj, AmountTypes.Cancellation);
                dType.Add(AmountTypes.Cancellation, DRModel);
                ViewData["dType"] = dType;
                ViewData["Base"] = "Awarded"; 
                ViewData["RepType"] = "Reports By Round"; 
            }
            if (id.HasValue && id.Value == 2)
            {
                IEnumerable<DRContainer> DRModel = GeneratePartialReportsByAreaAwrd(prj, AmountTypes.Refund);
                dType.Add(AmountTypes.Refund, DRModel);
                DRModel = GeneratePartialReportsByAreaAwrd(prj, AmountTypes.AllTransfered);
                dType.Add(AmountTypes.AllTransfered, DRModel);
                DRModel = GeneratePartialReportsByAreaAwrd(prj, AmountTypes.CashOnHand);
                dType.Add(AmountTypes.CashOnHand, DRModel);
                DRModel = GeneratePartialReportsByAreaAwrd(prj, AmountTypes.UsedAmount);
                dType.Add(AmountTypes.UsedAmount, DRModel);
                DRModel = GeneratePartialReportsByAreaAwrd(prj, AmountTypes.Cancellation);
                dType.Add(AmountTypes.Cancellation, DRModel);
                ViewData["dType"] = dType;
                ViewData["Base"] = "Awarded";
                ViewData["RepType"] = "Reports By Area";
            }
            if (id.HasValue && id.Value == 3)
            {
                IEnumerable<DRContainer> DRModel = GeneratePartialReportsByTypeAwrd(prj, AmountTypes.Refund);
                dType.Add(AmountTypes.Refund, DRModel);
                DRModel = GeneratePartialReportsByTypeAwrd(prj, AmountTypes.AllTransfered);
                dType.Add(AmountTypes.AllTransfered, DRModel);
                DRModel = GeneratePartialReportsByTypeAwrd(prj, AmountTypes.CashOnHand);
                dType.Add(AmountTypes.CashOnHand, DRModel);
                DRModel = GeneratePartialReportsByTypeAwrd(prj, AmountTypes.UsedAmount);
                dType.Add(AmountTypes.UsedAmount, DRModel);
                DRModel = GeneratePartialReportsByTypeAwrd(prj, AmountTypes.Cancellation);
                dType.Add(AmountTypes.Cancellation, DRModel);
                ViewData["dType"] = dType;
                ViewData["Base"] = "Awarded";
                ViewData["RepType"] = "Reports By Type";
            }          
            if (id.HasValue && id.Value == 4)
            {
                IEnumerable<DRContainer> DRModel = GeneratePartialReportsByRoundUsed(prj, AmountTypes.AllTransfered);
                dType.Add(AmountTypes.AllTransfered, DRModel);
                ViewData["dType"] = dType;
                ViewData["Base"] = "Used";
                ViewData["RepType"] = "Reports By Round(Transfered/Used)";
            }
            if (id.HasValue && id.Value == 5)
            {
                IEnumerable<DRContainer> DRModel = GeneratePartialReportsByAreaUsed(prj, AmountTypes.AllTransfered);
                dType.Add(AmountTypes.AllTransfered, DRModel);
                ViewData["dType"] = dType;
                ViewData["Base"] = "Used";
                ViewData["RepType"] = "Reports By Area(Transfered/Used)";
            }
            if (id.HasValue && id.Value == 6)
            {
                IEnumerable<DRContainer> DRModel = GeneratePartialReportsByTypeUsed(prj, AmountTypes.AllTransfered);
                dType.Add(AmountTypes.AllTransfered, DRModel);
                ViewData["dType"] = dType;
                ViewData["Base"] = "Used";
                ViewData["RepType"] = "Reports By Type(Transfered/Used)";
            }
            return PartialView();
        }

        public IEnumerable<DRContainer> GeneratePartialReportsByRoundUsed(IQueryable<Project> prj, AmountTypes at)
        {
            var query = prj
              .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.CompetitionCode.CompetitionCodeList.CodeText,
              })
              .Select(group => new DRContainer()     //Select all Grouped into VersusContainer.
              {
                  Name = group.Key.CodeText,
                  GrantNumber = group.Count(),
                  Amt1 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), at),
                  Amt2 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), AmountTypes.UsedAmount)
              });

            return query;
        }
        public IEnumerable<DRContainer> GeneratePartialReportsByTypeUsed(IQueryable<Project> prj, AmountTypes at)
        {
            var query = prj
              .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.GrantType.GrantTypeList.GrantTypeText
              })
              .Select(group => new DRContainer()     //Select all Grouped into VersusContainer.
              {
                  Name = group.Key.GrantTypeText,
                  GrantNumber = group.Count(),
                  Amt1 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), at),
                  Amt2 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), AmountTypes.UsedAmount)
              });

            return query;
        }
        public IEnumerable<DRContainer> GeneratePartialReportsByAreaUsed(IQueryable<Project> prj, AmountTypes at)
        {
            var query = prj
              .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.ProgramArea.ProgramAreaList.ProgramAreaText
              })
              .Select(group => new DRContainer()     //Select all Grouped into VersusContainer.
              {
                  Name = group.Key.ProgramAreaText,
                  GrantNumber = group.Count(),
                  Amt1 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), at),
                  Amt2 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), AmountTypes.UsedAmount)
              });

            return query;
        }

        public IEnumerable<DRContainer> GeneratePartialReports(IQueryable<Project> prj, AmountTypes at)
        {
            var query = prj
              .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.CompetitionCode.CompetitionCodeList.CodeText,
              })
              .Select(group => new DRContainer()     //Select all Grouped into VersusContainer.
              {
                  Name = group.Key.CodeText,
                  GrantNumber = group.Count(),
                  Amt1 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), at),
                  Amt2 = group.Sum(i => i.ProjectInfo.AwardedAmt)

              });                


            return query;
        }

        public IEnumerable<DRContainer> GeneratePartialReportsByAreaAwrd(IQueryable<Project> prj, AmountTypes at)
        {
            var query = prj
              .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.ProgramArea.ProgramAreaList.ProgramAreaText
              })
              .Select(group => new DRContainer()     //Select all Grouped into VersusContainer.
              {
                  Name = group.Key.ProgramAreaText,
                  GrantNumber = group.Count(),
                  Amt1 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), at),
                  Amt2 = group.Sum(i => i.ProjectInfo.AwardedAmt)

              });


            return query;
        }

        public IEnumerable<DRContainer> GeneratePartialReportsByTypeAwrd(IQueryable<Project> prj, AmountTypes at)
        {
            var query = prj
              .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.GrantType.GrantTypeList.GrantTypeText
              })
              .Select(group => new DRContainer()     //Select all Grouped into VersusContainer.
              {
                  Name = group.Key.GrantTypeText,
                  GrantNumber = group.Count(),
                  Amt1 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), at),
                  Amt2 = group.Sum(i => i.ProjectInfo.AwardedAmt)

              });


            return query;
        }

        public List<DRContainer> GenerateReports(IQueryable<Project> prj)
        {
            var query = prj
              .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.CompetitionCode.CompetitionCodeList.CodeText,
              })
              .Select(group => new DRContainer()     //Select all Grouped into VersusContainer.
              {
                  Name = group.Key.CodeText,
                  GrantNumber = group.Count(),
                  Amt1 = TotalFinAmount(group.Select(i => i.ProjectID).ToList(), AmountTypes.Refund),
                  Amt2 =  group.Sum(i=>i.ProjectInfo.AwardedAmt) 

              });


            var list = query.ToList();  //got First RESULT HERE!!!!!

           // The following var results must be re-organizaed andreturned.

            var results = new Dictionary<DRTYPE, Dictionary<DRAmountType, List<DRContainer>>>();
            var dic = new Dictionary<DRAmountType, List<DRContainer>>(); 
            if (query.Any())
            {
                dic.Add(DRAmountType.RefundVSAwardedAmount, query.ToList());
                results.Add(DRTYPE.Round, dic); 
            }              
            

            return list; 
        }


        public decimal TotalFinAmount(List<int> Projs, AmountTypes type)
        {
            FinanceResults finres = new FinanceResults();
            BudgetService budservice = new BudgetService();
            decimal Sum = 0; 
            foreach (var prjId in Projs)
            {
                finres.Initialize(prjId);
                decimal amt = 0;

                if (type == AmountTypes.AllTransfered)
                {
                    amt = finres.Project_TotalMoneyTransferedFromAwardAmount();
                }
                if (type == AmountTypes.UsedAmount)
                {
                    amt = finres.Project_TotalMoneySpentAmountFromAwardAmount();
                }
                if (type == AmountTypes.UnusedAmount)
                {
                    amt = finres.Project_TotalAmountLeftFromAwardAmount();
                }
                if (type == AmountTypes.CashOnHand)
                {
                    amt = finres.Project_TotalCashOnHand();
                }
                if (type == AmountTypes.Refund)
                {
                    //TODO Check with : =========
                    //Alya, what Returned she wants to USE! SEE IF Reports periods one is OKAY> BELOW
                    //   ViewData["Refund"] = SumTransfered - SumSpent - b.Returned;  // . Awarded Amt - total paid amt
                    Budget b = budservice.GetBudget(prjId);
                    if (b.Returned != null) amt = b.Returned.Value;
                }
                if (type == AmountTypes.Cancellation)
                {
                    // ViewData["Cancellation"] = ProjInfo.AwardedAmt.Value - b.Cancellation - SumTransfered;  // . Awarded Amt - total paid amt
                    Budget b = budservice.GetBudget(prjId);
                    if (b.Cancellation != null) amt = b.Cancellation.Value;
                }


                Sum = Sum + amt;
            }

            return Sum; 
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult index(FinReportFilter frepf, List<String> Area, List<String> gtype, List<String> compete)
        //{
            //AppDropDownsService ServiceDDL = new AppDropDownsService();
            //ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            //ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            //ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            //ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            //ViewData["Region"] = ServiceDDL.GetRegionList();


            ////Status is fixed for all Grants.
            //List<string> status = new List<string>();
            //status.Add("4"); //closed 
            //status.Add("6"); //terminated.

            //LReportsRepository rep = new LReportsRepository();
            //IQueryable<Project> prj = rep.GetResults3(frepf, null, null, null, null, status, null, null, null); //1. gets resulting project list after filtering.

            //List<Project> prjList = prj.ToList();
            //ViewData["prj"] = prj;

            //ViewData["test1"] = GenerateReports(prj); 

            //if (prjList != null)
            //{
            //    //2. generates/calculates VS amounts.
            //   var results = new Dictionary<ContainerType, Dictionary<DRAmountType, List<DRContainer>>>();
            //    ViewData["results2"] = results;
            //}            


        //    return View(frepf);

        //}

    }
}
