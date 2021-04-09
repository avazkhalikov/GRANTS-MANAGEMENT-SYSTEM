using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOTACORE.CORE.Domain;
using System.Text;
using System.Data.Linq;


namespace BOTACORE.CORE.Services.Impl
{  


    /// <summary>
    /// All Classes used just to 
    /// hold the data (Struct), later to be sent to View...
    /// </summary>
    public class FinanceAggregateService
    {
        private List<CatRepViewHelper> catreplist;
        private Budget bud;

        public FinanceAggregateService(Budget _bud)
        {
            catreplist = new List<CatRepViewHelper>();
            bud = _bud;
        }


        public CatRepViewHelper GetAcumulatedFooter(List<CatRepViewHelper> acum, int reppersize)
        {
            CatRepViewHelper cumulative = new CatRepViewHelper();
            int i = -1;
            int size;

            //initialize cumulative repper.
            for (size = 0; size < reppersize; size++)
            {
                RepperViewHelper cumrepper = new RepperViewHelper();
                cumrepper.SumValue = 0;
                cumulative.repper.Add(cumrepper);
            }


            foreach (CatRepViewHelper catrepview in acum)
            {
                cumulative.CatSumValue = cumulative.CatSumValue + catrepview.CatSumValue;
             
                //this works if all list item in repper list has the same size. quadratik list.
                foreach (RepperViewHelper repper in catrepview.repper)
                {
                    i++;
                    cumulative.repper[i].SumValue = cumulative.repper[i].SumValue + repper.SumValue;

                }
                // }

                //clear index.
                i = -1;
            }

            return cumulative;
        }



        public List<CatRepViewHelper> GetView(int reppersize)
        {
            int ArticleTotal = 0;
            int CatSumValue = 0;
            int i = -1;
            int size;



            foreach (FinArticleCategory cat in bud.FinArticleCategories)
            {

                //initialize cumulative repper.
                List<RepperViewHelper> reppers = new List<RepperViewHelper>();
                for (size = 0; size < reppersize; size++)
                {
                    RepperViewHelper repperag = new RepperViewHelper();
                    repperag.SumValue = 0;
                    reppers.Add(repperag);
                }


                foreach (FinancialArticle finarticle in cat.FinancialArticles)
                {

                    i = -1;

                    ArticleTotal = finarticle.Price.Value * finarticle.Amt.Value * finarticle.Times.Value * finarticle.TimePeriod.Value + finarticle.TransferAmt.Value;
                    CatSumValue = CatSumValue + ArticleTotal;

                    foreach (ReportPeriod repper in finarticle.ReportPeriods)
                    {
                        //3 rep periods loop here.
                        i++;
                        //reppers[0] = repper[0] + repper.amount 
                        //reppers[1] = repper[1] + repper.amount 
                        reppers[i].SumValue = reppers[i].SumValue + repper.Amount.Value;
                    }
                }

                CatRepViewHelper catrep = new CatRepViewHelper();
                List<RepperViewHelper> subreppers = new List<RepperViewHelper>();
                catrep.CatName = cat.FinArtCatList.FinArticleCatName;
                catrep.CatSumValue = CatSumValue;


                //reppers.Clear() clears the resulting subreppers too, 
                //that is why i add each item in foreach.
                foreach (RepperViewHelper repperitem in reppers)
                {
                    subreppers.Add(repperitem);
                }
                catrep.repper = subreppers;
                catreplist.Add(catrep);

                //clear all for next category.
                ArticleTotal = 0;
                CatSumValue = 0;
                i = -1;
                reppers.Clear();
            }

            return catreplist;

        }


        #region  GetView()
        /// <summary>
        /// returns the result view. 
        /// Accumulates the Article amount for each Category
        /// Accumulates the each report period seperately for each Category.
        /// reppers[0] = repper[0] + repper.amount 
        /// reppers[1] = repper[1] + repper.amount 
        /// </summary>
        /// <returns>List<CatRepViewHelper></returns>
        public List<CatRepViewHelper> GetView()
        {
            int ArticleTotal = 0;
            int CatSumValue = 0;
            int i = -1;
            int ReportPeriodLength = 0;
            int Length = 0;

            List<RepperViewHelper> reppers = new List<RepperViewHelper>();

            foreach (FinArticleCategory cat in bud.FinArticleCategories)
            {

                foreach (FinancialArticle finarticle in cat.FinancialArticles)
                {

                    i = -1;

                    ArticleTotal = finarticle.Price.Value * finarticle.Amt.Value * finarticle.Times.Value * finarticle.TimePeriod.Value + finarticle.TransferAmt.Value;
                    CatSumValue = CatSumValue + ArticleTotal;

                    //initialize the reppers only once for each category.
                    if (ReportPeriodLength < finarticle.ReportPeriods.Count)
                    {
                        ReportPeriodLength = finarticle.ReportPeriods.Count;
                        Length = ReportPeriodLength - Length;
                        for (int count = 0; count < Length; count++)
                        {
                            RepperViewHelper repperag = new RepperViewHelper();

                            reppers.Add(repperag);
                        }
                    }

                    foreach (ReportPeriod repper in finarticle.ReportPeriods)
                    {
                        //3 rep periods loop here.
                        i++;
                        //reppers[0] = repper[0] + repper.amount 
                        //reppers[1] = repper[1] + repper.amount 
                        reppers[i].SumValue = reppers[i].SumValue + repper.Amount.Value;
                    }

                }

                CatRepViewHelper catrep = new CatRepViewHelper();
                List<RepperViewHelper> subreppers = new List<RepperViewHelper>();
                catrep.CatName = cat.FinArtCatList.FinArticleCatName;
                catrep.CatSumValue = CatSumValue;


                //reppers.Clear() clears the resulting subreppers too, 
                //that is why i add each item in foreach.
                foreach (RepperViewHelper repperitem in reppers)
                {
                    subreppers.Add(repperitem);
                }

                catrep.repper = subreppers;
                catreplist.Add(catrep);

                //clear all for next category.
                ArticleTotal = 0;
                CatSumValue = 0;
                i = -1;
                ReportPeriodLength = 0;
                Length = 0;
                reppers.Clear();
            }

            return catreplist;
        }


    }//class end.

        #endregion 

}
