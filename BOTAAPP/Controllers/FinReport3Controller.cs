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
    public class numberOfItems
    {
        public int numOfgrant;
        public int numOfvisits;
    }

    public class FirstVs
    {
        public int id;
        public string title;
    }


    public class FinReport3Controller : Controller
    {
        // GET: /FinReport2/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            return View();

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SiteVisitReport()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            ViewData["results"] = null;
            ViewData["Region"] = ServiceDDL.GetRegionList();

            return View();

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SiteVisitReport(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> period, List<String> amount, List<String> oblast)
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            ViewData["Region"] = ServiceDDL.GetRegionList();
            ViewData["BudgetCatList"] = ServiceDDL.GetCatList();

           
            LReportsRepository rep = new LReportsRepository();
            IQueryable<Project> prj = rep.GetResults3(frepf, ID, Area, gtype, compete, status, period, oblast, amount); //1. gets resulting project list after filtering.


            List<Project> prjList = prj.ToList();
            ViewData["prj"] = prj;

            

            if (prjList != null)
            {
                //2. generates/calculates VS amounts.
                //Dictionary<ContainerType, Dictionary<int, List<FinCatReport>>> results = FinReportCore(prj, frepf, Area, gtype, compete, status, period, oblast);
                //Dictionary<ContainerType, List<FinCatReport>> results = FinReportCore(prj, frepf, Area, gtype, compete, status, period, oblast);

                //ViewData["results2"] = results;
                var MView2 = SiteVisitReports(prjList); 
                ViewData["MView2"] = MView2; 
            }

            //add List Filters.  //enable Report View enabled if not null.
            if (Area != null)
            {
                frepf.IsArea = true;
            }
            if (gtype != null)
            {
                frepf.IsGrantType = true;
            }
            if (compete != null)
            {
                frepf.IsCompetitionCode = true;
            }
            if (status != null)
            {
                frepf.IsStatus = true;
            }
            if (period != null)
            {
                frepf.IsPeriod = true;
            }
            if (oblast != null)
            {
                frepf.isRegion = true;
            }
            //=====

            return View(frepf);

        }

       

        public Dictionary<int, List<VsContainer>> SiteVisitReports(IEnumerable<Project> prj)
        {
            // var View = new Dictionary<ProgramAreaID, Dictionary<RegionID, numberOfItems>>();
            var MView = new Dictionary<FirstVs, Dictionary<int, numberOfItems>>();
           // ProjectService prj = new ProjectService();
            ProjectEventService evnts = new ProjectEventService();
            AppDropDownsService appDropDown = new AppDropDownsService();

            int? id = 1;
            IEnumerable<ProjectEvent> projevnts = evnts.GetProjectEventList(id.Value);

            int sitevisits = 0;
            //Filter OUT NULL STUFF!
            IEnumerable<Project> projects = prj.Where(p => p.CompetitionCode != null && p.ProgramArea != null);
            
            var projSiteVisits = new Dictionary<Project, int>();
            var MView2 = new Dictionary<int, List<VsContainer>>();
            //==========1===========
            if (projects != null)
            {
                var query = projects
                    .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                    {
                        g.CompetitionCode.CompetCodeID,
                        g.ProgramArea.ProgramAreaCodeID
                    })
                    .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                    {
                        ProjId = group.Select(i => i.ProjectID).ToList(),
                        Field1 = group.Key.CompetCodeID.Value,
                        Field2 = group.Key.ProgramAreaCodeID.Value,
                        iAmount = group.Sum(c => c.ProjectEvents.Count(projevnt => projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2)),
                        dAmount = group.Select(i => i.ProjectID).Count(),
                        Field1Title = group.Select(i => i.CompetitionCode.CompetitionCodeList.CodeText).FirstOrDefault(),
                        Field2Title = group.Select(i => i.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault()
                    });

                List<VsContainer> vsc = query.ToList();   //test.              

                MView2.Add(1, vsc);
            }
            // var View = new Dictionary<ProgramAreaID, Dictionary<CompCodeID, numberOfItems>>();
            //======var MView = new Dictionary<FirstVs, Dictionary<int, numberOfItems>>();=========================


            //==========2===========
                                             //Filter OUT NULL STUFF!
            IEnumerable<Project> projects2 = prj.Where(p=>p.Organization !=null && p.Organization.Addresses!=null && p.Organization.Addresses.FirstOrDefault().Region != null && p.ProgramArea != null);
            if (projects != null)
            {
                var query = projects2
                    .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                    {
                        g.Organization.Addresses.FirstOrDefault().DDIDRegion,
                        g.ProgramArea.ProgramAreaCodeID
                    })
                    .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                    {
                        ProjId = group.Select(i => i.ProjectID).ToList(),
                        Field1 = group.Key.DDIDRegion.Value,
                        Field2 = group.Key.ProgramAreaCodeID.Value,
                        iAmount = group.Sum(c => c.ProjectEvents.Count(projevnt => projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2)),
                        dAmount = group.Select(i => i.ProjectID).Count(),
                        Field1Title = group.Select(i => i.Organization.Addresses.FirstOrDefault().RegionList.DDNAME).FirstOrDefault(),
                        Field2Title = group.Select(i => i.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault()
                    });

                List<VsContainer> vsc = query.ToList();   //test.

                MView2.Add(2, vsc);
            }
            //===============================

            return MView2; 
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult test()
        {
            // var View = new Dictionary<ProgramAreaID, Dictionary<RegionID, numberOfItems>>();
            var MView = new Dictionary<FirstVs, Dictionary<int, numberOfItems>>();
            ProjectService prj = new ProjectService();
            ProjectEventService evnts = new ProjectEventService();
            AppDropDownsService appDropDown = new AppDropDownsService();

            int? id = 1;
            IEnumerable<ProjectEvent> projevnts = evnts.GetProjectEventList(id.Value);

            int sitevisits = 0;
            IEnumerable<Project> projects = prj.getAllProjects().Where(p => p.CompetitionCode != null && p.ProgramArea != null);
            //int sitevisits = projevnts.Count(projevnt => projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2);
            //  if (projevnt.EventType.EventTypeName == "Site Visit" || projevnt.EventType.EventTypeName == "Site visit")
            // projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2
            var projSiteVisits = new Dictionary<Project, int>();
            var MView2 = new Dictionary<int, List<VsContainer>>(); 
            //==========1===========
            if (projects != null)
            {
                var query = projects
                    .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                    {
                        g.CompetitionCode.CompetCodeID,
                        g.ProgramArea.ProgramAreaCodeID
                    })
                    .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                    {
                        ProjId = group.Select(i => i.ProjectID).ToList(),                     
                        Field1 = group.Key.CompetCodeID.Value,
                        Field2 = group.Key.ProgramAreaCodeID.Value,                    
                        iAmount = group.Sum(c => c.ProjectEvents.Count(projevnt => projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2)),
                        dAmount = group.Select(i => i.ProjectID).Count(),
                        Field1Title = group.Select(i => i.CompetitionCode.CompetitionCodeList.CodeText).FirstOrDefault(),
                        Field2Title = group.Select(i => i.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault()
                    });

                List<VsContainer> vsc = query.ToList();   //test.              

                MView2.Add(1, vsc); 
            }
            // var View = new Dictionary<ProgramAreaID, Dictionary<CompCodeID, numberOfItems>>();
            //======var MView = new Dictionary<FirstVs, Dictionary<int, numberOfItems>>();=========================

         


            //==========2===========
            IEnumerable<Project> projects2 = prj.getAllProjects().Where(p => p.Organization.Addresses.FirstOrDefault().Region!=null && p.ProgramArea != null);
            if (projects != null)
            {
                var query = projects2
                    .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                    {
                        g.Organization.Addresses.FirstOrDefault().DDIDRegion,
                        g.ProgramArea.ProgramAreaCodeID
                    })
                    .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                    {
                        ProjId = group.Select(i => i.ProjectID).ToList(),
                        Field1 = group.Key.DDIDRegion.Value,
                        Field2 = group.Key.ProgramAreaCodeID.Value,
                        iAmount = group.Sum(c => c.ProjectEvents.Count(projevnt => projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2)),
                        dAmount = group.Select(i => i.ProjectID).Count(),
                        Field1Title = group.Select(i => i.Organization.Addresses.FirstOrDefault().RegionList.DDNAME).FirstOrDefault(),
                        Field2Title = group.Select(i => i.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault()
                    });

                List<VsContainer> vsc = query.ToList();   //test.

                MView2.Add(2, vsc); 
            }
            //===============================


            //foreach (var p in projects)
            //{
            //    sitevisits += p.ProjectEvents.Count(projevnt => projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2);
            //    projSiteVisits.Add(p, sitevisits);
            //    sitevisits = 0;
            //}


            //IEnumerable<RegionList> regions = appDropDown.GetRegionList();

            //foreach (RegionList reg in regions)
            //{

            //}

            //foreach (var v in View)
            //{
            //    foreach(var vl in v.Value)
            //    {
            //       int hello = vl.Value.numOfgrant; 
            //    }
            //}

            //AppDropDownsService ServiceDDL = new AppDropDownsService();
            //var SubView = new Dictionary<int, numberOfItems>();
            //var SubView2 = new Dictionary<int, numberOfItems>();
            //// ==============
            //var number1 = new numberOfItems();
            //number1.numOfgrant = 35;
            //number1.numOfvisits = 40;
            //SubView.Add(1, number1);

            //var number2 = new numberOfItems();
            //number2.numOfgrant = 15;
            //number2.numOfvisits = 10;

            //SubView.Add(4, number2);

            //var number3 = new numberOfItems();
            //number3.numOfgrant = 25;
            //number3.numOfvisits = 60;

            //SubView.Add(8, number3);


            //var number4 = new numberOfItems();
            //number4.numOfgrant = 125;
            //number4.numOfvisits = 60;

            //var number5 = new numberOfItems();
            //number5.numOfgrant = 125;
            //number5.numOfvisits = 60;

            //SubView2.Add(3, number4);
            //SubView2.Add(2, number5);

            //FirstVs fv = new FirstVs();
            //fv.id = 1;
            //fv.title = "ECD";
            //MView.Add(fv, SubView);

            //FirstVs fv2 = new FirstVs();
            //fv2.id = 2;
            //fv2.title = "Youth";
            //MView.Add(fv2, SubView2);

            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["MView"] = MView;
            ViewData["MView2"] = MView2;
            ViewData["Region"] = ServiceDDL.GetRegionList();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();

            return View();


        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FinReportFilter frepf)
        {
            if (frepf != null)
            {
                LReportsRepository rep = new LReportsRepository();
                IQueryable<Project> prj = rep.GetResults(frepf);

                if (prj != null)
                {
                    // ViewData["report31"] = rep.IndicatorByBaseline(prj);
                    ViewData["prj"] = prj;
                    ViewData["report1"] = rep.RequestedAndAwardedAmountByRegion(prj);
                    ViewData["report2"] = rep.RequestedAmountByArea(prj);
                    ViewData["report3"] = rep.RequestedAmountByGrantType(prj);
                    ViewData["report4"] = rep.AwardedAmountByArea(prj);
                    ViewData["report5"] = rep.AwardedAmountByGrantType(prj);

                }

                return View(frepf);
            }
            else
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BudgetAnalysis()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            ViewData["results"] = null;
            ViewData["Region"] = ServiceDDL.GetRegionList();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BudgetAnalysis(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> period, List<String> amount, List<String> oblast)
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            ViewData["Region"] = ServiceDDL.GetRegionList();
            ViewData["BudgetCatList"] = ServiceDDL.GetCatList();
            
            //if (frepf != null)
            //{
            LReportsRepository rep = new LReportsRepository();
            IQueryable<Project> prj = rep.GetResults3(frepf, ID, Area, gtype, compete, status, period, oblast, amount); //1. gets resulting project list after filtering.


            List<Project> prjList = prj.ToList();
            ViewData["prj"] = prj;

            //List<FinBudgetOneProjReport> tableResult = rep.GetBudgetOneProjectSum(prj);

           // ViewData["tableResult"] = tableResult;

            if (prjList != null)
            {
                //2. generates/calculates VS amounts.
                //Dictionary<ContainerType, Dictionary<int, List<FinCatReport>>> results = FinReportCore(prj, frepf, Area, gtype, compete, status, period, oblast);
                Dictionary<ContainerType, List<FinCatReport>> results = FinReportCore(prj, frepf, Area, gtype, compete, status, period, oblast);
                
                ViewData["results2"] = results;
            }

            //add List Filters.  //enable Report View enabled if not null.
            if (Area != null)
            {   
                frepf.IsArea = true;
            }
            if (gtype != null)
            {
                frepf.IsGrantType = true;
            }
            if (compete != null)
            {
                frepf.IsCompetitionCode = true;
            }
            if (status != null)
            {
                frepf.IsStatus = true;
            }
            if (period != null)
            {
                frepf.IsPeriod = true;
            }
            if (oblast != null)
            {
                frepf.isRegion = true;
            }
            //=====

            return View(frepf);

        }


        ////dispetcher, resends model to ReportView.
        //public ActionResult ResultContainerDisplay(List<List<VsContainer>> results)
        //{
        //    return PartialView("ReportViewControl", results);
        //}



        /// <summary>
        ///   ==Logic: 
        ///   1. identify which field required, using List of controls/filters passed, frepf.
        ///  2. call aggregator/grouper methods for identified fields.
        ///   3.  collect the results and return.
        /// </summary>
        /// <param name="prjList"></param>
        /// <param name="frepf"></param>
        /// <param name="Area"></param>
        /// <param name="gtype"></param>
        /// <param name="compete"></param>
        /// <param name="status"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<ContainerType, List<FinCatReport>> FinReportCore(IQueryable<Project> prjList, FinReportFilter frepf, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> period, List<String> region)
        {
         
           // var LLVsContainer = new Dictionary<int, List<FinCatReport>>();
            var LLVsContainer = new List<FinCatReport>();

            LReportsRepository rep = new LReportsRepository();
            List<FinCatReport> result = null;

          //  var allHolder = new Dictionary<ContainerType, Dictionary<int, List<FinCatReport>>>();  //Dictionary<areaID, List<CatID, SumTrans,SumBudget>
            var allHolder = new Dictionary<ContainerType, List<FinCatReport>>();  //Dictionary<areaID, List<CatID, SumTrans,SumBudget>

            if (Area != null && Area.Count > 0)
            {
              //  LLVsContainer.Clear();
               // foreach (string area in Area)
                {
                       result = rep.BudgetVsArea(prjList);
                       if (result != null && result.Any())
                           allHolder.Add(ContainerType.BudgetVsArea, result);
                          // LLVsContainer.Add(result);
                }
               // allHolder.Add(ContainerType.BudgetVsArea, LLVsContainer);
            }
            if (compete != null && compete.Count > 0)
            {
                result = rep.BudgetVsRound(prjList);
                if (result != null && result.Any())
                    allHolder.Add(ContainerType.BudgetVsRound, result);

            }
            if (status != null && status.Count > 0)
            {
                result = rep.BudgetVsStatus(prjList);
                if (result != null && result.Any())
                    allHolder.Add(ContainerType.BudgetVsStatus, result); ;
            }
            if (gtype != null && gtype.Count > 0)
            {
                result = rep.BudgetVsType(prjList);
                if (result != null && result.Any())
                    allHolder.Add(ContainerType.BudgetVsType, result);
            }


            return allHolder; 
        }

        
        private static Dictionary<int, List<FinCatReport>> FillContainer(IQueryable<Project> prjList, FinReportFilter frepf, ContainerType ct,
             Dictionary<int, List<FinCatReport>> LLVsContainer)
        {
            bool areaVsType = false;
       
            LReportsRepository rep = new LReportsRepository();

               List<FinCatReport> result = null;
                switch (ct)
                {
                    case ContainerType.BudgetVsArea: result = rep.BudgetVsArea(prjList);
                        if (result != null && result.Any())
                        LLVsContainer.Add(1, result);
                        break;
                    case ContainerType.BudgetVsRound: result = rep.BudgetVsRound(prjList);
                        if (result != null && result.Any())
                        LLVsContainer.Add(1, result);
                        break;
                    case ContainerType.BudgetVsStatus: result = rep.BudgetVsStatus(prjList);
                        if (result != null && result.Any())
                        LLVsContainer.Add(1, result);
                        break;
                    case ContainerType.BudgetVsType: result = rep.BudgetVsType(prjList);
                        if (result!=null && result.Any())
                        LLVsContainer.Add(1, result);
                        break; 
                }
               
         
            return LLVsContainer;
        }

      


     }
}
