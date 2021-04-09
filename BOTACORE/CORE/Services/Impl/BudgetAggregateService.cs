using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOTACORE.CORE.Domain;
using System.Text;
using System.Data.Linq;


namespace BOTACORE.CORE.Services.Impl
{

    public class CatRepView
    {
        public decimal CatRepSum;
        public List<RepperView> repper;

        public CatRepView()
        {
            repper = new List<RepperView>();
        }
    }


    /// <summary>
    /// holds the repper view.
    /// </summary>
    public class RepperView
    {
        public int repperID;
        public decimal SumValue;
    }


   public class BudgetAggregateService
    {

        private List<CatRepView> catreplist;
        private List<FinArticleCategoryR> FinArticleCategoryRs;
        List<ReportPeriodListR> repperRs;

        public BudgetAggregateService(List<FinArticleCategoryR> _finartcats, List<ReportPeriodListR> _repperR)
        {
            catreplist = new List<CatRepView>();
            FinArticleCategoryRs = _finartcats;
            repperRs = _repperR; 
        }

      
       //This method returns Aggregated Report View Data. 
       public CatRepView AccumulateCatRep()
       {
           CatRepView catrepview = new CatRepView();

           decimal CatRepTotalCounter = 0;
           decimal transferAmt = 0;
           decimal PriceValue = 0; 

           foreach (FinArticleCategoryR finartcat in FinArticleCategoryRs)
           {
               if (finartcat.TransferAmt == null)
               {
                   transferAmt = 0;
               }
               else
               {
                   transferAmt = finartcat.TransferAmt.Value;
               }

               
               if (finartcat.Price.HasValue)   //just checking for null.
                   PriceValue = finartcat.Price.Value; 
               

               CatRepTotalCounter = CatRepTotalCounter + (PriceValue - transferAmt); 
           }

           catrepview.CatRepSum = CatRepTotalCounter;

           List<RepperView> repperList =  AccumulateReppers();

           catrepview.repper = repperList; 

            return catrepview;
        }



       public List<RepperView> AccumulateReppers()
       {
           decimal repperCount = 0;
           List<RepperView> repperViewList = new List<RepperView>(); 
           //from RepperList Get each ReportPeriodR.
           foreach (ReportPeriodListR repperListItem in repperRs)
           {
               foreach (ReportPeriodR repper in repperListItem.ReportPeriodRs)
               {
                   if(repper.Amount.HasValue)
                   repperCount = repperCount + repper.Amount.Value; 
               }

               RepperView repview = new RepperView();
               repview.repperID = repperListItem.ReportPeriodID;
               repview.SumValue = repperCount;
               repperViewList.Add(repview);
               repperCount = 0; 
           }

           return repperViewList; 
       }


       

    }
}
