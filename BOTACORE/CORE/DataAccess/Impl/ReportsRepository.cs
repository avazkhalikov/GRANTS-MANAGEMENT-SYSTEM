using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;
using System.Data;
using StructureMap;
using BOTACORE.CORE;
using BOTACORE.CORE.DataAccess.DAL;
namespace BOTACORE.CORE.DataAccess.Impl
{
    public class ReportsRepository
    {
        private string connectString;
        private ISqlCommands sqlcmd;

        public ReportsRepository()
        {
            Connection conn = new Connection();
            connectString = conn.GetDirectConnString();
            sqlcmd = SqlFactory.MSSQL();  
        }
        public List<Project> Key_Assosiations(string fname, string lname)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM vProjects vp WHERE vp.ProjectID in (");
            sb.Append("SELECT p.ProjectID FROM Projects p ");
            sb.Append("LEFT JOIN SSPStaffProjects sspsp on p.ProjectID = sspsp.ProjectID ");
            sb.Append("LEFT JOIN SSPStaff s ON sspsp.SSPStaffID = s.SSPStaffID ");
            sb.Append("WHERE p.ProjectID IS NOT NULL ");

            if (!String.IsNullOrEmpty(fname))
            {
                sb.Append("AND s.FirstName LIKE '%" + fname + "%' ");
            }
            if (!String.IsNullOrEmpty(lname))
            {
                sb.Append("AND s.LastName LIKE '%" + lname + "%' ");
            }
            sb.Append(" ) ");
            IDataReader dr = sqlcmd.GetProjectsForFilter(sb.ToString());            

            List<Project> Projects = LoadProjects(dr);

            return Projects;
        }

        public List<Project> Events(int? Status, string Desc, int? SSPorGrantee, int? Holder, string SchDateFrom,
            string SchDateTo, string CompDateFrom, string CompDateTo, int? TypeID, string CreDateFrom,
            string CreDateTo, string UpdDateFrom, string UpdDateTo, string FileName, string Author)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM vProjects vp WHERE vp.ProjectID in (");
            sb.Append("SELECT p.ProjectID FROM Projects p ");
            sb.Append("LEFT JOIN ProjectEvent pe ON p.ProjectID = pe.ProjectID ");
            sb.Append("LEFT JOIN EventType et ON pe.EventTypeID = et.EventTypeID ");
            sb.Append("LEFT JOIN ProjectEventDocs peds ON peds.EventID = pe.EventID ");
            sb.Append("LEFT JOIN ProjectEventDocument ped ON peds.ProjectEventDocumentID = ped.ProjectEventDocumentID ");
            sb.Append("WHERE p.ProjectID IS NOT NULL ");
            if (Status.HasValue)
            {
                sb.Append("AND pe.EventStatus = " + Status.Value.ToString() + " ");
            }
            if (!String.IsNullOrEmpty(Desc))
            {
                sb.Append("AND pe.EventDescription LIKE '%" + Desc + "%' ");
            }
            if (SSPorGrantee.HasValue)
            {
                sb.Append("AND pe.SSPOrGrantee = " + SSPorGrantee.Value.ToString() + " ");
            }
            if (Holder.HasValue)
            {
                sb.Append("AND pe.EventHolderID = " + Holder.Value.ToString() + " ");
            }
            if (!String.IsNullOrEmpty(SchDateFrom))
            {
                sb.Append("AND pe.ScheduledDate >= CONVERT(Datetime, '" + SchDateFrom + "', 103) ");
            }
            if (!String.IsNullOrEmpty(SchDateTo))
            {
                sb.Append("AND pe.ScheduledDate <= CONVERT(Datetime, '" + SchDateTo + "', 103) ");
            }
            if (!String.IsNullOrEmpty(CompDateFrom))
            {
                sb.Append("AND pe.CompletedDate >= CONVERT(Datetime, '" + CompDateFrom + "', 103) ");
            }
            if (!String.IsNullOrEmpty(CompDateTo))
            {
                sb.Append("AND pe.CompletedDate <= CONVERT(Datetime, '" + CompDateTo + "', 103) ");
            }
            if (TypeID.HasValue)
            {
                sb.Append("AND et.EventTypeID = " + TypeID.Value.ToString() + " ");
            }
            if (!String.IsNullOrEmpty(CreDateFrom))
            {
                sb.Append("AND ped.CreatedDate >= CONVERT(Datetime, '" + CreDateFrom + "', 103) ");
            }
            if (!String.IsNullOrEmpty(CreDateTo))
            {
                sb.Append("AND ped.CreatedDate <= CONVERT(Datetime, '" + CreDateTo + "', 103) ");
            }
            if (!String.IsNullOrEmpty(UpdDateFrom))
            {
                sb.Append("AND ped.UpdatedDate >= CONVERT(Datetime, '" + UpdDateFrom + "', 103) ");
            }
            if (!String.IsNullOrEmpty(UpdDateTo))
            {
                sb.Append("AND ped.UpdatedDate <= CONVERT(Datetime, '" + UpdDateTo + "', 103) ");
            }
            if (!String.IsNullOrEmpty(FileName))
            {
                sb.Append("AND ped.[FileName] LIKE '%" + FileName + "%' ");
            }
            if (!String.IsNullOrEmpty(Author))
            {
                sb.Append("AND ped.Author LIKE '%" + Author + "%' ");
            }
            sb.Append(" ) ");
            IDataReader dr = sqlcmd.GetProjectsForFilter(sb.ToString());

            List<Project> Projects = LoadProjects(dr);

            return Projects;
        }

        public List<Project> Indicator(int? basefrom, int? baseto, int? benchfrom, int? benchto,
            int? finalfrom, int? finalto, int? InCatLabelID, int? InLabelID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM vProjects vp WHERE vp.ProjectID in (");
            sb.Append("SELECT p.ProjectID FROM Projects p ");
            sb.Append("LEFT JOIN Indicator i ON p.ProjectID = i.IndicatorID ");
            sb.Append("LEFT JOIN IndicatorCategoryLabel icl ON i.IndicatorID = icl.IndicatorID ");
            sb.Append("LEFT JOIN IndicatorLabel il ON i.IndicatorID = il.IndicatorID ");
            sb.Append("LEFT JOIN IndicatorItem ii ON icl.IndicatorCategoryLabelID = ii.IndicatorCategoryLabelID AND ii.IndicatorLabelID = il.IndicatorLabelID ");
            sb.Append("WHERE p.ProjectID <> -1 ");
            if (baseto.HasValue)
            {
                sb.Append("AND ii.Baseline <= " + baseto.Value.ToString() + " ");
            }
            if (basefrom.HasValue)
            {
                sb.Append("AND ii.Baseline >= " + basefrom.Value.ToString() + " ");
            }
            if (benchto.HasValue)
            {
                sb.Append("AND ii.Benchmark <= " + benchto.Value.ToString() + " ");
            }
            if (benchfrom.HasValue)
            {
                sb.Append("AND ii.Benchmark >= " + benchfrom.Value.ToString() + " ");
            }
            if (finalto.HasValue)
            {
                sb.Append("AND ii.Final <= " + finalto.Value.ToString() + " ");
            }
            if (finalfrom.HasValue)
            {
                sb.Append("AND ii.Final >= " + finalfrom.Value.ToString() + " ");
            }
            if (InCatLabelID.HasValue)
            {
                sb.Append("AND icl.IndicatorCategoryLabelID = " + InCatLabelID.Value.ToString() + " ");
            }
            if (InLabelID.HasValue)
            {
                sb.Append("AND il.IndicatorLabelID = " + InLabelID.Value.ToString() + " ");
            }
            sb.Append(" ) ");
            IDataReader dr = sqlcmd.GetProjectsForFilter(sb.ToString());
            
            List<Project> Projects = LoadProjects(dr);

            return Projects;
        }

        public List<Project> Organization(string Name, int? LegSListID, string LegalAddress, string Country,
            string Area, string City, string Region, string Village, string Email, string WebSite,
            string FirstName, string LastName, string ContactPerEmail,
            string DonorName, string ProjectName, int? AmountFrom, int? AmountTo,
            string FundedYearFrom, string FundedYearTo, string ContactPerson)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM vProjects vp WHERE vp.ProjectID in (");
            sb.Append("SELECT p.ProjectID FROM Projects p ");
            sb.Append("LEFT JOIN Organization o ON p.OrgID = p.OrgID ");
            sb.Append("LEFT JOIN Address a ON a.OrgID = o.OrgID ");
            sb.Append("LEFT JOIN Contact c ON c.OrgID = o.OrgID ");
            sb.Append("LEFT JOIN OtherFunders [of] ON [of].OrgID = o.OrgID ");
            sb.Append("LEFT JOIN LegalStatus ls ON ls.OrgID = o.OrgID ");
            sb.Append("LEFT JOIN General g ON g.OrgID = o.OrgID ");
            sb.Append("WHERE o.OrgID IS NOT NULL ");

            if (!String.IsNullOrEmpty(Name))
            {
                sb.Append("AND (g.[Name] LIKE '%" + Name + "%' OR g.[NameRu] LIKE '%" + Name + "%') ");
            }
            if (LegSListID.HasValue)
            {
                sb.Append("AND ls.LegSListID = " + LegSListID.Value.ToString() + " ");
            }

            if (!String.IsNullOrEmpty(LegalAddress))
            {
                sb.Append("AND a.LegalAddress LIKE '%" + LegalAddress + "%' ");
            }
            if (!String.IsNullOrEmpty(Country))
            {
                sb.Append("AND a.Country LIKE '%" + Country + "%' ");
            }
            if (!String.IsNullOrEmpty(Area))
            {
                sb.Append("AND a.Area LIKE '%" + Area + "%' ");
            }
            if (!String.IsNullOrEmpty(City))
            {
                sb.Append("AND a.City LIKE '%" + City + "%' ");
            }
            if (!String.IsNullOrEmpty(Region))
            {
                sb.Append("AND a.Region LIKE '%" + Region + "%' ");
            }
            if (!String.IsNullOrEmpty(Village))
            {
                sb.Append("AND a.Village LIKE '%" + Village + "%' ");
            }
            if (!String.IsNullOrEmpty(Email))
            {
                sb.Append("AND a.email LIKE '%" + Email + "%' ");
            }
            if (!String.IsNullOrEmpty(WebSite))
            {
                sb.Append("AND a.website LIKE '%" + WebSite + "%' ");
            }


            if (!String.IsNullOrEmpty(FirstName))
            {
                sb.Append("AND c.FirstName LIKE '%" + FirstName + "%' ");
            }
            if (!String.IsNullOrEmpty(LastName))
            {
                sb.Append("AND c.LastName LIKE '%" + LastName + "%' ");
            }
            if (!String.IsNullOrEmpty(ContactPerEmail))
            {
                sb.Append("AND c.EmailAddress LIKE '%" + ContactPerEmail + "%' ");
            }


            if (!String.IsNullOrEmpty(DonorName))
            {
                sb.Append("AND [of].DonorName LIKE '%" + DonorName + "%' ");
            }
            if (!String.IsNullOrEmpty(ProjectName))
            {
                sb.Append("AND [of].ProjectName LIKE '%" + ProjectName + "%' ");
            }
            if (AmountFrom.HasValue)
            {
                sb.Append("AND [of].Amount >=" + AmountFrom.Value.ToString() + " ");
            }
            if (AmountTo.HasValue)
            {
                sb.Append("AND [of].Amount <=" + AmountTo.Value.ToString() + " ");
            }
            if (!String.IsNullOrEmpty(FundedYearFrom))
            {
                sb.Append("AND [of].FundedYear >=" + FundedYearFrom + " ");
            }
            if (!String.IsNullOrEmpty(FundedYearTo))
            {
                sb.Append("AND [of].FundedYear <=" + FundedYearTo + " ");
            }
            if (!String.IsNullOrEmpty(ContactPerson))
            {
                sb.Append("AND [of].ContactPerson LIKE '%" + ContactPerson + "%' ");
            }
            sb.Append(" ) ");
            IDataReader dr = sqlcmd.GetProjectsForFilter(sb.ToString());

            List<Project> Projects = LoadProjects(dr);

            return Projects;
        }

        public List<Project> Projects(int? AmountReqFrom, int? AmountReqTo, int? AwardedAmtFrom, int? AwardedAmtTo,
            string AcceptedDateFrom, string AcceptedDateTo, string StartDateFrom, string StartDateTo,
            string EndDateFrom, string EndDateTo, string CloseDateFrom, string CloseDateTo,
            string Country, string Area, string City, string Region, string Village, string Title,
            int? ProposalStatusID, int? ProgramAreaCodeID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM vProjects vp WHERE vp.ProjectID in (");
            sb.Append("SELECT p.ProjectID FROM Projects p ");
            sb.Append("LEFT JOIN ProjectInfo pji ON p.ProjectID = pji.ProjectInfoID ");
            sb.Append("LEFT JOIN ProjectLocation pl ON p.ProjectID = pl.ProjectID ");
            sb.Append("LEFT JOIN ProposalStatus ps ON p.ProjectID = ps.ProjectID ");
            sb.Append("LEFT JOIN ProposalStatusList psl ON ps.PropStatusID = psl.ProposalStatusID ");
            sb.Append("LEFT JOIN ProposalInfo ppi ON p.ProjectID = ppi.ProjectID ");
            sb.Append("LEFT JOIN ProgramArea pa ON p.ProjectID = pa.ProjectID ");
            sb.Append("LEFT JOIN ProgramAreaList pal ON pa.ProgramAreaCodeID = pal.ProgramAreaCodeID ");
            sb.Append("WHERE p.ProjectID <> -1 ");

            if (AmountReqFrom.HasValue)
            {
                sb.Append("AND pji.AmtRequested >= " + AmountReqFrom.Value.ToString() + " ");
            }
            if (AmountReqTo.HasValue)
            {
                sb.Append("AND pji.AmtRequested <= " + AmountReqTo.Value.ToString() + " ");
            }

            if (AwardedAmtFrom.HasValue)
            {
                sb.Append("AND pji.AwardedAmt >= " + AwardedAmtFrom.Value.ToString() + " ");
            }
            if (AwardedAmtTo.HasValue)
            {
                sb.Append("AND pji.AwardedAmt <= " + AwardedAmtTo.Value.ToString() + " ");
            }

            if (!string.IsNullOrEmpty(AcceptedDateFrom))
            {
                sb.Append("AND pji.AcceptedDate >= CONVERT(Datetime, '" + AcceptedDateFrom + "', 103) ");
            }
            if (!string.IsNullOrEmpty(AcceptedDateTo))
            {
                sb.Append("AND pji.AcceptedDate <= CONVERT(Datetime, '" + AcceptedDateTo + "', 103) ");
            }

            if (!string.IsNullOrEmpty(StartDateFrom))
            {
                sb.Append("AND pji.StartDate >= CONVERT(Datetime, '" + StartDateFrom + "', 103) ");
            }
            if (!string.IsNullOrEmpty(StartDateTo))
            {
                sb.Append("AND pji.StartDate <= CONVERT(Datetime, '" + StartDateTo + "', 103) ");
            }

            if (!string.IsNullOrEmpty(EndDateFrom))
            {
                sb.Append("AND pji.EndDate >= CONVERT(Datetime, '" + EndDateFrom + "', 103) ");
            }
            if (!string.IsNullOrEmpty(EndDateTo))
            {
                sb.Append("AND pji.EndDate <= CONVERT(Datetime, '" + EndDateTo + "', 103) ");
            }

            if (!string.IsNullOrEmpty(CloseDateFrom))
            {
                sb.Append("AND pji.ClosedDate >= CONVERT(Datetime, '" + CloseDateFrom + "', 103) ");
            }
            if (!string.IsNullOrEmpty(CloseDateTo))
            {
                sb.Append("AND pji.ClosedDate <= CONVERT(Datetime, '" + CloseDateTo + "', 103) ");
            }

            if (!string.IsNullOrEmpty(Country))
            {
                sb.Append("AND pl.Country LIKE '%" + Country + "%' ");
            }
            if (!string.IsNullOrEmpty(Area))
            {
                sb.Append("AND pl.Area LIKE '%" + Area + "%' ");
            }
            if (!string.IsNullOrEmpty(City))
            {
                sb.Append("AND pl.City LIKE '%" + City + "%' ");
            }
            if (!string.IsNullOrEmpty(Region))
            {
                sb.Append("AND pl.Region LIKE '%" + Region + "%' ");
            }
            if (!string.IsNullOrEmpty(Village))
            {
                sb.Append("AND pl.Village LIKE '%" + Village + "%' ");
            }

            if (!string.IsNullOrEmpty(Title))
            {
                sb.Append("AND (ppi.TitleR LIKE '%" + Title + "%' OR ppi.TitleR LIKE '%" + Title + "%')");
            }
            if (ProposalStatusID.HasValue)
            {
                sb.Append("AND psl.ProposalStatusID = " + ProposalStatusID.Value.ToString() + " ");
            }
            if (ProgramAreaCodeID.HasValue)
            {
                sb.Append("AND pa.ProgramAreaCodeID = " + ProgramAreaCodeID.Value.ToString() + " ");
            }
            sb.Append(" ) ");
            IDataReader dr = sqlcmd.GetProjectsForFilter(sb.ToString());

            List<Project> Projects = LoadProjects(dr);

            return Projects;
        }

        public List<Project> Reports(int? PID, int? ProposalStatusID, string OrganizationName,
            int? AwardedAmtFrom, int? AwardedAmtTo, string AcceptedDateFrom, string AcceptedDateTo,
            int? GrantTypeCodeID, int? ProgramAreaCodeID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM vProjects vp WHERE vp.ProjectID in (");
            sb.Append("SELECT p.ProjectID ");
            sb.Append("FROM ProjectInfo [pi], ProposalInfo [pri], General [g], Projects [p], ProposalStatusList [psl], ProposalStatus [ps], GrantType [gt], ProgramArea [pa] ");
            sb.Append("WHERE [pri].[ProjectID] = [pi].[ProjectInfoID] ");
            sb.Append("AND [pi].[ProjectInfoID] = [p].[ProjectID] ");
            sb.Append("AND [p].[OrgID] = [g].[OrgID] ");
            sb.Append("AND [ps].[PropStatusID] = [psl].[ProposalStatusID] ");
            sb.Append("AND [p].[ProjectID] = [gt].[ProjectID] ");
            sb.Append("AND [p].[ProjectID] = [pa].[ProjectID] ");
            if (PID.HasValue)
            {
                sb.Append("AND [pri].[ProjectID] IN ( " + PID.Value.ToString() + " ) ");
            }
            if (ProposalStatusID.HasValue)
            {
                sb.Append("AND [ps].[PropStatusID] = " + ProposalStatusID.Value.ToString() + " ");
            }
            if (!string.IsNullOrEmpty(OrganizationName))
            {
                sb.Append("AND [g].[Name] LIKE '%" + OrganizationName + "%' ");
            }
            if (AwardedAmtFrom.HasValue)
            {
                sb.Append("AND [pi].[AwardedAmt] >= " + AwardedAmtFrom.Value.ToString() + " ");
            }
            if (AwardedAmtTo.HasValue)
            {
                sb.Append("AND [pi].[AwardedAmt] <= " + AwardedAmtTo.Value.ToString() + " ");
            }
            if (!string.IsNullOrEmpty(AcceptedDateFrom))
            {
                sb.Append("AND [pi].[AcceptedDate] >= CONVERT(Datetime, '" + AcceptedDateFrom + "', 103) ");
            }
            if (!string.IsNullOrEmpty(AcceptedDateTo))
            {
                sb.Append("AND [pi].[AcceptedDate] <= CONVERT(Datetime, '" + AcceptedDateTo + "', 103) ");
            }
            if (GrantTypeCodeID.HasValue)
            {
                sb.Append("AND [gt].[GrantTypeCodeID] = " + GrantTypeCodeID.Value.ToString() + " ");
            }
            if (ProgramAreaCodeID.HasValue)
            {
                sb.Append("AND [pa].[ProgramAreaCodeID] = " + ProgramAreaCodeID.Value.ToString() + " ");
            }
            sb.Append(" ) ");
            IDataReader dr = sqlcmd.GetProjectsForFilter(sb.ToString());

            List<Project> Projects = LoadProjects(dr);

            return Projects;
        }


        public IDataReader Filter2(int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> oblast, List<String> period, List<String> amount,
            List<String> location)
        {
            StringBuilder qHeader = new StringBuilder();
            StringBuilder qFrom = new StringBuilder();
            StringBuilder qWhere = new StringBuilder();
            qHeader.Append("SELECT ROW_NUMBER() OVER(ORDER BY vp.Label ASC) AS '#', vp.Label AS 'Project ID', vp.NameRu AS 'Organization Name' ");
            qFrom.Append("FROM vProjects vp ");
            qWhere.Append("WHERE vp.ProjectID <> 0 ");

            if (ID != null) { qWhere.Append("AND vp.ProjectID = " + ID + " "); }
            else
            {
                if (Area != null && Area.Count > 0)
                {
                    qHeader.Append(", pal.ProgramAreaText AS 'Program Area' ");
                    qFrom.Append("LEFT JOIN ProgramArea pa ON vp.ProjectID = pa.ProjectID ");
                    qFrom.Append("LEFT JOIN ProgramAreaList pal ON pa.ProgramAreaCodeID = pal.ProgramAreaCodeID ");
                    if (!Area.Contains("All"))
                    {
                        for (int i = 0; i < Area.Count; i++)
                        {
                            if (i == 0)
                            {
                                qWhere.Append("AND (pa.ProgramAreaCodeID = " + Area[i].ToString() + " ");
                            }
                            else
                            {
                                qWhere.Append("OR pa.ProgramAreaCodeID = " + Area[i].ToString() + " ");
                            }
                        }
                        qWhere.Append(")");
                    }
                }


                if (location != null && location.Count > 0)
                {
                    qHeader.Append(", pls.Name AS 'Project Location' ");
                    qFrom.Append("LEFT JOIN ProjLocation pl ON vp.ProjectID = pl.ProjectID ");
                    qFrom.Append("LEFT JOIN ProjLocationList pls ON pl.ID = pls.ID ");
                    if (!location.Contains("All"))
                    {
                        for (int i = 0; i < Area.Count; i++)
                        {
                            if (i == 0)
                            {
                                qWhere.Append("AND (pl.ID = " + location[i].ToString() + " ");
                            }
                            else
                            {
                                qWhere.Append("OR pl.ID = " + location[i].ToString() + " ");
                            }
                        }
                        qWhere.Append(")");
                    }
                }




                if (gtype != null && gtype.Count > 0)
                {
                    qHeader.Append(", gtl.GrantTypeText AS 'Grant Type' ");
                    qFrom.Append("LEFT JOIN GrantType gt ON vp.ProjectID = gt.ProjectID ");
                    qFrom.Append("LEFT JOIN GrantTypeList gtl ON gt.GrantTypeCodeID = gtl.GrantTypeCodeID ");
                    if (!gtype.Contains("All"))
                    {
                        for (int i = 0; i < gtype.Count; i++)
                        {
                            if (i == 0)
                            {
                                qWhere.Append("AND (gt.GrantTypeCodeID = " + gtype[i].ToString() + " ");
                            }
                            else
                            {
                                qWhere.Append("OR gt.GrantTypeCodeID = " + gtype[i].ToString() + " ");
                            }
                        }
                        qWhere.Append(")");
                    }
                }

                if (compete != null && compete.Count > 0)
                {
                    qHeader.Append(", ccl.CodeText AS 'Round' ");
                    qFrom.Append("LEFT JOIN CompetitionCode cc ON vp.ProjectID = cc.ProjectID ");
                    qFrom.Append("LEFT JOIN CompetitionCodeList ccl ON cc.CompetCodeID = ccl.CompetitionCodeID ");
                    if (!compete.Contains("All"))
                    {
                        for (int i = 0; i < compete.Count; i++)
                        {
                            if (i == 0)
                            {
                                qWhere.Append("AND (cc.CompetCodeID = " + compete[i].ToString() + " ");
                            }
                            else
                            {
                                qWhere.Append("OR cc.CompetCodeID = " + compete[i].ToString() + " ");
                            }
                        }
                        qWhere.Append(")");
                    }
                }

                if (status != null && status.Count > 0)
                {
                    qHeader.Append(", psl.ProposalStatusText AS 'Status' ");
                    qFrom.Append("LEFT JOIN ProposalStatus ps ON vp.ProjectID = ps.ProjectID ");
                    qFrom.Append("LEFT JOIN ProposalStatusList psl ON ps.PropStatusID = psl.ProposalStatusID ");
                    if (!status.Contains("All"))
                    {
                        for (int i = 0; i < status.Count; i++)
                        {
                            if (i == 0)
                            {
                                qWhere.Append("AND (ps.PropStatusID = " + status[i].ToString() + " ");
                            }
                            else
                            {
                                qWhere.Append("OR ps.PropStatusID = " + status[i].ToString() + " ");
                            }
                        }
                        qWhere.Append(")");
                    }
                }

                if ((amount != null && amount.Count > 0) | (period != null && period.Count > 0))
                {
                    qFrom.Append("LEFT JOIN ProjectInfo pi ON vp.ProjectID = pi.ProjectInfoID ");
                    if (amount != null && amount.Count > 0)
                    {
                        qHeader.Append(", pi.AwardedAmt AS 'Grant Amount' ");
                        if (!amount.Contains("All"))
                        {
                            for (int i = 0; i < amount.Count; i++)
                            {
                                if (i == 0)
                                {
                                    qWhere.Append("AND (pi.AwardedAmt " + amount[i].ToString() + " ");
                                }
                                else
                                {
                                    qWhere.Append("OR pi.AwardedAmt " + amount[i].ToString() + " ");
                                }
                            }
                            qWhere.Append(")");
                        }
                    }
                    if (period != null && period.Count > 0)
                    {
                        qHeader.Append(", Convert(VARCHAR(10), pi.EndDate, 103) AS 'End Date' ");
                        if (!period.Contains("All"))
                        {
                            for (int i = 0; i < period.Count; i++)
                            {
                                if (i == 0)
                                {
                                    qWhere.Append("AND ((pi.EndDate >= CONVERT(Datetime, '1/1/" + period[i].ToString() + "', 103) AND pi.EndDate <= CONVERT(Datetime, '31/12/" + period[i].ToString() + "', 103)) ");
                                }
                                else
                                {
                                    qWhere.Append("OR (pi.EndDate >= CONVERT(Datetime, '1/1/" + period[i].ToString() + "', 103) AND pi.EndDate <= CONVERT(Datetime, '31/12/" + period[i].ToString() + "', 103)) ");
                                }
                            }
                            qWhere.Append(")");
                        }
                    }
                }

                if ((oblast != null && oblast.Count > 0) /*| (location!= null && location.Count > 0)*/)
                {
                    qFrom.Append("LEFT JOIN Address ad ON ad.OrgID = vp.OrgID ");                    
                 //   qWhere.Append("AND ad.isLegalAddress = 1 ");

                    //Avaz:Now it does not depend on isLegal, but uses the Last Entered Addresses's ID!                    
                    qWhere.Append("AND AddessID = (SELECT TOP 1 AddessID FROM Address where OrgID = vp.OrgID Order by AddessID asc) ");

                    if (oblast != null && oblast.Count > 0)
                    {
                        qFrom.Append("LEFT JOIN RegionList regl ON regl.DDID = ad.DDIDRegion ");
                        qHeader.Append(", regl.DDNAME AS 'Region' ");
                        if (!oblast.Contains("All"))
                        {
                            for (int i = 0; i < oblast.Count; i++)
                            {
                                if (i == 0)
                                {   //fixed the error- Avaz,  Joha SQL Injection is not TYPE SAFE! was reg.DDID
                                    qWhere.Append("AND (regl.DDID = " + oblast[i].ToString() + " ");
                                }
                                else
                                {
                                    qWhere.Append("OR regl.DDID = " + oblast[i].ToString() + " ");
                                }
                            }
                            qWhere.Append(")");
                        }
                    }





                 
                    //if (location != null && location.Count > 0)
                    //{
                    //    qHeader.Append(", ad.City ");
                    //    if (!location.Contains("All"))
                    //    {
                    //        for (int i = 0; i < location.Count; i++)
                    //        {
                    //            if (i == 0)
                    //            {
                    //                qWhere.Append("AND (ad.City " + " LIKE N'%" + location[i].ToString() + "%' ");
                    //            }
                    //            else
                    //            {
                    //                qWhere.Append("OR (ad.City " + " LIKE N'%" + location[i].ToString() + "%' ");
                    //            }
                    //        }
                    //        qWhere.Append(")");
                    //    }
                    //}
                    
                }
            }
            string FinalQuery = qHeader.ToString() + qFrom.ToString() + qWhere.ToString();
            IDataReader dr = sqlcmd.GetProjectsForFilter(FinalQuery);

            //List<Project> Projects = LoadProjects(dr);
            //return Projects;
            return dr;
        }
        private List<Project> LoadProjects(IDataReader dr)
        {
            bool error = false;
            List<Project> Projects = new List<Project>();

            try
            {
                using (dr)
                {
                    while (dr.Read())
                    {
                        //do smth
                        Project p = new Project();
                        if (dr["ProjectID"] != DBNull.Value)
                        {
                        p.ProjectID = Convert.ToInt32(dr["ProjectID"].ToString());
                        }
                        
                        p.ProposalInfo = new ProposalInfo();

                        if (dr["TitleE"] != DBNull.Value)
                        {
                            p.ProposalInfo.TitleE = dr["TitleE"].ToString();
                        }
                        if (dr["TitleR"] != DBNull.Value)
                        {
                            p.ProposalInfo.TitleR = dr["TitleR"].ToString();
                        }
                        
                        p.Organization = new Organization();
                        p.Organization.General = new General();

                        if (dr["Name"] != DBNull.Value)
                        {
                            p.Organization.General.Name = dr["Name"].ToString();
                        }
                        if (dr["NameRu"] != DBNull.Value)
                        {
                            p.Organization.General.NameRu = dr["NameRu"].ToString();
                        }
                        
                        p.ProposalStatus = new ProposalStatus();
                        p.ProposalStatus.ProposalStatusList = new ProposalStatusList();

                        if (dr["ProposalStatusText"] != DBNull.Value)
                        {
                            p.ProposalStatus.ProposalStatusList.ProposalStatusText = dr["ProposalStatusText"].ToString();
                        }
                        p.ProjectInfo = new ProjectInfo();
                        if (dr["AwardedAmt"] != DBNull.Value)
                        {
                            p.ProjectInfo.AwardedAmt = Convert.ToInt32(dr["AwardedAmt"].ToString());
                        }
                        Projects.Add(p);
                    }
                }
            }
            catch(Exception ex)
            {
                //---------- TODO: Joha use Try Catch and Log class. -----------
                Log.EnsureInitialized();
                Log.Error(typeof(ReportsRepository), "----------------------------------------------", ex);
                error = true;
            }
           finally
           {
               dr.Close();
           }

            if (error == true)
            {
                return null;
            }
            else
            {
                return Projects;
            }
        }
    }
}
