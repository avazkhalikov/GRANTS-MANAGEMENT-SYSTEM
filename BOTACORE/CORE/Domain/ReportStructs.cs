using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOTACORE.CORE.Domain
{
    #region Budget analysis
    public class Rc
    {
        public decimal Amount; // 45,666
        public string Row;  // SS 
        public string Column; // ECD
    }
    #endregion 

    public class LabelByCount
    {
        public string Label { get; set; }
        public int? Counted { get; set; }
        public int? GrantTypeCodeID { get; set; }
        public int? LabelContentCategoryID { get; set; }
        public int? LabelID { get; set; }
    }
    public class IndicatorRep
    {
        public string Label { get; set; }
        public int? Counted { get; set; }
        public int? GrantTypeCodeID { get; set; }
        public string GrantTypeR { get; set; }
        public string GrantTypeV { get; set; }
        public int? LabelContentCategoryID { get; set; }
        public int? LabelID { get; set; }
    }
    
    public class RequestedAndAwardedAmountStruct
    {
        public string Region { get; set; }
        public int NumberofAllProjects { get; set; }
        public int NumberofActiveProjects { get; set; }
        public int NumberofProposals { get; set; }
        public decimal RequestedAmt { get; set; }
        public decimal AwarderAmt { get; set; }
    }

    public class RequestedAmountByX
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Average { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
        public int NumberOfGrants { get; set; }
    }


}
