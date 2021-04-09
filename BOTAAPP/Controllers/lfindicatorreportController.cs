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
    public class lfindicatorreportController : Controller
    {
        //
        // GET: /lfindicatorreport/

        public ActionResult Index()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            ViewData["results"] = null;
            ViewData["Region"] = ServiceDDL.GetRegionList();
            ViewData["LfIndicatorList"] = ServiceDDL.GetLfIndicatorList();
            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> period, List<String> amount, List<String> oblast, List<String> lfIndicators)
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            ViewData["Region"] = ServiceDDL.GetRegionList();
            ViewData["BudgetCatList"] = ServiceDDL.GetCatList();
            ViewData["LfIndicatorList"] = ServiceDDL.GetLfIndicatorList();

            LReportsRepository rep = new LReportsRepository();
            IQueryable<Project> prj = rep.GetResults3(frepf, ID, Area, gtype, compete, status, period, oblast, amount, lfIndicators); //1. gets resulting project list after filtering.


            List<Project> prjList = prj.ToList();
            ViewData["prj"] = prj;



            if (prjList != null)
            {
                var query = prjList
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                    {
                        g.LFIndicator.LFIndicatorID,                        
                    })
                    .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                    {
                        ProjId = group.Select(i => i.ProjectID).ToList(),
                        Field1 = group.Key.LFIndicatorID,                                          
                        iAmount = prjList.Count(),  
                        dAmount = group.Select(i => i.ProjectID).Count(), 
                        Field1Title = "# of grants with " + group.Select(i=>i.LFIndicator.LFIndicatorList.CodeText).FirstOrDefault()
                    });
               
                List<VsContainer> vsc = query.ToList();   //test. 
                ViewData["vsc"] = vsc; 
            }


            //add List Filters.  //enable Report View enabled if not null.
            if (lfIndicators != null)
            {
                frepf.isLFIndicator = true;
            }
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
    }
}
