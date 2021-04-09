using System.Collections.Generic;
namespace BOTAMVC3
{
    public enum ContainerType
    {
        AreaVsType,
        AreaVsRound,
        AreaVsStatus,
        TypeVsRound,
        TypeVsStatus,
        RoundVsStatus,
        AreaVsRegion, 
        TypeVsRegion,
        RoundVsRegion,
        BudgetVsArea,
        BudgetVsType,
        BudgetVsRound,
        BudgetVsStatus
    };


    public enum IndicatorContainerType
    {
        OblastVsIndicatorLabelCategory,
        AreaVsIndicatorLabelCategory,
        TypeVsIndicatorLabelCategory,
        RoundVsIndicatorLabelCategory
    };

    public enum DRTYPE
    {
        Round,
        Type,
        Area,
        
    };

    public enum AmountTypes
    {
        AmountRequested,
        AwardedAmount,
        AllTransfered,
        UsedAmount,
        UnusedAmount,
        CashOnHand,
        Refund,
        Cancellation
    };

    public enum DRAmountType
    {      
        AllTransferedVSAwardedAmount,
        CashOnHandVSAwardedAmount,
        UsedAmountVSAwardedAmount,
        UnusedAmountVSAwardedAmount,
        RefundVSAwardedAmount,
        CancellationVSAwardedAmount,
        AllTransferedVSUsed 
    };
 


    //View Wrapper Class for ReportViewControl.aspx Financial Report.
    //it is simply to hold Database Lists such as grant type and program area. 
    //-----and TO Reuse the RepotViewControl Block code, instead of copy pasting for each List.  ----
    /*
           //TODO: cases(ListType) go here    Wrapper Dictionary(string=ListType, Dictinory(string=Text, Int=Val))                 
            //Dictionary(string, int) generalX = empty; Dictionary(string, int) generalY = empty;
            // Case found generalX = (IEnumerable<GrantTypeList>)ViewData["GrantType"] through ForeachEnum here.
            //Later generalX, generalY replace all -> IEnumerable<GrantTypeList>)ViewData["GrantType"] 
     */




}