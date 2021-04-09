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
    public class Filter2Controller : BOTAController
    {
        //
        // GET: /Filter2/

        public ActionResult Index()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
            IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            IEnumerable<GrantTypeList> GrantType = ServiceDDL.GetGrantTypeList();
            IEnumerable<CompetitionCodeList> CompletionCode = ServiceDDL.GetCompetitionCodeList();
            IEnumerable<ProposalStatusList> Status = ServiceDDL.GetProposalStatusList();
            IEnumerable<RegionList> Region = ServiceDDL.GetRegionList();
            IEnumerable<ProjLocationList> PrLoc = ServiceDDL.GetProjectLocationList();
            ViewData["ProgramArea"] = ProgramArea;
            ViewData["ProposalStatus"] = ProposalStatus;
            ViewData["GrantType"] = GrantType;
            ViewData["CompletionCode"] = CompletionCode;
            ViewData["Status"] = Status;
            ViewData["Region"] = Region;
            ViewData["ProjLoc"] = PrLoc;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(int? ID, List<String> Area, List<String> gtype, List<String> compete, 
            List<String> status, List<String> oblast, List<String> period, List<String> amount,
            List<String> projloc)
        {
            ReportsRepository rr = new ReportsRepository();
            IDataReader ResultTable = rr.Filter2(ID, Area, gtype, compete, status, oblast, period, amount, projloc);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            ds.Tables[0].Load(ResultTable);
            ViewData["DataSet"] = ds;             
            return Index();
        }        
    }
}
