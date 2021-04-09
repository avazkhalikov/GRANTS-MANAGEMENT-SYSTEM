using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
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
using BOTACORE.CORE.Helpers;

namespace BOTACORE.CORE.DataAccess.Impl
{

    
   public class LReportsRepository
    {
       //private string connectString;
       //BOTADataContext db;
       static BOTADataContext db;
       public LReportsRepository()
       {
           //Connection conn = new Connection();
           //connectString = conn.GetDirectConnString();
           //db = new BOTADataContext(connectString);
           db = ReportsDataContext.GetDataContext();
          
       }

       /* 
        
VsContainer{
   int ProjId
   string Field1 
   string Field2
}
       //after grouped by Field1 and Field2.
List 1 projID        Field 1(string)       Field 2(string)
       projectId       Youth                 Social Service
       projectID       Youth                 Social Service
List 2
     projectID ECD Social Service
List 3 
     ProjectID Youth Action Plan
     ProjectID Youth Action Plan

Resulting List: VsContainer1, VsContainer2...
        */

     //  #region BudgetAnalysis Reports ============================
       //we have 7 Type of Amounts: from frepf.
       //InitialRequestedAmount =1
       //AwardedAmount  =2 
       //AllTransfered =3
       //UsedAmount =4 
       //UnUsedAmount =5 
       //CashOnHand = 6
       //Refund = 7
       //Cancellation = 8

       #region ============ Area vs. Status ================
       public List<VsContainer> DoAreaVsStatus(IQueryable<Project> result, int Amounts)
       {


           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.ProposalStatus.PropStatusID,
                   g.ProgramArea.ProgramAreaCodeID
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.PropStatusID,
                   Field2 = group.Key.ProgramAreaCodeID.Value,
                   //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                   //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                {
                    g.ProposalStatus.PropStatusID,
                    g.ProgramArea.ProgramAreaCodeID
                })
                .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                {
                    ProjId = group.Select(i => i.ProjectID).ToList(),
                    Field1 = group.Key.PropStatusID,
                    Field2 = group.Key.ProgramAreaCodeID.Value,
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

           //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoAreaVsStatusFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if (Amounts == 4)
           {
               var listResult = DoAreaVsStatusFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoAreaVsStatusFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoAreaVsStatusFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoAreaVsStatusFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoAreaVsStatusFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null;
       }


       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoAreaVsStatusFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.ProposalStatus.PropStatusID,
                   g.ProgramArea.ProgramAreaCodeID
               })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.PropStatusID,
                   Field2 = group.Key.ProgramAreaCodeID.Value,
                   dAmount = 0,
                   //==== Summing happens in Foreach!  Next===
                   ByAmountName = Result
               });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }
       #endregion

       #region ============ Type vs. Round ================
       public List<VsContainer> DoGrantTypeVsCompetitionCode(IQueryable<Project> result, int Amounts)
       {


           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.GrantType.GrantTypeCodeID,
                   g.CompetitionCode.CompetCodeID
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.CompetCodeID.Value,
                   Field2 = group.Key.GrantTypeCodeID.Value,
                   //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                   //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                {
                    g.GrantType.GrantTypeCodeID,
                    g.CompetitionCode.CompetCodeID
                })
                .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                {
                    ProjId = group.Select(i => i.ProjectID).ToList(),
                    Field1 = group.Key.CompetCodeID.Value,
                    Field2 = group.Key.GrantTypeCodeID.Value,
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

           //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoTypeVsCompetitionCodeFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if (Amounts == 4)
           {
               var listResult = DoTypeVsCompetitionCodeFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoTypeVsCompetitionCodeFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoTypeVsCompetitionCodeFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoTypeVsCompetitionCodeFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoTypeVsCompetitionCodeFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null;
       }


       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoTypeVsCompetitionCodeFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.CompetitionCode.CompetCodeID,
                   g.GrantType.GrantTypeCodeID
               })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.CompetCodeID.Value,
                   Field2 = group.Key.GrantTypeCodeID.Value,
                   dAmount = 0,
                   //==== Summing happens in Foreach!  Next===
                   ByAmountName = Result
               });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }
       #endregion

       #region ============ Type vs. Status ================
       public List<VsContainer> DoGrantTypeVsStatus(IQueryable<Project> result, int Amounts)
       {


           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.ProposalStatus.PropStatusID,
                   g.GrantType.GrantTypeCodeID
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.PropStatusID,
                   Field2 = group.Key.GrantTypeCodeID.Value,
                   //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                   //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                {
                    g.ProposalStatus.PropStatusID,
                    g.GrantType.GrantTypeCodeID
                })
                .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                {
                    ProjId = group.Select(i => i.ProjectID).ToList(),
                    Field1 = group.Key.PropStatusID,
                    Field2 = group.Key.GrantTypeCodeID.Value,
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

           //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoGrantTypeVsStatusFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if (Amounts == 4)
           {
               var listResult = DoGrantTypeVsStatusFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoGrantTypeVsStatusFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoGrantTypeVsStatusFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoGrantTypeVsStatusFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoGrantTypeVsStatusFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null;
       }


       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoGrantTypeVsStatusFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.ProposalStatus.PropStatusID,
                   g.GrantType.GrantTypeCodeID
               })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.PropStatusID,
                   Field2 = group.Key.GrantTypeCodeID.Value,
                   dAmount = 0,
                   //==== Summing happens in Foreach!  Next===
                   ByAmountName = Result
               });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }
       #endregion

       //case ContainerType.AreaVsRegion: result = rep.DoAreaVsRegion(prjList, 1).OrderBy(k => k.Field2).ToList(); break;
       //             case ContainerType.RoundVsRegion: result = rep.DoRoundVsRegion(prjList, 1).OrderBy(k => k.Field2).ToList(); break;
       //             case ContainerType.TypeVsRegion: result = rep.DoTypeVsRegion(prjList, 1).OrderBy(k => k.Field2).ToList(); break; 
       #region  DoRoundVsRegion DoCompetitionCodeVsStatus
       public List<VsContainer> DoRoundVsRegion(IQueryable<Project> result, int Amounts)
       {

           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.CompetitionCode.CompetCodeID,
                   g.Organization.Addresses.FirstOrDefault().DDIDRegion
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.DDIDRegion,
                   Field2 = group.Key.CompetCodeID,
                   //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                   //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                {
                    g.CompetitionCode.CompetCodeID,
                    g.Organization.Addresses.FirstOrDefault().DDIDRegion
                })
                .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                {
                    ProjId = group.Select(i => i.ProjectID).ToList(),
                    Field1 = group.Key.DDIDRegion,
                    Field2 = group.Key.CompetCodeID,
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

           //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoRoundVsRegionFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if (Amounts == 4)
           {
               var listResult = DoRoundVsRegionFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoRoundVsRegionFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoRoundVsRegionFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoRoundVsRegionFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoRoundVsRegionFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null;
       }

       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoRoundVsRegionFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.CompetitionCode.CompetCodeID,
                   g.Organization.Addresses.FirstOrDefault().DDIDRegion
               })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.DDIDRegion,
                   Field2 = group.Key.CompetCodeID,
                   dAmount = 0,
                   //==== Summing happens in Foreach!  Next===
                   ByAmountName = Result
               });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }

       #endregion 
       #region DoTypeVsRegion DoCompetitionCodeVsStatus
       public List<VsContainer> DoTypeVsRegion(IQueryable<Project> result, int Amounts)
       {

           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.GrantType.GrantTypeCodeID,
                   g.Organization.Addresses.FirstOrDefault().DDIDRegion
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.DDIDRegion,
                   Field2 = group.Key.GrantTypeCodeID,
                   //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                   //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                {
                    g.GrantType.GrantTypeCodeID,
                    g.Organization.Addresses.FirstOrDefault().DDIDRegion
                })
                .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                {
                    ProjId = group.Select(i => i.ProjectID).ToList(),
                    Field1 = group.Key.DDIDRegion,
                    Field2 = group.Key.GrantTypeCodeID,
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

           //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoTypeVsRegionFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if (Amounts == 4)
           {
               var listResult = DoTypeVsRegionFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoTypeVsRegionFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoTypeVsRegionFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoTypeVsRegionFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoTypeVsRegionFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null;
       }

       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoTypeVsRegionFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.GrantType.GrantTypeCodeID,
                   g.Organization.Addresses.FirstOrDefault().DDIDRegion
               })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.DDIDRegion,
                   Field2 = group.Key.GrantTypeCodeID,
                   dAmount = 0,
                   //==== Summing happens in Foreach!  Next===
                   ByAmountName = Result
               });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }

       #endregion 


       #region  DoAreaVsRegion DoCompetitionCodeVsStatus
       public List<VsContainer> DoAreaVsRegion(IQueryable<Project> result, int Amounts)
       {


           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.ProgramArea.ProgramAreaCodeID,
                   g.Organization.Addresses.FirstOrDefault().DDIDRegion
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.DDIDRegion,
                   Field2 = group.Key.ProgramAreaCodeID,
                   //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                   //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                {
                    g.ProgramArea.ProgramAreaCodeID,
                    g.Organization.Addresses.FirstOrDefault().DDIDRegion
                })
                .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                {
                    ProjId = group.Select(i => i.ProjectID).ToList(),
                    Field1 = group.Key.DDIDRegion,
                    Field2 = group.Key.ProgramAreaCodeID,
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

           //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoAreaVsRegionFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if (Amounts == 4)
           {
               var listResult = DoAreaVsRegionFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoAreaVsRegionFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoAreaVsRegionFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoAreaVsRegionFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoAreaVsRegionFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null;
       }


       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoAreaVsRegionFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.ProgramArea.ProgramAreaCodeID,
                   g.Organization.Addresses.FirstOrDefault().DDIDRegion
               })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.DDIDRegion,
                   Field2 = group.Key.ProgramAreaCodeID,
                   dAmount = 0,
                   //==== Summing happens in Foreach!  Next===
                   ByAmountName = Result
               });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }
       #endregion

       /// <summary>
       /// Returns aggregate(Sum) of one Project by salary, tax....Transfered and Awarded(Budget) amounts.
       /// </summary>
       /// <param name="prjList"></param>
       /// <returns></returns>
       public List<FinBudgetOneProjReport> GetBudgetOneProjectSum(IQueryable<Project> prjList)
       {
           //each grants award, all transfered by Category(Salary, tax)
           var result = (from i in prjList
                         join budg in db.Budgets on i.ProjectID equals budg.BudgetID
                         join farts in db.FinArticleCategoryRs on budg.BudgetID equals farts.BudgetID
                         select new FinBudgetOneProjReport
                                    {
                                        Proj = i,  //project.
                                        CatID = farts.FinCatID,
                                        Awarded = farts.Price,
                                        SumByCatTransfered = farts.ReportPeriodRs.Sum(a => a.Amount)
                                       
                                    }).ToList();
           return result; 
       }


       #region Budget(awarded, transfered) Versus Area/Type ...
       public List<FinCatReport> BudgetVsArea(IQueryable<Project> prjList)
      {
          //each grants award, all transfered by Category(Salary, tax)
          var result = (from i in prjList
                        join budg in db.Budgets on i.ProjectID equals budg.BudgetID
                        join farts in db.FinArticleCategoryRs on budg.BudgetID equals farts.BudgetID
                        select new 
                        {
                            ProjID = i.ProjectID,
                            CatID = farts.FinCatID,
                            Awarded = farts.Price,  // - budg.Returned
                            SumByCatTransfered = farts.ReportPeriodRs.Sum(a => a.Amount),
                            gKind =  i.ProgramArea.ProgramAreaCodeID
                        }).Distinct().ToList();  //*** in some projects i got duplicates!!!, 
                                                 //also some projects have 0 awarded Budget, even though grant already marked as accepted.

        // Compare  two columns, one AwardedAmountByArea( next awardfromcats.)

          //group by Program Area and sum all transfereds and awards under that group.
          if(result.Any())
          {
             var groupSum = result
                  .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.gKind,
                   g.CatID
               }).Select(reg => new FinCatReport()
                   {
                       CatID = reg.Key.CatID,
                       SumTrans = reg.Sum(p => p.SumByCatTransfered).Value,
                       SumBudget = reg.Sum(p => p.Awarded).Value,
                       gKind = reg.Key.gKind
                   });
             
              return groupSum.ToList();
          }

          return null; 
      }

      public List<FinCatReport> BudgetVsRound(IQueryable<Project> prjList)
       {
           //each grants award, all transfered by Category(Salary, tax)
           var result = (from i in prjList
                         join budg in db.Budgets on i.ProjectID equals budg.BudgetID
                         join farts in db.FinArticleCategoryRs on budg.BudgetID equals farts.BudgetID
                         select new
                         {
                             ProjID = i.ProjectID,
                             CatID = farts.FinCatID,
                             Awarded = farts.Price,  // - budg.Returned
                             SumByCatTransfered = farts.ReportPeriodRs.Sum(a => a.Amount),
                             gKind = i.CompetitionCode.CompetCodeID
                         }).Distinct().ToList();  //*** in some projects i got duplicates!!!, 
           //also some projects have 0 awarded Budget, even though grant already marked as accepted.

           // Compare  two columns, one AwardedAmountByArea( next awardfromcats.)

           //group by Program Area and sum all transfereds and awards under that group.
           if (result.Any())
           {
               var groupSum = result
                    .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                    {
                        g.gKind,
                        g.CatID
                    }).Select(reg => new FinCatReport()
                    {
                        CatID = reg.Key.CatID,
                        SumTrans = reg.Sum(p => p.SumByCatTransfered).Value,
                        SumBudget = reg.Sum(p => p.Awarded).Value,
                        gKind = reg.Key.gKind
                    });

               return groupSum.ToList();
           }

           return null; ; 
       }

      public List<FinCatReport> BudgetVsStatus(IQueryable<Project> prjList)
      {
          //each grants award, all transfered by Category(Salary, tax)
          var result = (from i in prjList
                        join budg in db.Budgets on i.ProjectID equals budg.BudgetID
                        join farts in db.FinArticleCategoryRs on budg.BudgetID equals farts.BudgetID
                        select new
                        {
                            ProjID = i.ProjectID,
                            CatID = farts.FinCatID,
                            Awarded = farts.Price,  // - budg.Returned
                            SumByCatTransfered = farts.ReportPeriodRs.Sum(a => a.Amount),
                            gKind = i.ProposalStatus.PropStatusID
                        }).Distinct().ToList();  //*** in some projects i got duplicates!!!, 
          //also some projects have 0 awarded Budget, even though grant already marked as accepted.

          // Compare  two columns, one AwardedAmountByArea( next awardfromcats.)

          //group by Program Area and sum all transfereds and awards under that group.
          if (result.Any())
          {
              var groupSum = result
                   .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                   {
                       g.gKind,
                       g.CatID
                   }).Select(reg => new FinCatReport()
                   {
                       CatID = reg.Key.CatID,
                       SumTrans = reg.Sum(p => p.SumByCatTransfered).Value,
                       SumBudget = reg.Sum(p => p.Awarded).Value,
                       gKind = reg.Key.gKind
                   });

              return groupSum.ToList();
          }

          return null; ;
      }

      public List<FinCatReport> BudgetVsType(IQueryable<Project> prjList)
      {
          //each grants award, all transfered by Category(Salary, tax)
          var result = (from i in prjList
                        join budg in db.Budgets on i.ProjectID equals budg.BudgetID
                        join farts in db.FinArticleCategoryRs on budg.BudgetID equals farts.BudgetID
                        select new
                        {
                            ProjID = i.ProjectID,
                            CatID = farts.FinCatID,
                            Awarded = farts.Price,  // - budg.Returned
                            SumByCatTransfered = farts.ReportPeriodRs.Sum(a => a.Amount),
                            gKind = i.GrantType.GrantTypeCodeID
                        }).Distinct().ToList();  //*** in some projects i got duplicates!!!, 
          //also some projects have 0 awarded Budget, even though grant already marked as accepted.

          // Compare  two columns, one AwardedAmountByArea( next awardfromcats.)

          //group by Program Area and sum all transfereds and awards under that group.
          if (result.Any())
          {
              var groupSum = result
                   .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                   {
                       g.gKind,
                       g.CatID
                   }).Select(reg => new FinCatReport()
                   {
                       CatID = reg.Key.CatID,
                       SumTrans = reg.Sum(p => p.SumByCatTransfered).Value,
                       SumBudget = reg.Sum(p => p.Awarded).Value,
                       gKind = reg.Key.gKind
                   });

              return groupSum.ToList();
          }

          return null; 
      }

       #endregion 

      // db.FinArtCatListRs Budget Analysis last Step
       public void AggregateBudgeByCat()
       {
           //from r in result
                          //            join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
           /* select new
                                      {
                                          ProjectID = r.ProjectID,
                                          IndicatorID = iitems.IndicatorID,
                                          IndicatorLabelID = iitems.IndicatorLabelID,
                                          IndicatorLabelText = ilabel.Text,
                                          IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                                          ToCount = iitems.Final,// - iitems.Baseline,
                                          //TODO:  this is asked in Reports form double Check!
                                          LabelContentCategory = iContentLabel.ID,
                                          // labelOrder = ilabel.LabelOrder
                                      }).ToList();
            * var groupSum = IndicatorByBase.GroupBy(ibase => ibase.IndicatorLabelID)
                   .Select(reg => new LabelByCount()
                   {
                       Counted = reg.Sum(p => p.ToCount).Value,
                       //TODO:summing is happening here.
                       Label = reg.FirstOrDefault().IndicatorLabelText,
                       GrantTypeCodeID = reg.FirstOrDefault().IndicatorLabelGranTypeCodeID,
                       LabelContentCategoryID = reg.FirstOrDefault().LabelContentCategory,
                       LabelID = reg.Key,
                       //labelOrder = reg.Select(l => l.labelOrder)
                   });*/
         
           //==================
           //this chunk of Linq is executed against each VS.(Round1, Round2..) and Added to LIST HOLDER/Container!
           //moneyContainer<Round1, List<CatAmountHolder>>
           //CatAmountHolder = CatID, Sum Awarded, Sum Transfered.
           //====================
           var result = (from i in db.Projects
                        join budg in db.Budgets on i.ProjectID equals budg.BudgetID
                        join farts in db.FinArticleCategoryRs on budg.BudgetID equals farts.BudgetID
                        where i.ProjectID == 194 
                        select new
                                   {
                                       ProjID = i.ProjectID,
                                       CatID = farts.FinCatID, 
                                       Awarded = farts.Price, 
                                       SumByCatTransfered = farts.ReportPeriodRs.Sum(a=>a.Amount)
                                   }).ToList();

        /*   above gives me this! 
         * { ProjID = 194, CatID = 4, Awarded = 1009440, SumByCatTransfered = 974636 }
             { ProjID = 194, CatID = 5, Awarded = 45425, SumByCatTransfered = 30898.62 }
              .....ProjiD 195 ... */          
           
           foreach (var r in result)
           {
               int projID =  r.ProjID;
               int CatID = r.CatID.Value;
               decimal awarded = r.Awarded.Value;
               decimal transd = r.SumByCatTransfered.Value; 
           }


       }


       public void PopulateFinCatIDStrict()
       {
           var cats = (from ls in db.FinArtCatListRs
                         select ls).OrderBy(c => c.FinArticleCatListID).ToList();
           var finArticles = from ls in db.FinArticleCategoryRs
                             select ls; 
           foreach(FinArtCatListR cat in cats)
           {
              foreach(FinArticleCategoryR fa in finArticles)
              {
                 if(cat.FinArticleCatName == fa.FinArticleCatText)  //strict comparasion
                 {
                    UpdateFinArtCatID(cat, fa);
                 }
              }
           }

       }

       /// <summary>
       /// this is run only after strict popuplation!
       /// </summary>
       public void PopulateFinCatIDNonStrict()
       {
           var cats = (from ls in db.FinArtCatListRs
                       select ls).OrderBy(c => c.FinArticleCatListID).ToList();
           var finArticles = from ls in db.FinArticleCategoryRs
                             where ls.FinCatID == 0
                             select ls;
           foreach (FinArtCatListR cat in cats)
           {
               foreach (FinArticleCategoryR fa in finArticles)
               {
                   if (cat.FinArticleCatName.Contains(fa.FinArticleCatText))  //strict comparasion
                   {
                       UpdateFinArtCatID(cat, fa);
                   }
               }
           }

       }

       private void UpdateFinArtCatID(FinArtCatListR cat, FinArticleCategoryR fa)
       {
           bool result = true;
           try
           {
               var budgetToUpdate = (from b in db.FinArticleCategoryRs
                                     where b.FinArticleCatID== fa.FinArticleCatID
                                     select b).First();

               if (budgetToUpdate != null)
               {
                   budgetToUpdate.FinCatID = cat.FinArticleCatListID;

                   // db.Budgets.Attach(budgetToUpdate);
                   db.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, budgetToUpdate);
                   db.SubmitChanges();
               }
           }
           catch (Exception ex)
           {
               //   result = false;
           }
       }

       #region ============ Round vs. Status ================
       public List<VsContainer> DoCompetitionCodeVsStatus(IQueryable<Project> result, int Amounts)
       {


           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.CompetitionCode.CompetCodeID,
                   g.ProposalStatus.PropStatusID
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.PropStatusID,
                   Field2 = group.Key.CompetCodeID.Value,
                   //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                   //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                {
                    g.CompetitionCode.CompetCodeID,
                    g.ProposalStatus.PropStatusID
                })
                .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                {
                    ProjId = group.Select(i => i.ProjectID).ToList(),
                    Field1 = group.Key.PropStatusID,
                    Field2 = group.Key.CompetCodeID.Value,
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

           //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoCompetitionCodeVsStatusFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if (Amounts == 4)
           {
               var listResult = DoCompetitionCodeVsStatusFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoCompetitionCodeVsStatusFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoCompetitionCodeVsStatusFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoCompetitionCodeVsStatusFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoCompetitionCodeVsStatusFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null;
       }


       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoCompetitionCodeVsStatusFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.CompetitionCode.CompetCodeID,
                   g.ProposalStatus.PropStatusID
               })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.PropStatusID,
                   Field2 = group.Key.CompetCodeID.Value,
                   dAmount = 0,
                   //==== Summing happens in Foreach!  Next===
                   ByAmountName = Result
               });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }
       #endregion

       #region ===========  doAreaVsCompetitionCode ===============
       /// <summary>
       /// AreaVsRound Aggregator.
       /// </summary>
       /// <param name="result"></param>
       /// <param name="Amounts"></param>
       /// <returns>List of VsContainer.</returns>
       public List<VsContainer> DoAreaVsCompetitionCode(IQueryable<Project> result, int Amounts)
       {


           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
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
                   //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                   //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
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
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

           //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoAreaVsCompetitonCodeFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if (Amounts == 4)
           {
               var listResult = DoAreaVsCompetitonCodeFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoAreaVsCompetitonCodeFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoAreaVsCompetitonCodeFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoAreaVsCompetitonCodeFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoAreaVsCompetitonCodeFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null;
       }


       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoAreaVsCompetitonCodeFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.CompetitionCode.CompetCodeID,
                   g.ProgramArea.ProgramAreaCodeID
               })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.CompetCodeID.Value,
                   Field2 = group.Key.ProgramAreaCodeID.Value,
                   dAmount = 0,
                   //==== Summing happens in Foreach!  Next===
                   ByAmountName = Result
               });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }

#endregion 


       #region ========+== doAreaVsType Financials. =================
       /// <summary>
       /// AreaVsType Aggregator.
       /// </summary>
       /// <param name="result"></param>
       /// <param name="Amounts"></param>
       /// <returns>List of VsContainer.</returns>
       public List<VsContainer> doAreaVsType(IQueryable<Project> result, int Amounts)
       {
          

           if (Amounts == 1)  // 1 == Initial RequestedAmount==
           {
               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.GrantType.GrantTypeCodeID,
                   g.ProgramArea.ProgramAreaCodeID
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.GrantTypeCodeID.Value,
                   Field2 = group.Key.ProgramAreaCodeID.Value,
                 //  Field1Title = group.Select(k => k.GrantType.GrantTypeList.GrantTypeText).FirstOrDefault(), //group.Select(k => k.GrantType.GrantTypeList).Where(d => d.GrantTypeCodeID == group.Key.GrantTypeCodeID.Value).FirstOrDefault().GrantTypeText,
                 //  Field2Title = group.Select(k => k.ProgramArea.ProgramAreaList.ProgramAreaText).FirstOrDefault(),
                   dAmount = group.Sum(c => c.ProjectInfo.AmtRequested != null ? c.ProjectInfo.AmtRequested.Value : 0),
                   ByAmountName = "Initial Requested"
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
           }
           if (Amounts == 2) //==Awarded Amount===
           {
               var query = result
                .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                {
                    g.GrantType.GrantTypeCodeID,
                    g.ProgramArea.ProgramAreaCodeID
                })
                .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
                {
                    ProjId = group.Select(i => i.ProjectID).ToList(),
                    Field1 = group.Key.GrantTypeCodeID.Value,
                    Field2 = group.Key.ProgramAreaCodeID.Value,
                    dAmount = group.Sum(c => c.ProjectInfo.AwardedAmt),
                    ByAmountName = "Awarded Amount"
                });

               List<VsContainer> vsc = query.ToList();
               return query.ToList();   //add result to Container List.
           }

         //  FinanceResults finres = new FinanceResults(id.Value);
           if (Amounts == 3) //==All Transfered==
           {
               var listResult = DoAreaVsTypeFinancialList(result, "Total Transfered");
               return listResult;//query.ToList();   //add result to Container List.
           }
           // //UsedAmount =4 
           if(Amounts  == 4)
           {
               var listResult = DoAreaVsTypeFinancialList(result, "Used Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //UnUsedAmount =5 
           if (Amounts == 5)
           {
               var listResult = DoAreaVsTypeFinancialList(result, "UnUsed Amount");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //CashOnHand = 6
           if (Amounts == 6)
           {
               var listResult = DoAreaVsTypeFinancialList(result, "Cash OnHand");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Refund = 7
           if (Amounts == 7)
           {
               var listResult = DoAreaVsTypeFinancialList(result, "Refund");
               return listResult;//query.ToList();   //add result to Container List. 
           }
           //Cancellation = 8
           if (Amounts == 8)
           {
               var listResult = DoAreaVsTypeFinancialList(result, "Cancellation");
               return listResult;//query.ToList();   //add result to Container List. 
           }

           return null; 
       }


       //This must be refactored into DoXVsYfinance(X, Y) 
       //case X, Y
       //Case X, Y 
       private static List<VsContainer> DoAreaVsTypeFinancialList(IQueryable<Project> result, string Result)
       {
           //This Query For AreaVSType
           var query = result
               .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
                                 {
                                     g.GrantType.GrantTypeCodeID,
                                     g.ProgramArea.ProgramAreaCodeID
                                 })
               .Select(group => new VsContainer() //Select all Grouped into VersusContainer.
                                    {
                                        ProjId = group.Select(i => i.ProjectID).ToList(),
                                        Field1 = group.Key.GrantTypeCodeID.Value,
                                        Field2 = group.Key.ProgramAreaCodeID.Value,
                                        dAmount = 0,
                                       //==== Summing happens in Foreach!  Next===
                                        ByAmountName = Result
                                    });

           //Next Case Area VS Round.

           //Since SQL can't be generated for calling method and aggregating, doing aggregation manually through foreach.
           //summing total transfer amount for each grant withing Area/Vs Type Projects collection.
           var listResult = VsContainers(Result, query);

           // List<VsContainer> vsc = query.ToList();
           return listResult;
       }
#endregion 
       #region =========== VsContainer ===========
       private static List<VsContainer> VsContainers(string Result, IQueryable<VsContainer> query)
       {
           FinanceResults finres = new FinanceResults();
           BudgetService budservice = new BudgetService();
           decimal Sum = 0;
           List<VsContainer> listResult = query.ToList();
           foreach (var vsContainer in listResult)
           {
               foreach (var prjId in vsContainer.ProjId)
               {
                   finres.Initialize(prjId);
                   decimal amt = 0;

                   if (Result == "Total Transfered")
                   {
                       amt = finres.Project_TotalMoneyTransferedFromAwardAmount();
                   }
                   if (Result == "Used Amount")
                   {
                       amt = finres.Project_TotalMoneySpentAmountFromAwardAmount();
                   }
                   if (Result == "UnUsed Amount")
                   {
                       amt = finres.Project_TotalAmountLeftFromAwardAmount();
                   }
                   if (Result == "Cash OnHand ")
                   {
                       amt = finres.Project_TotalCashOnHand();
                   }
                   if (Result == "Refund")
                   {
                       //TODO Check with : =========
                       //Alya, what Returned she wants to USE! SEE IF Reports periods one is OKAY> BELOW
                       //   ViewData["Refund"] = SumTransfered - SumSpent - b.Returned;  // . Awarded Amt - total paid amt
                       Budget b = budservice.GetBudget(prjId);
                       if (b.Returned != null) amt = b.Returned.Value;
                   }
                   if (Result == "Cancellation")
                   {
                       // ViewData["Cancellation"] = ProjInfo.AwardedAmt.Value - b.Cancellation - SumTransfered;  // . Awarded Amt - total paid amt
                       Budget b = budservice.GetBudget(prjId);
                       if (b.Cancellation != null) amt = b.Cancellation.Value;
                   }


                   Sum = Sum + amt;
               }

               vsContainer.dAmount = Sum;
               Sum = 0;
           }
           return listResult;
       }

       #endregion 


       #region New Indicator Reports
       public List<IndicatorRepContainer> OblastVsIndicatorLabelCategory(IQueryable<Project> result, int contentCategory, List<RegionList> regions)
       {
           
           var iresult2 = new List<IndicatorRepContainer>();
           foreach (RegionList reg in regions)
           {
               //fucking ignore resharper!, Linq does check for null while "Where"
               var projs = result.Where(d => d.Organization.Addresses.FirstOrDefault().DDIDRegion == reg.DDID);
              // List<Project> prjList = projs.ToList();

               List<LabelByCount> resLabel = IndicatorByContentCategory(projs, contentCategory);
               //insert into resulting view container each category item and returned results.
               var indicatorRepContainer = new IndicatorRepContainer();
               indicatorRepContainer.Col = resLabel;
               indicatorRepContainer.Row = reg.DDNAME;
               iresult2.Add(indicatorRepContainer);

           }

           return iresult2;
       }

       public List<IndicatorRepContainer> AreaVsIndicatorLabelCategory(IQueryable<Project> result, int contentCategory, List<ProgramAreaList> areas)
       {
           
           var iresult2 = new List<IndicatorRepContainer>();
           foreach (ProgramAreaList are in areas)
           {
               //fucking ignore resharper!, Linq does check for null while "Where"
               var projs = result.Where(d => d.ProgramArea.ProgramAreaCodeID == are.ProgramAreaCodeID);
               List<Project> prjList = projs.ToList();

               List<LabelByCount> resLabel = IndicatorByContentCategory(projs, contentCategory);
               //insert into resulting view container each category item and returned results.
               var indicatorRepContainer = new IndicatorRepContainer();
               indicatorRepContainer.Col = resLabel;
               indicatorRepContainer.Row = are.ProgramAreaText;
               iresult2.Add(indicatorRepContainer);

           }

           return iresult2;
       }

       public List<IndicatorRepContainer> RoundVsIndicatorLabelCategory(IQueryable<Project> result, int contentCategory, List<CompetitionCodeList> codes)
       {

           var iresult2 = new List<IndicatorRepContainer>();
           foreach (CompetitionCodeList code in codes)
           {
               //fucking ignore resharper!, Linq does check for null while "Where"
               var projs = result.Where(d => d.CompetitionCode.CompetCodeID== code.CompetitionCodeID);
               List<Project> prjList = projs.ToList();

               List<LabelByCount> resLabel = IndicatorByContentCategory(projs, contentCategory);
               //insert into resulting view container each category item and returned results.
               var indicatorRepContainer = new IndicatorRepContainer();
               indicatorRepContainer.Col = resLabel;
               indicatorRepContainer.Row = code.CodeText;
               iresult2.Add(indicatorRepContainer);

           }

           return iresult2;
       }

       public List<IndicatorRepContainer> TypeVsIndicatorLabelCategory(IQueryable<Project> result, int contentCategory, List<GrantTypeList> types)
       {

           var iresult2 = new List<IndicatorRepContainer>();
           foreach (GrantTypeList type in types)
           {
               //fucking ignore resharper!, Linq does check for null while "Where"
               var projs = result.Where(d => d.GrantType.GrantTypeCodeID == type.GrantTypeCodeID);
               List<Project> prjList = projs.ToList();

               List<LabelByCount> resLabel = IndicatorByContentCategory(projs, contentCategory);
               //insert into resulting view container each category item and returned results.
               var indicatorRepContainer = new IndicatorRepContainer();
               indicatorRepContainer.Col = resLabel;
               indicatorRepContainer.Row = type.GrantTypeText;
               iresult2.Add(indicatorRepContainer);

           }

           return iresult2;
       }
       //List<Project> listprojs = result.ToList();

           //var all = new List<VsContainer2>();
           //if (contentCategory == 1)  // 1 == Initial RequestedAmount==
           //{
           //    //group by results projects by Regions.
           //    foreach (var regionList in Regions)
           //    {
           //        var vsContainer2 = new VsContainer2 {Field1 = regionList.DDID};
           //        var projContainer = new List<Project>();
           //        vsContainer2.Projs = projContainer;   //each region inserted into container item and each item has project list of that region.
           //        foreach (var proj in result)
           //        {
           //            if (proj.Organization != null && proj.Organization.Addresses != null && proj.Organization.Addresses.FirstOrDefault() != null 
           //                && proj.Organization.Addresses.FirstOrDefault().DDIDRegion != null) 
           //            {
           //                if (proj.Organization.Addresses.FirstOrDefault().DDIDRegion == regionList.DDID)
           //                   vsContainer2.Projs.Add(proj);
           //            }
           //         }
           //        all.Add(vsContainer2);
           //    }
           //}

           ////all is the container that has: 1. Region1, List<Project>  2. Region2, List<Project>
           ////Next step sends each List of Projects  to IndicatorByContentCategory which makes calculation of bench,base...of that set of projects.
           //var iresult = new List<IndicatorRepContainer>();
           //foreach(VsContainer2 vs2 in all)
           //{
           //    List<LabelByCount> resLabel = IndicatorByContentCategory(vs2.Projs.AsQueryable(), contentCategory);
               
           //    //insert into resulting view container each category item and returned results.
           //    var indicatorRepContainer = new IndicatorRepContainer();
           //    indicatorRepContainer.Col = resLabel;
           //    indicatorRepContainer.Row = Regions.FirstOrDefault(r => r.DDID == vs2.Field1).DDNAME;
           //    iresult.Add(indicatorRepContainer);
           //}

           //int hello = 0; 
     

           //return null;
      
       #endregion 

       #region Indicators

       //--------------------------Indicators ==========================================================
       public List<IndRepHolder> IndicatorsByRoundArea(IQueryable<Project> result)
       {
           //Get the List of All We need! see select new
           var indics = (from i in db.Indicators
                         from r in result
                         join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                         join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals ilabel.IndicatorLabelID
                         where iitems.IndicatorID == r.ProjectID && ilabel.LabelType != 2   //don't count AMOUNTS!  LabelType = 2
                         select new
                         {
                             ProjectID = r.ProjectID,
                             IndicatorID = iitems.IndicatorID,
                             IndicatorLabelID = iitems.IndicatorLabelID,
                             IndicatorLabelText = ilabel.Text,
                             IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                             ToCount = iitems.Final- iitems.Baseline,
                             CodeText = r.CompetitionCode.CompetitionCodeList.CodeText, 
                             ProgramAreaText = r.ProgramArea.ProgramAreaList.ProgramAreaText
                         }).ToList();

          
           var query = indics
              .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.CodeText,
                  g.ProgramAreaText
              })
              .Select(group => new IndRepHolder() //Select all Grouped into VersusContainer.
              {
                  Column = group.Key.CodeText,
                  Row = group.Key.ProgramAreaText,
                  Val = group.Sum(s=>s.ToCount).Value                  
              });

           return query.ToList(); 
        
       }

       //--------------------------Indicators ==========================================================
       public List<IndRepHolder> IndicatorsByCompetitionContentCategory(IQueryable<Project> result)
       {
           //Get the List of All We need! see select new
           var indics = (from i in db.Indicators
                         from r in result
                         join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                         join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals ilabel.IndicatorLabelID
                          join iContentLabel in db.IndicatorLabelContentCategories on
                                          ilabel.LabelContentCategory equals iContentLabel.ID
                         where iitems.IndicatorID == r.ProjectID && ilabel.LabelType != 2   //don't count AMOUNTS!  LabelType = 2
                         select new
                         {
                             ProjectID = r.ProjectID,
                             IndicatorID = iitems.IndicatorID,
                             IndicatorLabelID = iitems.IndicatorLabelID,
                             IndicatorLabelText = ilabel.Text,
                             IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                             ToCount = iitems.Final - iitems.Baseline,
                             CodeText = r.CompetitionCode.CompetitionCodeList.CodeText,
                             ProgramAreaText = r.ProgramArea.ProgramAreaList.ProgramAreaText, 
                             LabelContentCategory = iContentLabel.Text
                         }).ToList();


           var query = indics
              .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                 // g.CodeText,
                  g.LabelContentCategory,
                  g.CodeText
                 // g.ProgramAreaText
              })
              .Select(group => new IndRepHolder() //Select all Grouped into VersusContainer.
              {
                  Column = group.Key.LabelContentCategory,
                  Row = group.Key.CodeText,
                  Val = group.Sum(s => s.ToCount).Value
              });

           return query.ToList();

       }


       public List<IndRepHolder> IndicatorsByRegionContentCategory(IQueryable<Project> result)
       {
           //Get the List of All We need! see select new
           var indics = (from i in db.Indicators
                         from r in result
                         join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                         join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals ilabel.IndicatorLabelID
                         join iContentLabel in db.IndicatorLabelContentCategories on
                                         ilabel.LabelContentCategory equals iContentLabel.ID
                         where iitems.IndicatorID == r.ProjectID && ilabel.LabelType != 2   //don't count AMOUNTS!  LabelType = 2
                         select new
                         {
                             ProjectID = r.ProjectID,
                             IndicatorID = iitems.IndicatorID,
                             IndicatorLabelID = iitems.IndicatorLabelID,
                             IndicatorLabelText = ilabel.Text,
                             IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                             ToCount = iitems.Final - iitems.Baseline,
                             CodeText = r.Organization.Addresses.FirstOrDefault().RegionList.DDNAME,
                             ProgramAreaText = r.ProgramArea.ProgramAreaList.ProgramAreaText,
                             LabelContentCategory = iContentLabel.Text
                         }).ToList();


           var query = indics
              .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  // g.CodeText,
                  g.LabelContentCategory,
                  g.CodeText
                  // g.ProgramAreaText
              })
              .Select(group => new IndRepHolder() //Select all Grouped into VersusContainer.
              {
                  Column = group.Key.LabelContentCategory,
                  Row = group.Key.CodeText,
                  Val = group.Sum(s => s.ToCount).Value
              });

           return query.ToList();

       }

       public List<IndRepHolder> IndicatorsByRegionArea(IQueryable<Project> result)
       {
           //Get the List of All We need! see select new
           var indics = (from i in db.Indicators
                         from r in result
                         join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                         join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals ilabel.IndicatorLabelID
                         where iitems.IndicatorID == r.ProjectID && ilabel.LabelType != 2
                         select new
                         {
                             ProjectID = r.ProjectID,
                             IndicatorID = iitems.IndicatorID,
                             IndicatorLabelID = iitems.IndicatorLabelID,
                             IndicatorLabelText = ilabel.Text,
                             IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                             ToCount = iitems.Final - iitems.Baseline,
                             CodeText = r.Organization.Addresses.FirstOrDefault().RegionList.DDNAME,
                             ProgramAreaText = r.ProgramArea.ProgramAreaList.ProgramAreaText
                         }).ToList();


           var query = indics
              .GroupBy(g => new //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              {
                  g.CodeText,
                  g.ProgramAreaText
              })
              .Select(group => new IndRepHolder() //Select all Grouped into VersusContainer.
              {
                  Column = group.Key.CodeText,
                  Row = group.Key.ProgramAreaText,
                  Val = group.Sum(s => s.ToCount).Value
              });

           return query.ToList();

       }

    
       //Indicators Reports ...
       public List<LabelByCount> IndicatorByContentCategory(IQueryable<Project> result, int contentCategory)
       {

           //====== Knowledge type Final/Amount of Projects, where Indicator > 0
           if (contentCategory == 8)   
           {
               var IndicatorByBase = (from i in db.Indicators
                                      from r in result
                                      join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                                      join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals
                                          ilabel.IndicatorLabelID
                                      join iContentLabel in db.IndicatorLabelContentCategories on
                                          ilabel.LabelContentCategory equals iContentLabel.ID
                                      where iitems.IndicatorID == r.ProjectID && iContentLabel.ID == contentCategory
                                      select new
                                      {
                                          ProjectID = r.ProjectID,
                                          IndicatorID = iitems.IndicatorID,
                                          IndicatorLabelID = iitems.IndicatorLabelID,
                                          IndicatorLabelText = ilabel.Text,
                                          IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                                          ToCount = iitems.Final,// - iitems.Baseline,
                                          //TODO:  this is asked in Reports form double Check!
                                          LabelContentCategory = iContentLabel.ID,
                                     
                                          // labelOrder = ilabel.LabelOrder
                                      }).ToList();


               var groupSum = IndicatorByBase.GroupBy(ibase => ibase.IndicatorLabelID)
                   .Select(reg => new LabelByCount()
                   {  //=========================== password toBota: KmkggY4r===========================================
                       Counted = reg.Sum(p => p.ToCount).Value / (reg.Count(d => d.ToCount>0)> 0 ? reg.Count(d => d.ToCount>0) : 1),  //Sum all Indicator Vals, divide by number of Projects where indicator val >0  
                       //... if else prevents for 0 division.
                       //=======================================================================
                       //TODO:summing is happening here.
                       Label = reg.FirstOrDefault().IndicatorLabelText,
                       GrantTypeCodeID = reg.FirstOrDefault().IndicatorLabelGranTypeCodeID,
                       LabelContentCategoryID = reg.FirstOrDefault().LabelContentCategory,
                       LabelID = reg.Key,
                       //labelOrder = reg.Select(l => l.labelOrder)
                   });

               List<LabelByCount> listtest = groupSum.ToList();
               // return groupSum.OrderBy(gt => gt.GrantTypeCodeID).ToList(); //order the list by GrantType.          
               return groupSum.ToList(); //order the list by GrantType.
           }


           //=================== service type of indicators need Final only.
           if (contentCategory == 6)   
           {
               var IndicatorByBase = (from i in db.Indicators
                                      from r in result
                                      join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                                      join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals
                                          ilabel.IndicatorLabelID
                                      join iContentLabel in db.IndicatorLabelContentCategories on
                                          ilabel.LabelContentCategory equals iContentLabel.ID
                                      where iitems.IndicatorID == r.ProjectID && iContentLabel.ID == contentCategory
                                      select new
                                      {
                                          ProjectID = r.ProjectID,
                                          IndicatorID = iitems.IndicatorID,
                                          IndicatorLabelID = iitems.IndicatorLabelID,
                                          IndicatorLabelText = ilabel.Text,
                                          IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                                          ToCount = iitems.Final,// - iitems.Baseline,
                                          //TODO:  this is asked in Reports form double Check!
                                          LabelContentCategory = iContentLabel.ID,
                                          // labelOrder = ilabel.LabelOrder
                                      }).ToList();

               var groupSum = IndicatorByBase.GroupBy(ibase => ibase.IndicatorLabelID)
                   .Select(reg => new LabelByCount()
                   {
                       Counted = reg.Sum(p => p.ToCount).Value,
                       //TODO:summing is happening here.
                       Label = reg.FirstOrDefault().IndicatorLabelText,
                       GrantTypeCodeID = reg.FirstOrDefault().IndicatorLabelGranTypeCodeID,
                       LabelContentCategoryID = reg.FirstOrDefault().LabelContentCategory,
                       LabelID = reg.Key,
                       //labelOrder = reg.Select(l => l.labelOrder)
                   });

               // return groupSum.OrderBy(gt => gt.GrantTypeCodeID).ToList(); //order the list by GrantType.          
               return groupSum.ToList(); //order the list by GrantType.
           }
           else
           {
               //Get the List of All We need! see select new
               var IndicatorByBase = (from i in db.Indicators
                                      from r in result
                                      join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                                      join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals
                                          ilabel.IndicatorLabelID
                                      join iContentLabel in db.IndicatorLabelContentCategories on
                                          ilabel.LabelContentCategory equals iContentLabel.ID
                                      where iitems.IndicatorID == r.ProjectID && iContentLabel.ID == contentCategory
                                      select new
                                                 {
                                                     ProjectID = r.ProjectID,
                                                     IndicatorID = iitems.IndicatorID,
                                                     IndicatorLabelID = iitems.IndicatorLabelID,
                                                     IndicatorLabelText = ilabel.Text,
                                                     IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                                                     ToCount = iitems.Final - iitems.Baseline,
                                                     //TODO:  this is asked in Reports form double Check!
                                                     LabelContentCategory = iContentLabel.ID
                                                 }).ToList();

               var groupSum = IndicatorByBase.GroupBy(ibase => ibase.IndicatorLabelID)
                   .Select(reg => new LabelByCount()
                                      {
                                          Counted = reg.Sum(p => p.ToCount).Value,
                                          //TODO:summing is happening here.
                                          Label = reg.FirstOrDefault().IndicatorLabelText,
                                          GrantTypeCodeID = reg.FirstOrDefault().IndicatorLabelGranTypeCodeID,
                                          LabelContentCategoryID = reg.FirstOrDefault().LabelContentCategory,
                                          LabelID = reg.Key
                                      });

               // return groupSum.OrderBy(gt => gt.GrantTypeCodeID).ToList(); //order the list by GrantType.          
               return groupSum.ToList(); //order the list by GrantType.
           }
          
                    

       }


       //Indicators Reports ...
       public List<LabelByCount> IndicatorByBaseline(IQueryable<Project> result)
       {   
          //Get the List of All We need! see select new
           var IndicatorByBase = from i in db.Indicators
                                 from r in result
                                 join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                                 join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals ilabel.IndicatorLabelID
                                 where iitems.IndicatorID==r.ProjectID
                                 select new { IndicatorID = iitems.IndicatorID,
                                              IndicatorLabelID = iitems.IndicatorLabelID, 
                                              IndicatorLabelText = ilabel.Text, 
                                              IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                                              ToCount = iitems.Baseline};        

           var groupSum = IndicatorByBase.GroupBy(ibase => ibase.IndicatorLabelText)
                                .Select (reg => new LabelByCount()
                                {
                                   Counted = reg.Sum(p => p.ToCount).Value,
                                   Label = reg.Key,                                   
                                   GrantTypeCodeID = reg.FirstOrDefault().IndicatorLabelGranTypeCodeID
                                });

           return groupSum.OrderBy(gt=>gt.GrantTypeCodeID).ToList(); //order the list by GrantType.          
   
       }

       public List<LabelByCount> IndicatorByBenchmark(IQueryable<Project> result)
       {
           //Get the List of All We need! see select new
           var IndicatorByBase = from i in db.Indicators
                                 from r in result
                                 join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                                 join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals ilabel.IndicatorLabelID
                                 where iitems.IndicatorID == r.ProjectID
                                 select new
                                 {
                                     IndicatorID = iitems.IndicatorID,
                                     IndicatorLabelID = iitems.IndicatorLabelID,
                                     IndicatorLabelText = ilabel.Text,
                                     IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                                     ToCount = iitems.Benchmark
                                 };

           var groupSum = IndicatorByBase.GroupBy(ibase => ibase.IndicatorLabelText)
                                .Select(reg => new LabelByCount()
                                {
                                    Counted = reg.Sum(p => p.ToCount).Value,
                                    Label = reg.Key,
                                    GrantTypeCodeID = reg.FirstOrDefault().IndicatorLabelGranTypeCodeID
                                });

           return groupSum.OrderBy(gt => gt.GrantTypeCodeID).ToList();

       }


       public List<LabelByCount> IndicatorByFinal(IQueryable<Project> result)
       {
           //Get the List of All We need! see select new
           var IndicatorByBase = from i in db.Indicators
                                 from r in result
                                 join iitems in db.IndicatorItems on i.IndicatorID equals iitems.IndicatorID
                                 join ilabel in db.IndicatorLabels on iitems.IndicatorLabelID equals ilabel.IndicatorLabelID
                                 where iitems.IndicatorID == r.ProjectID
                                 select new
                                 {
                                     IndicatorID = iitems.IndicatorID,
                                     IndicatorLabelID = iitems.IndicatorLabelID,
                                     IndicatorLabelText = ilabel.Text,
                                     IndicatorLabelGranTypeCodeID = ilabel.GrantTypeCodeID,
                                     ToCount = iitems.Final
                                 };

           var groupSum = IndicatorByBase.GroupBy(ibase => ibase.IndicatorLabelText)
                                .Select(reg => new LabelByCount()
                                {
                                    Counted = reg.Sum(p => p.ToCount).Value,
                                    Label = reg.Key,
                                    GrantTypeCodeID = reg.FirstOrDefault().IndicatorLabelGranTypeCodeID
                                });

           return groupSum.OrderBy(gt => gt.GrantTypeCodeID).ToList();

       }

       #endregion 


       #region  ------- Fin Reports ... ----------
       //Report 1
       public List<RequestedAndAwardedAmountStruct> RequestedAndAwardedAmountByRegion(IQueryable<Project> result)
       {          

           //1.Groups By Region,/ Counts how many in each Group./ Sums Amts in each Group.
           var RegionGroup = result.GroupBy(ra => ra.Organization.Addresses.FirstOrDefault().RegionList.DDNAME)
                            .Select(reg => new RequestedAndAwardedAmountStruct()
                            {
                                Region = reg.Key,
                                NumberofAllProjects = reg.Count(),
                                NumberofActiveProjects = reg.Count(p => p.ProposalStatus.ProposalStatusList.ProposalStatusText=="Active"),
                                NumberofProposals = reg.Count(p => p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Proposal"),
                                RequestedAmt = reg.Sum(reqamt=>reqamt.ProjectInfo.AmtRequested).Value,
                                AwarderAmt = reg.Sum(reqamt=>reqamt.ProjectInfo.AwardedAmt).Value
                            });

           try
           {
               var MatchList2 = RegionGroup.ToList();
               return MatchList2; 
           }
           catch
           {
               return null; 
           }
           
        
       }



       //Report 2
       public List<RequestedAmountByX> RequestedAmountByArea(IQueryable<Project> result)
       {
           //1.Groups By Region,/ Counts how many in each Group./ Sums Amts in each Group.
           //var RegionGroup = result.GroupBy(ra => ra.Organization.Addresses.FirstOrDefault().RegionList.DDNAME)
           
           //filter out to only Grants.
           result = result.Where(p => p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Active" ||
                                 p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Closed");

           var RegionGroup = result.GroupBy(ra => ra.ProgramArea.ProgramAreaList.ProgramAreaText)
                            .Select(reg => new RequestedAmountByX()
                            {
                                Type = reg.Key,
                                Amount = reg.Sum(reqamt => reqamt.ProjectInfo.AmtRequested).Value,
                                Average = (int)reg.Average(reqamt => reqamt.ProjectInfo.AmtRequested).Value,
                                MinimumAmount = reg.Min(reqamt => reqamt.ProjectInfo.AmtRequested).Value,
                                MaximumAmount = reg.Max(reqamt => reqamt.ProjectInfo.AmtRequested).Value,
                                NumberOfGrants = reg.Count()
                            });

           try
           {
               var MatchList2 = RegionGroup.ToList();
               return MatchList2; 
           }
           catch
           {
               return null; 
           }

       }

       //Report 3
       public List<RequestedAmountByX> RequestedAmountByGrantType(IQueryable<Project> result)
       {
           //1.Groups By Region,/ Counts how many in each Group./ Sums Amts in each Group.
           //var RegionGroup = result.GroupBy(ra => ra.Organization.Addresses.FirstOrDefault().RegionList.DDNAME)

           //filter out to only Grants.
           result = result.Where(p => p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Active" ||
                                 p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Closed");

           var RegionGroup = result.GroupBy(ra => ra.GrantType.GrantTypeList.GrantTypeText)
                            .Select(reg => new RequestedAmountByX()
                            {
                                Type = reg.Key,
                                Amount = reg.Sum(reqamt => reqamt.ProjectInfo.AmtRequested).Value,
                                Average = (int)reg.Average(reqamt => reqamt.ProjectInfo.AmtRequested).Value,
                                MinimumAmount = reg.Min(reqamt => reqamt.ProjectInfo.AmtRequested).Value,
                                MaximumAmount = reg.Max(reqamt => reqamt.ProjectInfo.AmtRequested).Value,
                                NumberOfGrants = reg.Count()
                            });


           try
           {
               var MatchList2 = RegionGroup.ToList();
               return MatchList2;
           }
           catch
           {
               return null;
           }

          

       }

       //Report 4 
       public List<RequestedAmountByX> AwardedAmountByArea(IQueryable<Project> result)
       {
           //1.Groups By Region,/ Counts how many in each Group./ Sums Amts in each Group.
           //var RegionGroup = result.GroupBy(ra => ra.Organization.Addresses.FirstOrDefault().RegionList.DDNAME)

           //filter out to only Grants.
           result = result.Where(p => p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Active" ||
                                 p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Closed");

           var RegionGroup = result.GroupBy(ra => ra.ProgramArea.ProgramAreaList.ProgramAreaText)
                            .Select(reg => new RequestedAmountByX()
                            {
                                Type = reg.Key,
                                Amount = reg.Sum(reqamt => reqamt.ProjectInfo.AwardedAmt).Value,
                                Average = (int)reg.Average(reqamt => reqamt.ProjectInfo.AwardedAmt).Value,
                                MinimumAmount = reg.Min(reqamt => reqamt.ProjectInfo.AwardedAmt).Value,
                                MaximumAmount = reg.Max(reqamt => reqamt.ProjectInfo.AwardedAmt).Value,
                                NumberOfGrants = reg.Count()
                            });


           try
           {
               var MatchList2 = RegionGroup.ToList();
               return MatchList2;
           }
           catch
           {
               return null;
           }

       }

       //Report 5
       public List<RequestedAmountByX> AwardedAmountByGrantType(IQueryable<Project> result)
       {
           //1.Groups By Region,/ Counts how many in each Group./ Sums Amts in each Group.
           //var RegionGroup = result.GroupBy(ra => ra.Organization.Addresses.FirstOrDefault().RegionList.DDNAME)

           //filter out to only Grants.
           result = result.Where(p => p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Active" ||
                                 p.ProposalStatus.ProposalStatusList.ProposalStatusText == "Closed");

           var RegionGroup = result.GroupBy(ra => ra.GrantType.GrantTypeList.GrantTypeText)
                            .Select(reg => new RequestedAmountByX()
                            {
                                Type = reg.Key,
                                Amount = reg.Sum(reqamt => reqamt.ProjectInfo.AwardedAmt).Value,
                                Average = (int)reg.Average(reqamt => reqamt.ProjectInfo.AwardedAmt).Value,
                                MinimumAmount = reg.Min(reqamt => reqamt.ProjectInfo.AwardedAmt).Value,
                                MaximumAmount = reg.Max(reqamt => reqamt.ProjectInfo.AwardedAmt).Value,
                                NumberOfGrants = reg.Count()
                            });


           try
           {
               var MatchList2 = RegionGroup.ToList();
               return MatchList2;
           }
           catch
           {
               return null;
           }

       }


       public IQueryable<Project> GetResults(FinReportFilter frepf)
       {

        //   BOTADataContext db = new BOTADataContext(connectString);
           IQueryable<Project> matches = db.Projects;
          // IQueryable<FinReportFilter> result = null; 

           if (frepf.isAmountRequested.Value)
           {
               if (frepf.RequestedDateStart.HasValue && frepf.RequestedDateEnd.HasValue)
               {
                   matches = matches.Where(a => a.ProposalInfo.RequestDate >= frepf.RequestedDateStart 
                                            && a.ProposalInfo.RequestDate <= frepf.RequestedDateEnd);
               }             

           }

       //    var MatchList2 = matches.ToList(); 
            //people.Join(pets,
            //    person => person,
            //    pet => pet.Owner,
            //    (person, pet) =>
            //        new { OwnerName = person.Name, Pet = pet.Name }).ToList();

           if (frepf.isOrganizationName.Value)
           { 
              //join the project with organization where organization has projectID.
                matches.Join(matches,
                            project => project,
                            organization => organization,
                            (project, organization) =>
                             new { ProjectID = project.ProjectID, OrgPrjectID = organization.ProjectID });
           
           }


           matches = matches.Where(a => a.isDeleted.Value.Equals(null));

      //     var MatchList = matches.ToList();
           

           return matches;
       }

       public IQueryable<Project> GetResults4(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
          List<String> status, List<String> period, List<String> indicatorCategory, List<String> location)
       {
           return ReportPredicates.GetResults4(frepf, ID, Area, gtype, compete, status, period, indicatorCategory, location);
       }

       public IQueryable<Project> GetResults3(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> period, List<String> oblast, List<String> amount)
       {
           return ReportPredicates.GetResults3(frepf, ID, Area, gtype, compete, status, period, oblast, amount, null);
       }

       public IQueryable<Project> GetResults3(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
           List<String> status, List<String> period, List<String> oblast, List<String> amount, List<String> lfIndicator)
       {
           return ReportPredicates.GetResults3(frepf, ID, Area, gtype, compete, status, period, oblast, amount, lfIndicator);
       }
    
         public IQueryable<Project> GetResults2(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> oblast, List<String> period, List<String> amount, List<String> location)
         {
             return ReportPredicates.GetResults2(frepf, ID, Area, gtype, compete, status, oblast, period, amount, location);
         }
    }

       #endregion 

    

    }





