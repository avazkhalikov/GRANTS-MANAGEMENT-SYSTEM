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
    public class BudgetPaymentReport
    {
        public decimal cashOnHand=0;        // comes from previous period CalcBalance
        public decimal PeriodGotMoney = 0;    // comes from previous period CalcMoneyToTransfer
        public decimal CalcAllMoney = 0;
        public decimal MoneySpent=0;        //comes from Aggregated Transfer calculations for this periods.
        public decimal CalcBalance = 0;
        public decimal ReportPeriodRequest = 0;  //comes from USER INPUT! Updated in ReportPeriod table.
        public decimal CalcMoneyToTransfer = 0;
        public decimal StartingAmount = 0; 
        public ReportPeriodListR repper;

    }

    public class BudgetPaymentReportService
    {

       // private List<CatRepView> catrep;
        private List<ReportPeriodListR> reppers;
        private CatRepView acumcat;

        public BudgetPaymentReportService(/*List<CatRepView> _catrep, */ IEnumerable<ReportPeriodListR> _reppers, CatRepView _acumcat)
        {
           // catrep = _catrep;
            reppers = _reppers.ToList();
            acumcat = _acumcat;

       
        }

        public List<BudgetPaymentReport> GenerateReports2(int InitialAmount)
        {
            //--   acumcat.repper[0].SumValue = "expenditure reported" //for the first period
            //--   reppers[0].PaymentAmount = "first payment" //initial payment  
            List<BudgetPaymentReport> PaymentReports = new List<BudgetPaymentReport>();

            int i;
            decimal Balance =0; 
            for (i = 0; i < reppers.Count(); i++)
            {
                BudgetPaymentReport paymentreport = new BudgetPaymentReport();

                //initial first period payment. 
                if (i == 0)
                {
                    //Check for Value Exist if don't make it 0; 
                    if (reppers[i].PaymentAmount.HasValue)
                        paymentreport.PeriodGotMoney = reppers[i].PaymentAmount.Value;
                    else
                        paymentreport.PeriodGotMoney = 0;

                    if (acumcat.repper[i] != null)
                        paymentreport.MoneySpent = acumcat.repper[i].SumValue;
                    else
                        paymentreport.MoneySpent = 0;

                    if (reppers[i].PaymentAmount.HasValue)
                        paymentreport.cashOnHand = reppers[i].PaymentAmount.Value;
                    else
                        paymentreport.cashOnHand = 0;

                    paymentreport.CalcBalance = paymentreport.cashOnHand - paymentreport.MoneySpent;
                    Balance = paymentreport.CalcBalance; //this will be used for next period.
                    
                    //in intial period we transfer as much as they ask!
                    if (reppers[i].PaymentAmount.HasValue)
                    {
                        paymentreport.CalcMoneyToTransfer = reppers[i].PaymentAmount.Value;
                    }
                    else
                    {
                        paymentreport.CalcMoneyToTransfer = 0; 
                    }
                }
                else
                {
                   
                    paymentreport.StartingAmount = Balance;

                    if (reppers[i].PaymentAmount.HasValue)
                    {
                        //if you have enough money, and asking more, you get nothing.
                        if (reppers[i].PaymentAmount.Value < Balance)
                        {
                            paymentreport.CalcMoneyToTransfer = 0;
                        }
                        else
                        {
                            paymentreport.CalcMoneyToTransfer = reppers[i].PaymentAmount.Value - Balance;
                        }
                    }
                    else
                    {
                        paymentreport.CalcMoneyToTransfer = 0;
                    }

                   

                    paymentreport.PeriodGotMoney = paymentreport.CalcMoneyToTransfer;


                    if (reppers[i].PaymentStatus != null)
                    {
                        if (reppers[i].PaymentStatus.Value == 2)
                        {
                            paymentreport.cashOnHand = paymentreport.StartingAmount + paymentreport.PeriodGotMoney;
                        }
                        else
                        {
                            paymentreport.cashOnHand = paymentreport.StartingAmount;
                        }
                    }
                    else
                    {
                        paymentreport.cashOnHand = paymentreport.StartingAmount;
                    }



                    if (acumcat.repper[i] != null)
                        paymentreport.MoneySpent = acumcat.repper[i].SumValue;
                    else
                        paymentreport.MoneySpent = 0;


                    paymentreport.CalcBalance = paymentreport.cashOnHand - paymentreport.MoneySpent;
                    Balance = paymentreport.CalcBalance;  

                }

                paymentreport.repper = reppers[i];  //add the report period too. to display in view.
                PaymentReports.Add(paymentreport);

            }



            return PaymentReports; 
        }


        public List<BudgetPaymentReport> GenerateReports(int InitialAmount)
        {
            List<BudgetPaymentReport> PaymentReports = new List<BudgetPaymentReport>();

            int i;
            for (i = 0; i < reppers.Count(); i++)
            {
                BudgetPaymentReport paymentreport = new BudgetPaymentReport();

                if (i > 0)
                {
                    if (PaymentReports[i - 1] != null)
                        paymentreport.cashOnHand = PaymentReports[i - 1].CalcBalance;
                    else
                        paymentreport.cashOnHand = 0;
                }
                else
                {
                    paymentreport.cashOnHand = 0;
                }

                if (i > 0)
                {
                    if (PaymentReports[i - 1] != null)
                        paymentreport.PeriodGotMoney = PaymentReports[i - 1].CalcMoneyToTransfer;
                    else
                        paymentreport.PeriodGotMoney = InitialAmount; 
                }
                else
                {
                    paymentreport.PeriodGotMoney = InitialAmount; 
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

                //if grantee is requesting money, even though he has money in balance then we don't transfer.
                if (paymentreport.ReportPeriodRequest > paymentreport.CalcBalance)
                {
                    paymentreport.CalcMoneyToTransfer = paymentreport.ReportPeriodRequest - paymentreport.CalcBalance;
                }
                else
                {
                    paymentreport.CalcMoneyToTransfer = 0; 
                }

                //if no request then we don't transfer.
                if (paymentreport.ReportPeriodRequest == 0)
                {
                    paymentreport.CalcMoneyToTransfer = 0;
                }


                paymentreport.repper = reppers[i];  //add the report period too. to display in view.

                PaymentReports.Add(paymentreport);
            }



            return PaymentReports;

        }

    }
}
