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
    public class FilterController : BOTAController
    {
        //
        // GET: /Filter/

        public ActionResult Key(string fname)
        {
            ViewData["fname"] = fname;
            return View();
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Key(string fname, string lname)
        {
            ReportsRepository rr = new ReportsRepository();
            List<Project> Projects_Key = rr.Key_Assosiations(fname, lname);

            ViewData["result"] = Projects_Key;
            return Key(fname);
        }

        public ActionResult Index()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
            IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            ViewData["ProgramArea"] = ProgramArea;
            ViewData["ProposalStatus"] = ProposalStatus;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(int? AmountReqFrom, int? AmountReqTo, int? AwardedAmtFrom, int? AwardedAmtTo,
            string AcceptedDateFrom, string AcceptedDateTo, string StartDateFrom, string StartDateTo,
            string EndDateFrom, string EndDateTo, string CloseDateFrom, string CloseDateTo,
            string Country, string Area, string City, string Region, string Village, string Title,
            int? ProposalStatusID, int? ProgramAreaCodeID)
        {
            ReportsRepository rr = new ReportsRepository();
            List<Project> Projects_Key = rr.Projects(AmountReqFrom, AmountReqTo, AwardedAmtFrom, AwardedAmtTo,
             AcceptedDateFrom, AcceptedDateTo, StartDateFrom, StartDateTo, EndDateFrom, EndDateTo, CloseDateFrom,
             CloseDateTo, Country, Area, City, Region, Village, Title, ProposalStatusID, ProgramAreaCodeID);

            ViewData["result"] = Projects_Key;
            return Index();
        }

        public ActionResult Indicator()
        {
            IndicatorService InSer = new IndicatorService();
            IEnumerable<IndicatorCategoryLabel> ICL = InSer.GetIndicatorCategoryLabelsAll();            
            ViewData["ICL"] = ICL;
            IEnumerable<IndicatorLabel> IL = InSer.GetIndicatorLabelsAll();
            ViewData["IL"] = IL;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Indicator(int? basefrom, int? baseto, int? benchfrom, int? benchto,
            int? finalfrom, int? finalto, int? InCatLabelID, int? InLabelID)
        {
            ReportsRepository rr = new ReportsRepository();
            List<Project> Projects_Key = rr.Indicator(basefrom, baseto, benchfrom, benchto,
            finalfrom, finalto, InCatLabelID, InLabelID);

            ViewData["result"] = Projects_Key;
            return Indicator();
        }



        public ActionResult Event()
        {
            
            SSPStaffService ssp = new SSPStaffService();
            IEnumerable<SSPStaff> SSP = ssp.GetSSPStaffList();
            ViewData["SSP"] = SSP;
            EventTypeService ets = new EventTypeService();
            IEnumerable<EventType> etl = ets.GetEventTypeList();
            ViewData["ETL"] = etl;
            //IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            
            //ViewData["ProposalStatus"] = ProposalStatus;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Event(int? Status, string Desc, int? SSPorGrantee, int? Holder, string SchDateFrom,
            string SchDateTo, string CompDateFrom, string CompDateTo, int? TypeID, string CreDateFrom,
            string CreDateTo, string UpdDateFrom, string UpdDateTo, string FileName, string Author)
        {
            ReportsRepository rr = new ReportsRepository();
            List<Project> Projects_Key = rr.Events(Status, Desc, SSPorGrantee, Holder, SchDateFrom,
            SchDateTo, CompDateFrom, CompDateTo, TypeID, CreDateFrom, CreDateTo, UpdDateFrom, UpdDateTo,
            FileName, Author);

            ViewData["result"] = Projects_Key;
            return Event();
        }



        public ActionResult Organization()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            IEnumerable<LegalStatusList> ls = ServiceDDL.GetLegalStatusList();
            ViewData["ls"] = ls;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Organization(string Name, int? LegSListID, string LegalAddress, string Country,
            string Area, string City, string Region, string Village, string Email, string WebSite,
            string FirstName, string LastName, string ContactPerEmail,
            string DonorName, string ProjectName, int? AmountFrom, int? AmountTo,
            string FundedYearFrom, string FundedYearTo, string ContactPerson)
        {
            ReportsRepository rr = new ReportsRepository();
            List<Project> Projects_Key = rr.Organization(Name, LegSListID, LegalAddress, Country, Area, 
            City, Region, Village, Email, WebSite, FirstName, LastName, ContactPerEmail, DonorName, 
            ProjectName, AmountFrom, AmountTo, FundedYearFrom, FundedYearTo, ContactPerson);

            ViewData["result"] = Projects_Key;
            return Organization();
        }
    }
}
