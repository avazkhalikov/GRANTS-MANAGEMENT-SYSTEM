using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.DataAccess;
using StructureMap;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.Impl;

namespace BOTACORE.CORE.Services.Impl
{
   public class BudgetService
    {
    
      BudgetRepository budrep;
      public BudgetService()
      {
          budrep = new BudgetRepository();
      }


      public decimal? BudgetAwardedbyCatSummed(int ProjID)
      {
          if (budrep.BudgetAwardedbyCatSummed(ProjID) != null && budrep.BudgetAwardedbyCatSummed(ProjID).HasValue)
          {
              return budrep.BudgetAwardedbyCatSummed(ProjID);
          }
          else
          {
              return 0;
          }

          
      }

      public Budget GetBudget(int BudgetID)
      {
          return budrep.GetBudget(BudgetID);
      
      }

      public int GetBudgetInitialAmount(int BudgetID)
      {

          return budrep.GetBudgetInitialAmount(BudgetID);
          
      
      }

       

       #region Report Periods
       public IEnumerable<ReportPeriodListR> GetFinPeriods(int BudgetID)
       {

           
           return budrep.GetFinPeriods(BudgetID);

       }
       

       /// <summary>
       /// Updates Report Periods.
       /// </summary>
       /// <param name="repperlist"></param>
       /// <returns></returns>
       public bool UpdateReportPeriodList(List<ReportPeriodListR> repperlist)
       {
           return budrep.UpdateReportPeriodList(repperlist);
       }


       public bool CreateReportPeriod(ReportPeriodListR item)
       {
           //     Insert must not work if any article exists in this Budget.

       //    return budrep.CreateReportPeriod(item);

           if (GetFinArticleCategory(item.BudgetID).Count() == 0)  //If No BUDGET
           {
               return budrep.CreateReportPeriod(item);
           }
           else  //IF BUDGET EXISTS
           {
               if (budrep.CreateReportPeriod(item)) //if success.
               { //
                   IEnumerable<FinArticleCategoryR> fincats = GetFinArticleCategory(item.BudgetID);
                   List<ReportPeriodR> ReportPeriods = new List<ReportPeriodR>();

                   foreach (FinArticleCategoryR fincat in fincats)
                   {   //insert article itself.
                       
                       ReportPeriodR repper = new ReportPeriodR();
                       repper.ReportPeriodID = item.ReportPeriodID;
                       repper.FinArticleCatID = fincat.FinArticleCatID;
                       repper.Amount = 0; 
                       ReportPeriods.Add(repper); 

                   }

                  return  budrep.InsertArticleToReportPeriodR(ReportPeriods);  

               }

               return false; 
            }

       }


       public bool DeleteReportPeriod(int RepperID, int BudgetID)
       {


           return budrep.DeleteReportPeriod(RepperID, BudgetID);
       }


       public void UpdateReportPeriodTrans(List<ReportPeriodR> ReportPeriods)
       {
           budrep.UpdateReportPeriodTrans(ReportPeriods);
       }


       #endregion 

       public bool UpdateBudgetInitialAmount(int initialamt, int budgetID)
       {
        
          return budrep.UpdateBudgetInitialAmount(initialamt,  budgetID);
         
       }
                 

       /// <summary>
       /// Drop Down List.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public IEnumerable<FinArtCatListR> GetCatList()
       {

           return budrep.GetCatList();
       }


       public void InsertNewCatName(string NewCatName)
       {

           budrep.InsertNewCatName(NewCatName); 
       }


       #region Financial Articles ....

       public IEnumerable<FinArticleCategoryR> GetFinArticleCategory(int BudgetID)
       {
              return budrep.GetFinArticleCategory(BudgetID);
       }



       public int AddFinArticleCategory(int id, int FinCatSel)
       {

          return budrep.AddFinArticleCategory(id, FinCatSel); 
       }
    

       public bool DeleteArticleCat(int? FinArticleCatID, int? BudgetID)
       {


           return budrep.DeleteArticleCat(FinArticleCatID, BudgetID);


       }


       public void UpdateCategoriesValues(List<FinArticleCategoryR> FinArticleCategory)
       {
           budrep.UpdateCategoriesValues(FinArticleCategory); 
       }

       public void UpdateBudget(int id, string infoBox, Decimal Cancellation, Decimal Returned)
       {
           budrep.UpdateBudget(id, infoBox, Cancellation, Returned);
       }

       


       #endregion 


    }
}



