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
   public class BudgetRepository
    {

       private string connectString;
       private ISqlCommands sqlcmd;

       public BudgetRepository()
       {
            //string connString = Settings.Default.EntityConnection; 
          //  Context = new BOTADBEntities1();
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();
           sqlcmd = SqlFactory.MSSQL();

       }

       #region Report Periods
       public decimal? BudgetAwardedbyCatSummed(int PrjID)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = (from i in db.Projects
                         join budg in db.Budgets on i.ProjectID equals budg.BudgetID
                         join farts in db.FinArticleCategoryRs on budg.BudgetID equals farts.BudgetID
                         where i.ProjectID == PrjID
                         select new
                         {
                             ProjID = i.ProjectID,
                             CatID = farts.FinCatID,
                             Awarded = farts.Price,  // - budg.Returned
                             //  SumByCatTransfered = farts.ReportPeriodRs.Sum(a => a.Amount),
                             //   gKind = i.ProgramArea.ProgramAreaCodeID
                         }).Distinct().ToList();  //in some projects i got duplicates!!!
           
           return result.Any() ? result.Sum(s => s.Awarded) : 0;
       }

       public IEnumerable<ReportPeriodListR> GetFinPeriods(int BudgetID)
       {

           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.ReportPeriodListRs
                        where ls.BudgetID == BudgetID
                        select ls;
           return result;

       }



       /// <summary>
       /// Updates Report Periods.
       /// </summary>
       /// <param name="repperlist"></param>
       /// <returns></returns>
       public bool UpdateReportPeriodList(List<ReportPeriodListR> repperlist)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {

               db.ReportPeriodListRs.AttachAll(repperlist);
               db.Refresh(RefreshMode.KeepCurrentValues, repperlist);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }



       public bool DeleteReportPeriod(int RepperID, int BudgetID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from r in db.ReportPeriodListRs
                               where r.ReportPeriodID == RepperID && r.BudgetID == BudgetID
                               select r).First();
               db.ReportPeriodListRs.DeleteOnSubmit(toDelete);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }

       public bool CreateReportPeriod(ReportPeriodListR item)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           bool result = true;
           try
           {
               db.ReportPeriodListRs.InsertOnSubmit(item);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               result = false;
           }
           return result;
       }


       public void UpdateReportPeriodTrans(List<ReportPeriodR> ReportPeriods)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               /*var oldItem  = (from p in db.ProjectEvents
                              where p.EventID == item.EventID
                              select p).First();*/
               db.ReportPeriodRs.AttachAll(ReportPeriods);
               db.Refresh(RefreshMode.KeepCurrentValues, ReportPeriods);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //   result = false;
           }

           //return result;
       }


       #endregion 

       public bool UpdateBudgetInitialAmount(int initialamt, int budgetID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               Budget budget = (from b in db.Budgets
                                where b.BudgetID == budgetID
                                select b).FirstOrDefault();

               budget.ContractInitialAmt = initialamt;

               //db.Budgets.Attach();
               db.Refresh(RefreshMode.KeepCurrentValues, budget);
               db.SubmitChanges();
               return true;
           }
           catch (Exception ex)
           {
               return false;
           }

           //return result;
       }


          

       /// <summary>
       /// Drop Down List.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public IEnumerable<FinArtCatListR> GetCatList()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = (from ls in db.FinArtCatListRs
                        select ls).OrderBy(c=>c.FinArticleCatListID);
           return result;
       }


       public void InsertNewCatName(string NewCatName)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           FinArtCatListR fcatlist = new FinArtCatListR();
           fcatlist.FinArticleCatName = NewCatName;
          
           try
           {
               db.FinArtCatListRs.InsertOnSubmit(fcatlist);
               db.SubmitChanges();
           }

           catch (Exception ex)
           {

           }

       }


       #region Financial Articles ....


       /* 
        SELECT     ReportPeriodListR.PeriodStart, ReportPeriodListR.PeriodEnd, ReportPeriodListR.PeriodTitle, ReportPeriodR.Amount, FinArticleCategoryR.Price, 
                      FinArticleCategoryR.TransferAmt, FinArticleCategoryR.FinArticleCatListID, FinArticleCategoryR.FinArticleCatID, FinArticleCategoryR.BudgetID
FROM         FinArticleCategoryR INNER JOIN
                      ReportPeriodR ON FinArticleCategoryR.FinArticleCatID = ReportPeriodR.FinArticleCatID INNER JOIN
                      ReportPeriodListR ON ReportPeriodR.ReportPeriodID = ReportPeriodListR.ReportPeriodID
WHERE     (FinArticleCategoryR.BudgetID = 1)
        */

       public Budget GetBudget(int BudgetID)
       {

           BOTADataContext db = new BOTADataContext(connectString);
           Budget budget = (from b in db.Budgets
                            where b.BudgetID == BudgetID
                            select b).FirstOrDefault();
           return budget;
       }


       public int GetBudgetInitialAmount(int BudgetID)
       {

           BOTADataContext db = new BOTADataContext(connectString);
           
           var budgetinitialamt = (from b in db.Budgets
                            where b.BudgetID == BudgetID
                            select b.ContractInitialAmt).FirstOrDefault();

           if (budgetinitialamt != null)
               return budgetinitialamt.Value;
           else
               return 0; 
           
       }



       public IEnumerable<FinArticleCategoryR> GetFinArticleCategory(int BudgetID)
       {

           BOTADataContext db = new BOTADataContext(connectString);
           var result = (from fincategs in db.FinArticleCategoryRs
                        join r in db.ReportPeriodRs on fincategs.FinArticleCatID equals r.FinArticleCatID
                        join rlist in db.ReportPeriodListRs on r.ReportPeriodID equals rlist.ReportPeriodID
                        where fincategs.BudgetID == BudgetID
                        select fincategs).Distinct().OrderBy(fcat=>fcat.FinArticleCatID);   //because of multip reports periods...dubplicates are returned.
           return result;
       }


       public bool InsertArticleToReportPeriodR(List<ReportPeriodR> ReportPeriods)
       {
           BOTADataContext db = new BOTADataContext(connectString);
        
           try
           {
               db.ReportPeriodRs.InsertAllOnSubmit(ReportPeriods.AsEnumerable());
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return false;  //could not insert into Report Periods.
           }

           return true; 
       
       }

     

       public int AddFinArticleCategory(int id, int FinCatSel)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           //Check if Any Report Periods Exist! 
           if (!db.ReportPeriodListRs.Any(l => l.BudgetID == id))  // !if exists. 
           {
               return -2; //this means no ReportPeriods created yet.

           }          


           FinArticleCategoryR fcat = new FinArticleCategoryR();
        
           //  fcat.FinArticleCatListID = FinCatSel;

           FinArtCatListR fcatr = (from ls in db.FinArtCatListRs
                                  where ls.FinArticleCatListID == FinCatSel
                                  select ls).FirstOrDefault();

           fcat.FinArticleCatText = fcatr.FinArticleCatName; 
           fcat.BudgetID = id;
           fcat.Price = 0;
           fcat.FinCatID = FinCatSel; 

           try
           {
               db.FinArticleCategoryRs.InsertOnSubmit(fcat);
               db.SubmitChanges();
           }

           catch(Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return -1;  //could not insert new Category somehow.
           }


           try
           {
               IEnumerable<ReportPeriodListR> ReportPeriodsList = from ls in db.ReportPeriodListRs
                                                                  where ls.BudgetID == id
                                                                  select ls;

               List<ReportPeriodR> ReportPeriods = new List<ReportPeriodR>();

               foreach (ReportPeriodListR rep in ReportPeriodsList)
               {
                   ReportPeriodR repper = new ReportPeriodR();
                   repper.ReportPeriodID = rep.ReportPeriodID;
                   repper.FinArticleCatID = fcat.FinArticleCatID;
                   repper.Amount = 0;
                   ReportPeriods.Add(repper);
               }


               db.ReportPeriodRs.InsertAllOnSubmit(ReportPeriods.AsEnumerable());
               db.SubmitChanges();
           }
           catch(Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectRepository), "Exception catched while Insert Project.", ex);
               return -3;  //could not insert into Report Periods.
           
           
           }

           return fcat.FinArticleCatID; 


       }
    

       public bool DeleteArticleCat(int? FinArticleCatID, int? BudgetID)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from c in db.FinArticleCategoryRs
                               where c.BudgetID == BudgetID.Value && c.FinArticleCatID == FinArticleCatID
                               select c).First();
               db.FinArticleCategoryRs.DeleteOnSubmit(toDelete);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;


       }


       public void UpdateBudget(int id, string infoBox, Decimal Cancellation, Decimal Returned)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
          
           try
           {
               var budgetToUpdate  = (from b in db.Budgets
                              where b.BudgetID == id 
                              select b).First();

               if (budgetToUpdate != null)
               {
                   budgetToUpdate.InfoBox = infoBox;
                   budgetToUpdate.Cancellation = Cancellation;
                   budgetToUpdate.Returned = Returned; 

                  // db.Budgets.Attach(budgetToUpdate);
                   db.Refresh(RefreshMode.KeepCurrentValues, budgetToUpdate);
                   db.SubmitChanges();
               }
           }
           catch (Exception ex)
           {
               //   result = false;
           }

           //return result;
       }



       public void UpdateCategoriesValues(List<FinArticleCategoryR> FinArticleCategory)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               /*var oldItem  = (from p in db.ProjectEvents
                              where p.EventID == item.EventID
                              select p).First();*/
               db.FinArticleCategoryRs.AttachAll(FinArticleCategory);
               db.Refresh(RefreshMode.KeepCurrentValues, FinArticleCategory);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               //   result = false;
           }

           //return result;
       }


       #endregion 


    }
}
