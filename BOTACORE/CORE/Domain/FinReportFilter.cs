using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOTACORE.CORE.Domain
{
    //Budget Analysis Salary Sum Holder
    public class BudgetByCat
    {
        public int? Catid = 0;
        public string CatName;
        public decimal? Awarded = 0;
        public decimal? UsedSum = 0; 
    }

    //============
  
    public class FinBudgetOneProjReport
    {
        public Project  Proj;
        public int? CatID=0;
        public decimal? Awarded=0;
        public decimal? SumByCatTransfered=0;
    }

    public class FinCatReport
    {
        public int? CatID=0;
        public int? gKind = 0;
        public decimal? SumTrans=0;
        public decimal? SumBudget=0;
    }

    public class IndRepHolder
    {
        public string Column;
        public string Row;
        public int? Val;
    }   

    public class VsContainer
    {
        public List<int> ProjId;
        public int? Field1=0;
        public int? Field2=0;
        public decimal? dAmount=0;
        public int? iAmount=0;
        public string Field1Title;
        public string Field2Title;
        public string ByAmountName; 
    }

    public class DRContainer
    {
        public string Name;  //(Group Name, 1st round)
        public int? GrantNumber = 0;
        public decimal? Amt1 = 0;
        public decimal? Amt2 = 0;      
    }

    public class IndicatorRepContainer
    {
        public string Row;
        public List<LabelByCount> Col;
    }

    public class VsContainer2
    {
        public List<Project> Projs;
        public int Field1;
    }

    public class FinReportFilter
    {
//OrganizationName (Yes/No)
//ProjectName (Yes/No)
//Contacts  (Yes/No) (info from “Contacts, 1) -> Director’s Name/cell phone/email/address of organization/phone(in One Column Cell)
//RequestedDate (from dd/mm/year to dd/mm/year) (Yes/No)
//AmountRequested (Yes/No)
//AwardedAmount (Yes/No)
//StartDate (Yes/No)
//EndDate (Yes/No)
//Closedate (Yes/No)
//TotalSiteVisits (Yes/No) (use the number from Project Info)


        private System.Nullable<bool> _isEmail;
        public System.Nullable<bool> isEmail
        {
            get
            {
                return this._isEmail;
            }
            set
            {
                if (this._isEmail != value)
                {
                    this._isEmail = value;

                }
            }
        }

        private System.Nullable<bool> _isIndicator;
        public System.Nullable<bool> isIndicator
        {
            get
            {
                return this._isIndicator;
            }
            set
            {
                if (this._isIndicator != value)
                {
                    this._isIndicator = value;

                }
            }
        }


        private System.Nullable<bool> _isListOnly;
        public System.Nullable<bool> isListOnly
        {
            get
            {
                return this._isListOnly;
            }
            set
            {
                if (this._isListOnly != value)
                {
                    this._isListOnly = value;

                }
            }
        }


        private System.Nullable<bool> _isLFIndicator;
        public System.Nullable<bool> isLFIndicator
        {
            get
            {
                return this._isLFIndicator;
            }
            set
            {
                if (this._isLFIndicator != value)
                {
                    this._isLFIndicator = value;

                }
            }
        }


        private System.Nullable<bool> _OSRussian;
        public System.Nullable<bool> OSRussian
        {
            get
            {
                return this._OSRussian;
            }
            set
            {
                if (this._OSRussian != value)
                {
                    this._OSRussian = value;

                }
            }
        }

       private System.Nullable<bool> _OSEnglish;
        public System.Nullable<bool> OSEnglish
        {
            get
            {
                return this._OSEnglish;
            }
            set
            {
                if (this._OSEnglish != value)
                {
                    this._OSEnglish = value;

                }
            }
        }

        private System.Nullable<bool> _OSKazakh;
        public System.Nullable<bool> OSKazakh
        {
            get
            {
                return this._OSKazakh;
            }
            set
            {
                if (this._OSKazakh != value)
                {
                    this._OSKazakh = value;

                }
            }
        }
    private System.Nullable<bool> _PSEnglish;
        public System.Nullable<bool> PSEnglish
        {
            get
            {
                return this._PSEnglish;
            }
            set
            {
                if (this._PSEnglish != value)
                {
                    this._PSEnglish = value;

                }
            }
        }


      private System.Nullable<bool> _PSKazakh;
        public System.Nullable<bool> PSKazakh
        {
            get
            {
                return this._PSKazakh;
            }
            set
            {
                if (this._PSKazakh != value)
                {
                    this._PSKazakh = value;

                }
            }
        }


        private System.Nullable<bool> _PSRussian;
        public System.Nullable<bool> PSRussian
        {
            get
            {
                return this._PSRussian;
            }
            set
            {
                if (this._PSRussian != value)
                {
                    this._PSRussian = value;

                }
            }
        }

      private System.Nullable<bool> _isAddress;
      public System.Nullable<bool> isAddress
        {
            get
            {
                return this._isAddress;
            }
            set
            {
                if (this._isAddress != value)
                {
                    this._isAddress = value;

                }
            }
        }


       private System.Nullable<bool> _isAmountRequested;
       public System.Nullable<bool> isAmountRequested
       {
           get
           {
               return this._isAmountRequested;
           }
           set
           {
               if (this._isAmountRequested != value)
               {
                   this._isAmountRequested = value;

               }
           }
       }

       private System.Nullable<bool> _isArea;
       public System.Nullable<bool> IsArea
       {
           get
           {
               return this._isArea;
           }
           set
           {
               if (this._isArea != value)
               {
                   this._isArea = value;

               }
           }
       }

       private System.Nullable<bool> _isGrantType;
       public System.Nullable<bool> IsGrantType
       {
           get
           {
               return this._isGrantType;
           }
           set
           {
               if (this._isGrantType != value)
               {
                   this._isGrantType = value;

               }
           }
       }


       private System.Nullable<bool> _isCompetitionCode;
       public System.Nullable<bool> IsCompetitionCode
       {
           get
           {
               return this._isCompetitionCode;
           }
           set
           {
               if (this._isCompetitionCode != value)
               {
                   this._isCompetitionCode = value;

               }
           }
       }



       private System.Nullable<bool> _isStatus;
       public System.Nullable<bool> IsStatus
       {
           get
           {
               return this._isStatus;
           }
           set
           {
               if (this._isStatus != value)
               {
                   this._isStatus = value;

               }
           }
       }

       private System.Nullable<bool> _isPeriod;
       public System.Nullable<bool> IsPeriod
       {
           get
           {
               return this._isPeriod;
           }
           set
           {
               if (this._isPeriod != value)
               {
                   this._isPeriod = value;

               }
           }
       }


        private System.Nullable<bool> _acceptedDate;
        public System.Nullable<bool> IsAcceptedDate
        {
            get
            {
                return this._acceptedDate;
            }
            set
            {
                if ((this._acceptedDate != value))
                {
                    this._acceptedDate = value;
                }
            }
        }

        private System.Nullable<bool> _awardedAmount;
        public System.Nullable<bool> IsAwardedAmount
        {
            get
            {
                return this._awardedAmount;
            }
            set
            {
                if ((this._awardedAmount != value))
                {
                    this._awardedAmount = value;
                }
            }
        }

        private System.Nullable<bool> _usedAmount;
        public System.Nullable<bool> IsUsedAmount
        {
            get
            {
                return this._usedAmount;
            }
            set
            {
                if ((this._usedAmount != value))
                {
                    this._usedAmount = value;
                }
            }
        }

        private System.Nullable<bool> _unusedAmount;
        public System.Nullable<bool> IsUnusedAmount
        {
            get
            {
                return this._unusedAmount;
            }
            set
            {
                if ((this._unusedAmount != value))
                {
                    this._unusedAmount = value;
                }
            }
        }

        private System.Nullable<bool> _cashOnHand;
        public System.Nullable<bool> IsCashOnHand
        {
            get
            {
                return this._cashOnHand;
            }
            set
            {
                if ((this._cashOnHand != value))
                {
                    this._cashOnHand = value;
                }
            }
        }

        private System.Nullable<bool> _cancellation;
        public System.Nullable<bool> IsCancellation
        {
            get
            {
                return this._cancellation;
            }
            set
            {
                if ((this._cancellation != value))
                {
                    this._cancellation = value;
                }
            }
        }

        private System.Nullable<bool> _refund;
        public System.Nullable<bool> IsRefund
        {
            get
            {
                return this._refund;
            }
            set
            {
                if ((this._refund != value))
                {
                    this._refund = value;
                }
            }
        }

        private System.Nullable<bool> _AllTransfered;
        public System.Nullable<bool> IsAllTransfered
        {
            get
            {
                return this._AllTransfered;
            }
            set
            {
                if ((this._AllTransfered != value))
                {
                    this._AllTransfered = value;
                }
            }
        }

       private System.Nullable<System.DateTime> _RequestedDateStart;
       public System.Nullable<System.DateTime> RequestedDateStart
       {
           get
           {
               return this._RequestedDateStart;
           }
           set
           {
               if ((this._RequestedDateStart != value))
               {
                   this._RequestedDateStart = value;                 
               }
           }
       }

       private System.Nullable<System.DateTime> _RequestedDateEnd;
       public System.Nullable<System.DateTime> RequestedDateEnd
       {
           get
           {
               return this._RequestedDateEnd;
           }
           set
           {
               if ((this._RequestedDateEnd != value))
               {
                   this._RequestedDateEnd = value;
               }
           }
       }


       private System.Nullable<bool> _isOrganizationName;
       public System.Nullable<bool> isOrganizationName
       {
           get
           {
               return this._isOrganizationName;
           }
           set
           {
               if (this._isOrganizationName != value)
               {
                   this._isOrganizationName = value;
                 
               }
           }
       }

       private System.Nullable<bool> _isContacts;
       public System.Nullable<bool> isContacts
       {
           get
           {
               return this._isContacts;
           }
           set
           {
               if (this._isContacts != value)
               {
                   this._isContacts = value;

               }
           }
       }


       private System.Nullable<bool> _isProjectName;
       public System.Nullable<bool> isProjectName
       {
           get
           {
               return this._isProjectName;
           }
           set
           {
               if (this._isProjectName != value)
               {
                   this._isProjectName = value;

               }
           }
       }




       private System.Nullable<bool> _isRequestedDate;
       public System.Nullable<bool> isRequestedDate
       {
           get
           {
               return this._isRequestedDate;
           }
           set
           {
               if (this._isRequestedDate != value)
               {
                   this._isRequestedDate = value;

               }
           }
       }



    



       private System.Nullable<bool> _isStartDate;
       public System.Nullable<bool> isStartDate
       {
           get
           {
               return this._isStartDate;
           }
           set
           {
               if (this._isStartDate != value)
               {
                   this._isStartDate = value;

               }
           }
       }



       private System.Nullable<bool> _isEndDate;
       public System.Nullable<bool> isEndDate
       {
           get
           {
               return this._isEndDate;
           }
           set
           {
               if (this._isEndDate != value)
               {
                   this._isEndDate = value;

               }
           }
       }



       private System.Nullable<bool> _isClosedate;
       public System.Nullable<bool> isClosedate
       {
           get
           {
               return this._isClosedate;
           }
           set
           {
               if (this._isClosedate != value)
               {
                   this._isClosedate = value;

               }
           }
       }

       private System.Nullable<bool> _isTotalSiteVisits;
       public System.Nullable<bool> isTotalSiteVisits
       {
           get
           {
               return this._isTotalSiteVisits;
           }
           set
           {
               if (this._isTotalSiteVisits != value)
               {
                   this._isTotalSiteVisits = value;

               }
           }
       }


       private System.Nullable<bool> _isBaseline;
       public System.Nullable<bool> isBaseline
       {
           get
           {
               return this._isBaseline;
           }
           set
           {
               if (this._isBaseline != value)
               {
                   this._isBaseline = value;

               }
           }
       }


       private System.Nullable<bool> _isRegion;
       public System.Nullable<bool> isRegion
       {
           get
           {
               return this._isRegion;
           }
           set
           {
               if (this._isRegion != value)
               {
                   this._isRegion = value;

               }
           }
       }


       private System.Nullable<bool> _isBenchmark;
       public System.Nullable<bool> isBenchmark
       {
           get
           {
               return this._isBenchmark;
           }
           set
           {
               if (this._isBenchmark != value)
               {
                   this._isBenchmark = value;

               }
           }
       }


       private System.Nullable<bool> _isFinal;
       public System.Nullable<bool> isFinal
       {
           get
           {
               return this._isFinal;
           }
           set
           {
               if (this._isFinal != value)
               {
                   this._isFinal = value;

               }
           }
       }

       

    }
}
