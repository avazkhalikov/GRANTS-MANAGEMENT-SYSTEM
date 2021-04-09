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
namespace BOTAMVC3.Controllers
{
    public class ReportController : BOTAController
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();            
            IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();            
            ViewData["ProposalStatus"] = ProposalStatus;
            IEnumerable<GrantTypeList> GrantTypeList = ServiceDDL.GetGrantTypeList();
            ViewData["GrantTypeList"] = GrantTypeList;
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
            ViewData["ProgramArea"] = ProgramArea;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(int? PID, int? ProposalStatusID, string OrganizationName,
            int? AwardedAmtFrom, int? AwardedAmtTo, string AcceptedDateFrom, string AcceptedDateTo,
            int? GrantTypeCodeID, int? ProgramAreaCodeID)
        {
            ReportsRepository rr = new ReportsRepository();
            List<Project> Projects_Key = rr.Reports(PID, ProposalStatusID, OrganizationName, AwardedAmtFrom, 
            AwardedAmtTo, AcceptedDateFrom, AcceptedDateTo, GrantTypeCodeID, ProgramAreaCodeID);

            ViewData["result"] = Projects_Key;
            return Index();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ProjectFilter()
        {
           return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProjectFilter(List<String> from_select_list, List<String> db2_select_list)
        {
            return View();
        }

    }
}
