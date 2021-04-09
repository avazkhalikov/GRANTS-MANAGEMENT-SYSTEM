using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;
using System.Data;
using BOTACORE.CORE.Services.Impl;
using StructureMap;
using BOTACORE.CORE;
using BOTACORE.CORE.DataAccess.DAL;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace BOTACORE.CORE.DataAccess.Impl
{
   public static class ReportPredicates
    {

       //private static string connectString;
       static BOTADataContext db;

        static ReportPredicates()
       {
           // Connection conn = new Connection();
           //connectString = conn.GetDirectConnString();
           //db = new BOTADataContext(connectString);

            db = ReportsDataContext.GetDataContext();

       }


       

         public static IQueryable<Project> GetResults4(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
          List<String> status, List<String> period, List<String> indicatorCategory, List<String> oblast)
        {
            IQueryable<Project> matches = db.Projects;
            //  var inner_predicate = PredicateBuilder.False<Project>();   //this is for ORs. that is wahy .False selected. ====
            var outer_predicate = PredicateBuilder.True<Project>();  //this is for AND  =====


            //check if Organization Name
            if (frepf.isOrganizationName.Value)
            {
                //join the project with organization where organization has projectID.
                matches.Join(matches,
                            project => project,
                            organization => organization,
                            (project, organization) =>
                             new { ProjectID = project.ProjectID, OrgPrjectID = organization.ProjectID });

            }

             
            //5. List<String> oblast
            if (oblast != null && oblast.Count > 0 && !oblast.Contains("All"))
            {

                try
                {
                    var inner_predicate_oblast = PredicateBuilder.False<Project>();
                    foreach (string item in oblast)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_oblast =
                            inner_predicate_oblast.Or(a => a.Organization.Addresses.FirstOrDefault().DDIDRegion == val);
                    }

                    matches = matches.Where(inner_predicate_oblast);
                }
                catch
                {
                    ;
                }
            }

             ////1.Indicator Append.??? IndicatorCategory  is not a type of Project! One Project May have many Categories of Indicators.         
            //if (indicatorCategory != null && indicatorCategory.Count > 0)
            //{
            //    try
            //    {
            //        var inner_predicate_indicator = PredicateBuilder.False<Project>();
            //        foreach (string item in indicatorCategory)
            //        {
            //            if (item != "All")  //we don't wanna convert All into Int.
            //            {
            //                int val = Convert.ToInt32(item);
            //                inner_predicate_indicator = inner_predicate_indicator.Or(a => a.Indicator..LabelContentCategory == val);
            //            }
            //        }

            //        matches = matches.Where(inner_predicate_indicator);
            //    }
            //    catch { ; }

            //}


            //1.Area Append.          
            if (Area != null && Area.Count > 0)
            {
                try
                {
                    var inner_predicate_area = PredicateBuilder.False<Project>();
                    foreach (string item in Area)
                    {
                        if (item != "All")  //we don't wanna convert All into Int.
                        {
                            int val = Convert.ToInt32(item);
                            inner_predicate_area = inner_predicate_area.Or(a => a.ProgramArea.ProgramAreaCodeID == val);
                        }
                    }

                    matches = matches.Where(inner_predicate_area);
                }
                catch { ; }

            }


            //1.Area Append.          
            if (period != null && period.Count > 0)
            {
                try
                {
                    var inner_predicate_period = PredicateBuilder.False<Project>();
                    foreach (string item in period)
                    {
                        if (item != "All")  //we don't wanna convert All into Int.
                        {
                            int val = Convert.ToInt32(item);
                            inner_predicate_period = inner_predicate_period.Or(a => a.ProjectInfo.ClosedDate != null && a.ProjectInfo.ClosedDate.Value.Year == val);
                        }
                    }

                    matches = matches.Where(inner_predicate_period);
                }
                catch { ; }

            }

            //2. GrantType Append ...?
            if (gtype != null && gtype.Count > 0 && !gtype.Contains("All"))
            {
                try
                {
                    var inner_predicate_gtype = PredicateBuilder.False<Project>();
                    foreach (string item in gtype)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_gtype = inner_predicate_gtype.Or(a => a.GrantType.GrantTypeCodeID == val);
                    }

                    matches = matches.Where(inner_predicate_gtype);
                }
                catch { ; }
            }

            //3. Competition Code:  compete
            if (compete != null && compete.Count > 0 && !compete.Contains("All"))
            {
                try
                {
                    var inner_predicate_compete = PredicateBuilder.False<Project>();
                    foreach (string item in compete)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_compete = inner_predicate_compete.Or(a => a.CompetitionCode.CompetCodeID == val);
                    }

                    matches = matches.Where(inner_predicate_compete);
                }
                catch { ; }

            }

            if (status != null && status.Count > 0 && !status.Contains("All"))
            {

                var inner_predicate_status = PredicateBuilder.False<Project>();
                foreach (string item in status)
                {
                    int val = Convert.ToInt32(item);
                    inner_predicate_status = inner_predicate_status.Or(a => a.ProposalStatus.PropStatusID == val);
                }

                matches = matches.Where(inner_predicate_status);  //This Ands All Ors.
                //  outer_predicate = outer_predicate.And(inner_predicate_status);  //accumulate this inner.       

            }


            //remove the Deleted Project From Reports:
            matches = matches.Where(a => a.isDeleted.Value.Equals(null));

            //return  db.Projects.Where(predicate);
            //   matches = matches.Where(outer_predicate);
            //  List<Project> pr = matches.ToList();
            return matches;

        }

        public static IQueryable<Project> GetResults3(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
          List<String> status, List<String> period, List<String> oblast, List<String> amount, List<String> lfIndicator)
        {
            IQueryable<Project> matches = db.Projects;
            //  var inner_predicate = PredicateBuilder.False<Project>();   //this is for ORs. that is wahy .False selected. ====
            var outer_predicate = PredicateBuilder.True<Project>();  //this is for AND  =====

            //5. List<String> oblast
            if (oblast != null && oblast.Count > 0 && !oblast.Contains("All"))
            {

                try
                {
                    var inner_predicate_oblast = PredicateBuilder.False<Project>();
                    foreach (string item in oblast)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_oblast =
                            inner_predicate_oblast.Or(a => a.Organization.Addresses.FirstOrDefault().DDIDRegion == val);
                    }

                    matches = matches.Where(inner_predicate_oblast);
                }
                catch
                {
                    ;
                }


            }

            //6. List<String> amount < 290000   <3625000     >7250000
            if (amount != null && amount.Count > 0 && !amount.Contains("All"))
            {
                int Code = -1;
                var inner_predicate_amount = PredicateBuilder.False<Project>(); //create a new empty one .
                for (int i = 0; i < amount.Count; i++)
                {
                    try { Code = Convert.ToInt32(amount[i].ToString()); }
                    catch { ;}

                    if (Code > -1)
                    {
                        if (Code == 290000 || Code == 3625000)
                        {
                            inner_predicate_amount = inner_predicate_amount.Or(a => a.ProjectInfo.AwardedAmt < Code);
                        }
                        else
                        {
                            inner_predicate_amount = inner_predicate_amount.Or(a => a.ProjectInfo.AwardedAmt > Code);

                        }

                    }
                }

                matches = matches.Where(inner_predicate_amount);   //This Ands All Ors.
                //outer_predicate = outer_predicate.And(inner_predicate);  //accumulate this inner.
            }


            //check if Organization Name
            if (frepf!=null && frepf.isOrganizationName!= null && frepf.isOrganizationName.Value)
            {
                //join the project with organization where organization has projectID.
                matches.Join(matches,
                            project => project,
                            organization => organization,
                            (project, organization) =>
                             new { ProjectID = project.ProjectID, OrgPrjectID = organization.ProjectID });

            }
            
            //1.Area Append.          
            if (Area != null && Area.Count > 0 && !Area.Contains("All"))
            {
                try
                {
                    var inner_predicate_area = PredicateBuilder.False<Project>();
                    foreach (string item in Area)
                    {
                        if (item != "All")  //we don't wanna convert All into Int.
                        {
                            int val = Convert.ToInt32(item);
                            inner_predicate_area = inner_predicate_area.Or(a => a.ProgramArea.ProgramAreaCodeID == val);
                        }
                    }

                    matches = matches.Where(inner_predicate_area);
                }
                catch { ; }

            }


            //1.Area Append.          
            if (period != null && period.Count > 0 && !period.Contains("All"))
            {
                try
                {
                    var inner_predicate_period = PredicateBuilder.False<Project>();
                    foreach (string item in period)
                    {
                        if (item != "All")  //we don't wanna convert All into Int.
                        {
                            int val = Convert.ToInt32(item);
                            inner_predicate_period = inner_predicate_period.Or(a => a.ProjectInfo.ClosedDate != null && a.ProjectInfo.ClosedDate.Value.Year == val);
                        }
                    }

                    matches = matches.Where(inner_predicate_period);
                }
                catch { ; }

            }

            //2. GrantType Append ...?
            if (gtype != null && gtype.Count > 0 && !gtype.Contains("All"))
            {
                try
                {
                    var inner_predicate_gtype = PredicateBuilder.False<Project>();
                    foreach (string item in gtype)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_gtype = inner_predicate_gtype.Or(a => a.GrantType.GrantTypeCodeID == val);
                    }

                    matches = matches.Where(inner_predicate_gtype);
                }
                catch { ; }
            }

            //2. Lf Indicator ...?
            if (lfIndicator != null && lfIndicator.Count > 0 && !lfIndicator.Contains("All"))
            {
                try
                {
                    var inner_predicate_lf = PredicateBuilder.False<Project>();
                    foreach (string item in lfIndicator)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_lf = inner_predicate_lf.Or(a => a.LFIndicator.LFIndicatorID == val);
                    }

                    matches = matches.Where(inner_predicate_lf);
                }
                catch { ; }
            }


            //3. Competition Code:  compete
            if (compete != null && compete.Count > 0 && !compete.Contains("All"))
            {
                try
                {
                    var inner_predicate_compete = PredicateBuilder.False<Project>();
                    foreach (string item in compete)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_compete = inner_predicate_compete.Or(a => a.CompetitionCode.CompetCodeID == val);
                    }

                    matches = matches.Where(inner_predicate_compete);
                }
                catch { ; }

            }

            if (status != null && status.Count > 0 && !status.Contains("All"))
            {

                var inner_predicate_status = PredicateBuilder.False<Project>();
                foreach (string item in status)
                {
                    int val = Convert.ToInt32(item);
                    inner_predicate_status = inner_predicate_status.Or(a => a.ProposalStatus.PropStatusID == val);
                }

                matches = matches.Where(inner_predicate_status);  //This Ands All Ors.
                //  outer_predicate = outer_predicate.And(inner_predicate_status);  //accumulate this inner.       

            }


            //remove the Deleted Project From Reports:
            matches = matches.Where(a => a.isDeleted.Value.Equals(null));

            //return  db.Projects.Where(predicate);
            //   matches = matches.Where(outer_predicate);
            //  List<Project> pr = matches.ToList();
            return matches;

        }



        public static IQueryable<Project> GetResults2(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
      List<String> status, List<String> oblast, List<String> period, List<String> amount,
      List<String> location)
        {

            IQueryable<Project> matches = db.Projects;
            //  var inner_predicate = PredicateBuilder.False<Project>();   //this is for ORs. that is wahy .False selected. ====
            var outer_predicate = PredicateBuilder.True<Project>();  //this is for AND  =====

            //check Amount Requested.
            if (frepf.isAmountRequested.Value)
            {
                if (frepf.RequestedDateStart.HasValue && frepf.RequestedDateEnd.HasValue)
                {
                    matches = matches.Where(a => a.ProposalInfo.RequestDate >= frepf.RequestedDateStart
                                             && a.ProposalInfo.RequestDate <= frepf.RequestedDateEnd);
                }

            }

            //check if Organization Name
            if (frepf.isOrganizationName.Value)
            {
                //join the project with organization where organization has projectID.
                matches.Join(matches,
                            project => project,
                            organization => organization,
                            (project, organization) =>
                             new { ProjectID = project.ProjectID, OrgPrjectID = organization.ProjectID });

            }


            //1.Area Append.          
            if (Area != null && Area.Count > 0 && !Area.Contains("All"))
            {
                try
                {
                    var inner_predicate_area = PredicateBuilder.False<Project>();
                    foreach (string item in Area)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_area = inner_predicate_area.Or(a => a.ProgramArea.ProgramAreaCodeID == val);
                    }

                    matches = matches.Where(inner_predicate_area);
                }
                catch { ; }

            }

            //2. GrantType Append ...?
            if (gtype != null && gtype.Count > 0 && !gtype.Contains("All"))
            {
                try
                {
                    var inner_predicate_gtype = PredicateBuilder.False<Project>();
                    foreach (string item in gtype)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_gtype = inner_predicate_gtype.Or(a => a.GrantType.GrantTypeCodeID == val);
                    }

                    matches = matches.Where(inner_predicate_gtype);
                }
                catch { ; }
            }

            //3. Competition Code:  compete
            if (compete != null && compete.Count > 0 && !compete.Contains("All"))
            {
                try
                {
                    var inner_predicate_compete = PredicateBuilder.False<Project>();
                    foreach (string item in compete)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_compete = inner_predicate_compete.Or(a => a.CompetitionCode.CompetCodeID == val);
                    }

                    matches = matches.Where(inner_predicate_compete);
                }
                catch { ; }

            }



            //4. GrantStatus status
            //if (status != null && status.Count > 0 && !status.Contains("All"))
            //{
            //    int Code = -1;
            //    var inner_predicate_status = PredicateBuilder.False<Project>();
            //    int[] CodeArray = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0,0};

            //    for (int i = 0; i < status.Count; i++)
            //    {
            //        try { CodeArray[i] = Convert.ToInt32(status[i].ToString()); }
            //        catch { ;}

            //        if (CodeArray[i] > -1)
            //        {

            //            inner_predicate_status = inner_predicate_status.Or(a => a.ProposalStatus.PropStatusID == CodeArray[i]);

            //        }
            //    }

            if (status != null && status.Count > 0 && !status.Contains("All"))
            {

                var inner_predicate_status = PredicateBuilder.False<Project>();
                foreach (string item in status)
                {
                    int val = Convert.ToInt32(item);
                    inner_predicate_status = inner_predicate_status.Or(a => a.ProposalStatus.PropStatusID == val);
                }

                matches = matches.Where(inner_predicate_status);  //This Ands All Ors.
                //  outer_predicate = outer_predicate.And(inner_predicate_status);  //accumulate this inner.       

            }


            //5. List<String> oblast
            if (oblast != null && oblast.Count > 0 && !oblast.Contains("All"))
            {

                try
                {
                    var inner_predicate_oblast = PredicateBuilder.False<Project>();
                    foreach (string item in oblast)
                    {
                        int val = Convert.ToInt32(item);
                        inner_predicate_oblast = inner_predicate_oblast.Or(a => a.Organization.Addresses.FirstOrDefault().DDIDRegion == val);
                    }

                    matches = matches.Where(inner_predicate_oblast);
                }
                catch { ; }



                //int Code = -1;
                //var inner_predicate = PredicateBuilder.False<Project>(); //create a new empty one .
                //for (int i = 0; i < oblast.Count; i++)
                //{
                //    try { Code = Convert.ToInt32(oblast[i].ToString()); }
                //    catch { ;}

                //    if (Code > -1)
                //    {

                //        inner_predicate = inner_predicate.Or(a => a.Organization.Addresses.FirstOrDefault().DDIDRegion == Code);

                //    }
                //}

                //matches = matches.Where(inner_predicate);  //This Ands All Ors.
                //outer_predicate = outer_predicate.And(inner_predicate);  //accumulate this inner.
            }

            //6. List<String> amount < 290000   <3625000     >7250000
            if (amount != null && amount.Count > 0 && !amount.Contains("All"))
            {
                int Code = -1;
                var inner_predicate_amount = PredicateBuilder.False<Project>(); //create a new empty one .
                for (int i = 0; i < amount.Count; i++)
                {
                    try { Code = Convert.ToInt32(amount[i].ToString()); }
                    catch { ;}

                    if (Code > -1)
                    {
                        if (Code == 290000 || Code == 3625000)
                        {
                            inner_predicate_amount = inner_predicate_amount.Or(a => a.ProjectInfo.AwardedAmt < Code);
                        }
                        else
                        {
                            inner_predicate_amount = inner_predicate_amount.Or(a => a.ProjectInfo.AwardedAmt > Code);

                        }

                    }
                }

                matches = matches.Where(inner_predicate_amount);   //This Ands All Ors.
                //outer_predicate = outer_predicate.And(inner_predicate);  //accumulate this inner.
            }


            //remove the Deleted Project From Reports:
            matches = matches.Where(a => a.isDeleted.Value.Equals(null));

            //return  db.Projects.Where(predicate);
            //   matches = matches.Where(outer_predicate);
            //  List<Project> pr = matches.ToList();
            return matches;

        }

    }
}
