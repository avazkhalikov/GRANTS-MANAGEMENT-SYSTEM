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
    /// View Domain Class. Struct to hold the data. 
    /// </summary>
    public class PaymentReport
    {
        public int cashOnHand;        // comes from previous period CalcBalance
        public int PeriodGotMoney;    // comes from previous period CalcMoneyToTransfer
        public int CalcAllMoney;
        public int MoneySpent;        //comes from Aggregated Transfer calculations for this periods.
        public int CalcBalance;
        public int ReportPeriodRequest;  //comes from USER INPUT! Updated in ReportPeriod table.
        public int CalcMoneyToTransfer;
        public ReportPeriodList repper;

    }

   public class PaymentReportService
    {

        private List<CatRepViewHelper> catrep;
        private List<ReportPeriodList> reppers;
        private CatRepViewHelper acumcat; 

        public PaymentReportService(List<CatRepViewHelper> _catrep, IEnumerable<ReportPeriodList> _reppers,  CatRepViewHelper _acumcat)
        {
            catrep = _catrep;
            reppers = _reppers.ToList();
            acumcat = _acumcat;
        }

        public List<PaymentReport> GenerateReports(int InitialAmount)
        {
            List<PaymentReport> PaymentReports = new List<PaymentReport>();
            
            int i;
            for (i = 0; i < reppers.Count(); i++)
            {
                PaymentReport paymentreport = new PaymentReport();

                if (i > 0)
                {
                    if (PaymentReports[i - 1] != null)
                        paymentreport.cashOnHand = PaymentReports[i - 1].CalcBalance;
                    else
                        paymentreport.cashOnHand = InitialAmount;
                }
                else
                {
                    paymentreport.cashOnHand = InitialAmount;
                }

                if (i > 0)
                {
                    if (PaymentReports[i - 1] != null)
                        paymentreport.PeriodGotMoney = PaymentReports[i - 1].CalcMoneyToTransfer;
                    else
                        paymentreport.PeriodGotMoney = 0;
                }
                else
                {
                    paymentreport.PeriodGotMoney = 0;
                }

                paymentreport.CalcAllMoney = paymentreport.cashOnHand + paymentreport.PeriodGotMoney;

                //acumulate the spendings of each repper period in catrep.

                paymentreport.MoneySpent = acumcat.repper[i].SumValue;
                
                paymentreport.CalcBalance = paymentreport.CalcAllMoney - paymentreport.MoneySpent;

                if (reppers[i].PaymentAmount == null)
                {
                    paymentreport.ReportPeriodRequest = 0;
                }
                else
                {
                    paymentreport.ReportPeriodRequest = reppers[i].PaymentAmount.Value;
                }

                paymentreport.CalcMoneyToTransfer = paymentreport.ReportPeriodRequest - paymentreport.CalcBalance;

                paymentreport.repper = reppers[i];  //add the report period too. to display in view.

                PaymentReports.Add(paymentreport); 
            }



            return PaymentReports; 
           
        }

    }
}
