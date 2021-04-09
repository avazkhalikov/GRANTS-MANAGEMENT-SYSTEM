using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.Services.Impl
{
    public class FinanceResults
    {
        BudgetService budservice;
        IEnumerable<ReportPeriodListR> reppers;
        BudgetAggregateService aggrv2;
        CatRepView catrepview;
        BudgetPaymentReportService budpayreportservice;
        List<BudgetPaymentReport> bpayrep;
        IEnumerable<FinArticleCategoryR> _finartcats;
       
        public FinanceResults(int ProjID)
        {
            try
            {
                budservice = new BudgetService();
                reppers = budservice.GetFinPeriods(ProjID);
                _finartcats = budservice.GetFinArticleCategory(ProjID); 
                //We aggregate the budget/transactions.
                aggrv2 = new BudgetAggregateService(_finartcats.ToList(), reppers.ToList());
                catrepview = aggrv2.AccumulateCatRep();
                budpayreportservice = new BudgetPaymentReportService(reppers.ToList(), catrepview);
                bpayrep = budpayreportservice.GenerateReports2(0);               

            }
            catch
            { 
            
            
            }
                
        }

        //empty contrusctor;
        public FinanceResults()
        {
            budservice = new BudgetService();
        }

        public decimal? BudgetAwardedbyCatSummed(int ProjID)
        {
             if(budservice.BudgetAwardedbyCatSummed(ProjID) !=null && budservice.BudgetAwardedbyCatSummed(ProjID).HasValue)
             {
                 if(ProjID == 31)
                 {
                     int hello = 0; 
                 }
                 return budservice.BudgetAwardedbyCatSummed(ProjID);
             }
             else
             {
                 return 0; 
             }
        }


        public void Initialize(int ProjID)
        {
            try
            {
                budservice = new BudgetService();
                reppers = budservice.GetFinPeriods(ProjID);
                _finartcats = budservice.GetFinArticleCategory(ProjID);
                //We aggregate the budget/transactions.
                aggrv2 = new BudgetAggregateService(_finartcats.ToList(), reppers.ToList());
                catrepview = aggrv2.AccumulateCatRep();
                budpayreportservice = new BudgetPaymentReportService(reppers.ToList(), catrepview);
                bpayrep = budpayreportservice.GenerateReports2(0);

            }
            catch
            {


            }
        }

        /// <summary>
        /// Return Total Money Spent for all periods from Budget
        /// </summary>
        /// <param name="projID"></param>
        /// <returns></returns>
        public decimal Project_TotalMoneySpentAmountFromAwardAmount()
        {
            

            decimal TotalSpent = 0;

            try
            {
                foreach (BudgetPaymentReport bpr in bpayrep)
                {
                    TotalSpent = TotalSpent + bpr.MoneySpent;

                }

            }
            catch
            {
                TotalSpent = 0;
               
            }

           return TotalSpent; 

        }

        /// <summary>
        /// Amount of money recieved, but not used. 
        /// (may be used, but no report in transactions)
        /// They recieved the money, but no report!!
        /// --------Amount Left IN a Last Period. ----------------
        /// </summary>
        /// <returns></returns>
        public decimal Project_TotalCashOnHand()
        {
            decimal TotalLeft = 0;
            decimal Cancellation = 0;
            decimal Returned = 0; 
            try
            {
                if (bpayrep.Last().repper.Budget.Cancellation.HasValue)
                {
                    Cancellation = bpayrep.Last().repper.Budget.Cancellation.Value; 
                }
                
                if (bpayrep.Last().repper.Budget.Returned.HasValue)
                {
                    Returned = bpayrep.Last().repper.Budget.Returned.Value;
                }

                TotalLeft = bpayrep.Last().CalcBalance - Returned; 
            }
            catch
            {
                TotalLeft = 0;
            
            }



            return TotalLeft;
        }

        /// 
        /// <summary>
        /// 
        /// Return Total Amount Left from Total of Budget (Not in Transactions)
        /// This does not take into account if money already transfered!
        /// 
        /// </summary>
        /// <param name="projID"></param>
        /// <returns></returns>
        public decimal Project_TotalAmountLeftFromAwardAmount()
        {
            decimal TotalSpent = 0;
            decimal TotalLeft = 0;

            try
            {
                TotalSpent = Project_TotalMoneySpentAmountFromAwardAmount();

                TotalLeft = catrepview.CatRepSum - TotalSpent;
            }
            catch
            {
               TotalLeft = 0;
            }

            return TotalLeft;
        }


        /// <summary>
        /// Return Total Money Transfered for all periods.
        /// </summary>
        /// <param name="projID"></param>
        /// <returns></returns>
        public decimal Project_TotalMoneyTransferedFromAwardAmount()
        {
            decimal TotalGotMoney = 0;

            try
            {
                foreach (BudgetPaymentReport bpr in bpayrep)
                {
                    if (bpr.repper.PaymentStatus.Value != 1) //if status not pending but already transfered.
                    {
                        TotalGotMoney = TotalGotMoney + bpr.PeriodGotMoney;
                    }

                }

            }
            catch
            {
                TotalGotMoney = 0;

            }

            return TotalGotMoney; 
            
            
        }

        /// <summary>
        /// Return Total CashOnHand From Last tranche. 
        /// </summary>
        /// <param name="projID"></param>
        /// <returns></returns>
        public decimal Project_TotalCashOnHandOfLastTransferPeriod()
        {

            decimal TotalLeft = 0;

            try
            {
                TotalLeft = bpayrep.Last().cashOnHand;
            }
            catch
            {
                TotalLeft = 0;

            }



            return TotalLeft;
        }

        /// <summary>
        /// Returns the amount to be transfered for the given repper. 
        /// </summary>
        /// <param name="repper"></param>
        /// <returns></returns>
        public decimal Project_PeriodTrancheAmount(ReportPeriodR repper)
        {
            decimal PeriodTrancheAmount = 0;

            try
            {
                foreach (BudgetPaymentReport bpr in bpayrep)
                {
                    if (repper.ReportPeriodID == bpr.repper.ReportPeriodID)
                    {
                       
                        PeriodTrancheAmount = bpr.CalcMoneyToTransfer;
                    }

                }
            }
            catch
            {
                PeriodTrancheAmount = 0; 
            }

            return PeriodTrancheAmount;
        }



    }
}
