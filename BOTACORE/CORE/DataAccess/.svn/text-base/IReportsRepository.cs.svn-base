using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
namespace BOTACORE.CORE.DataAccess
{
    public interface IReportsRepository
    {
        List<Project> Reports(int? PID, int? ProposalStatusID, string OrganizationName,
        int? AwardedAmtFrom, int? AwardedAmtTo, string AcceptedDateFrom, string AcceptedDateTo,
        int? GrantTypeCodeID, int? ProgramAreaCodeID);

        List<Project> Projects(int? AmountReqFrom, int? AmountReqTo, int? AwardedAmtFrom, int? AwardedAmtTo,
        string AcceptedDateFrom, string AcceptedDateTo, string StartDateFrom, string StartDateTo,
        string EndDateFrom, string EndDateTo, string CloseDateFrom, string CloseDateTo,
        string Country, string Area, string City, string Region, string Village, string Title,
        int? ProposalStatusID, int? ProgramAreaCodeID);

        List<Project> Organization(string Name, int? LegSListID, string LegalAddress, string Country,
        string Area, string City, string Region, string Village, string Email, string WebSite,
        string FirstName, string LastName, string ContactPerEmail,
        string DonorName, string ProjectName, int? AmountFrom, int? AmountTo,
        string FundedYearFrom, string FundedYearTo, string ContactPerson);

        List<Project> Indicator(int? basefrom, int? baseto, int? benchfrom, int? benchto,
        int? finalfrom, int? finalto, int? InCatLabelID, int? InLabelID);

        List<Project> Events(int? Status, string Desc, int? SSPorGrantee, int? Holder, string SchDateFrom,
        string SchDateTo, string CompDateFrom, string CompDateTo, int? TypeID, string CreDateFrom,
        string CreDateTo, string UpdDateFrom, string UpdDateTo, string FileName, string Author);

        List<Project> Key_Assosiations(string fname, string lname);

        List<Project> Filter2(int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> oblast, List<String> period, List<String> amount,
            List<String> location);
    }
}
