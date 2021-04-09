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
    public class OutcomeReportController : Controller
    {
        //
        // GET: /OutcomeReport/

        public ActionResult Index()
        {
            return RedirectToAction("outcome");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult outcome()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
            IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            IEnumerable<GrantTypeList> GrantType = ServiceDDL.GetGrantTypeList();
            IEnumerable<CompetitionCodeList> CompletionCode = ServiceDDL.GetCompetitionCodeList();
            IEnumerable<ProposalStatusList> Status = ServiceDDL.GetProposalStatusList();
            IEnumerable<RegionList> Region = ServiceDDL.GetRegionList();
            IEnumerable<IndicatorLabelContentCategory> IndicatorCategoryList =
                ServiceDDL.IndicatorLabelContentCategoryList();
            ViewData["ProgramArea"] = ProgramArea;
            ViewData["ProposalStatus"] = ProposalStatus;
            ViewData["GrantType"] = GrantType;
            ViewData["CompletionCode"] = CompletionCode;
            ViewData["Status"] = Status;
            ViewData["Region"] = Region;
            ViewData["IndicatorCategoryList"] = IndicatorCategoryList;
            ViewData["results"] = null;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult outcome(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype,
                                       List<String> compete,
                                       List<String> status, List<String> oblast, List<String> period,
                                       List<String> amount, List<String> indicatorcategory)
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            #region drops
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
            IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            IEnumerable<GrantTypeList> GrantType = ServiceDDL.GetGrantTypeList();
            IEnumerable<CompetitionCodeList> CompletionCode = ServiceDDL.GetCompetitionCodeList();
            IEnumerable<ProposalStatusList> Status = ServiceDDL.GetProposalStatusList();
            IEnumerable<RegionList> Region = ServiceDDL.GetRegionList();
            IEnumerable<IndicatorLabelContentCategory> IndicatorCategoryList =
                ServiceDDL.IndicatorLabelContentCategoryList();
            ViewData["ProgramArea"] = ProgramArea;
            ViewData["ProposalStatus"] = ProposalStatus;
            ViewData["GrantType"] = GrantType;
            ViewData["CompletionCode"] = CompletionCode;
            ViewData["Status"] = Status;
            ViewData["Region"] = Region;
            ViewData["IndicatorCategoryList"] = IndicatorCategoryList;
            #endregion
            //if (frepf != null)
            //{
            LReportsRepository rep = new LReportsRepository();


            IQueryable<Project> prj = rep.GetResults3(frepf, ID, Area, gtype, compete, status, period, oblast, amount);
            //1. gets resulting project list after filtering.


            List<Project> prjList = prj.ToList();
            ViewData["prj"] = prj;



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
            //=====

            return View(frepf);

        }

    }
}
