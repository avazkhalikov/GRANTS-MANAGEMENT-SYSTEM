using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using System.Text;
using System.Data.Linq;
using BOTAMVC3.Helpers;
using BOTACORE.CORE.DataAccess.Impl;
using System.Data;

namespace BOTAMVC3.Controllers
{
 


    public class FinReport2Controller : Controller
    {
        // GET: /FinReport2/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
           return View();
            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(FinReportFilter frepf)
        {
            if (frepf != null)
            {
                LReportsRepository rep = new LReportsRepository();
                IQueryable<Project> prj = rep.GetResults(frepf);

                if (prj != null)
                {
                   // ViewData["report31"] = rep.IndicatorByBaseline(prj);
                    ViewData["prj"] = prj;
                    ViewData["report1"] = rep.RequestedAndAwardedAmountByRegion(prj);
                    ViewData["report2"] = rep.RequestedAmountByArea(prj);
                    ViewData["report3"] = rep.RequestedAmountByGrantType(prj);
                    ViewData["report4"] = rep.AwardedAmountByArea(prj);
                    ViewData["report5"] = rep.AwardedAmountByGrantType(prj);  
                    
                }

                return View(frepf);
            }
            else
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BudgetAnalysis()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            ViewData["results"] = null;
            ViewData["Region"] = ServiceDDL.GetRegionList(); 
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BudgetAnalysis(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> period, List<String> amount, List<String> oblast)
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["CompletionCode"] = ServiceDDL.GetCompetitionCodeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["Status"] = ServiceDDL.GetProposalStatusList();
            ViewData["Region"] = ServiceDDL.GetRegionList(); 
            //if (frepf != null)
            //{
            LReportsRepository rep = new LReportsRepository();
            IQueryable<Project> prj = rep.GetResults3(frepf, ID, Area, gtype,compete,status, period, oblast, amount); //1. gets resulting project list after filtering.
             

            List<Project> prjList = prj.ToList();
            ViewData["prj"] = prj;
            

            if(prjList != null )
            {
                //2. generates/calculates VS amounts.
                if (!frepf.isListOnly.Value)
                {
                    Dictionary<ContainerType, Dictionary<AmountTypes, List<VsContainer>>> results = FinReportCore(prj, frepf, Area, gtype, compete, status, period, oblast);
                    ViewData["results2"] = results;
                }
            }
            
            //add List Filters.  //enable Report View enabled if not null.
            if(Area!=null)
            {
                frepf.IsArea = true; 
            }
            if (gtype != null)
            {
                frepf.IsGrantType = true;
            }
            if(compete!=null)
            {
                frepf.IsCompetitionCode = true;
            }
            if (status != null)
            {
                frepf.IsStatus = true;
            }
            if (period != null)
            {
                frepf.IsPeriod = true;
            }
            if (oblast != null)
            {
                frepf.isRegion = true;
            }

           
            //=====

            return View(frepf);
           
        }


        ////dispetcher, resends model to ReportView.
        //public ActionResult ResultContainerDisplay(List<List<VsContainer>> results)
        //{
        //    return PartialView("ReportViewControl", results);
        //}



        /// <summary>
        ///   ==Logic: 
        ///   1. identify which field required, using List of controls/filters passed, frepf.
        ///  2. call aggregator/grouper methods for identified fields.
        ///   3.  collect the results and return.
        /// </summary>
        /// <param name="prjList"></param>
        /// <param name="frepf"></param>
        /// <param name="Area"></param>
        /// <param name="gtype"></param>
        /// <param name="compete"></param>
        /// <param name="status"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public Dictionary<ContainerType, Dictionary<AmountTypes, List<VsContainer>>> FinReportCore(IQueryable<Project> prjList, FinReportFilter frepf, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> period, List<String> region)
        {
            //we have 7 Type of Amounts: from frepf.
            //InitialRequestedAmount =1
            //AwardedAmount  =2 
            //AllTransfered =3
            //UsedAmount =4 
            //UnUsedAmount =5 
            //CashOnHand = 6
            //Refund = 7
            //Cancellation = 8

            //Check if result has values found for every requested FIELD1, FIELD2 combination, 
            //if it does not then then insert into List ghost project with 0 values.
            //Updated list will be called result2!! 
            //WHY???? Because my stupid table view generation is getting messed up if one of the fields don't present.
            //I think this must be done before doAreaVsType called. Must be static function that populates existing result list with ghost projects.
         //   IQueryable<Project> prjFullList = FullFillList(prjList);
            

            //to identify 6 cases, defaulting false. we use above controls. Not prjList.
            Dictionary<string, List<Rc>> RepCollection = new Dictionary<string, List<Rc>>();

            var LLVsContainer = new Dictionary<ContainerType, Dictionary<AmountTypes, List<VsContainer>>>();
           // Dictionary<ContainerType, List<VsContainer>> LLVsContainer = null; 
            if (Area != null && Area.Count > 0 && gtype != null && gtype.Count > 0)
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.AreaVsType, LLVsContainer);
            }
            if (Area != null && Area.Count > 0 && compete != null && compete.Count > 0)
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.AreaVsRound, LLVsContainer);
            }
            if (Area != null && Area.Count > 0 && status != null && status.Count > 0)
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.AreaVsStatus, LLVsContainer);
            }
            if (gtype != null && gtype.Count > 0 && compete != null && compete.Count > 0)
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.TypeVsRound, LLVsContainer);
            }
            if (gtype != null && gtype.Count > 0 && status != null && status.Count > 0)
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.TypeVsStatus, LLVsContainer);
            }
            if (compete != null && compete.Count > 0 && status != null && status.Count > 0)
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.RoundVsStatus, LLVsContainer);
            }
            if (compete != null && compete.Count > 0 && region != null && region.Count > 0)   
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.RoundVsRegion, LLVsContainer);
            }
            if (gtype != null && gtype.Count > 0 && region != null && region.Count > 0)   //TypeVsRegion
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.TypeVsRegion, LLVsContainer);
            }
            if (Area != null && Area.Count > 0 && region != null && region.Count > 0)   //AreaVsRegion
            {
                LLVsContainer = FillContainer(prjList, frepf, ContainerType.AreaVsRegion, LLVsContainer);
            }
            
          //  bool areaVsRound = false;


            //more coming here.
        


            return LLVsContainer;
        }




        private static Dictionary<ContainerType, Dictionary<AmountTypes, List<VsContainer>>> FillContainer(IQueryable<Project> prjList, FinReportFilter frepf, ContainerType ct,
             Dictionary<ContainerType, Dictionary<AmountTypes, List<VsContainer>>> LLVsContainer) 
            //Dictionary<ContainerType, List<VsContainer>> LLVsContainer)
        {
            bool areaVsType = false;
           // List<List<VsContainer>> LLVsContainer = new List<List<VsContainer>>();  //container to hold List Results.
           
            //TODO: 
            //this code must be refactored!! ....One manager should be calling service class and 
            //service class manager should one by one call other service private methods,
            //send ones results to another and at the end return result to controller. 
            //Now: we have Too much of coupling of one method to another! Manager must be responsible for chain calls! also too much of repetitive code !
            //BUT HEY! WORKS!
            
            LReportsRepository rep = new LReportsRepository();
            if (frepf.isAmountRequested != null && frepf.isAmountRequested.Value)
            {
                List<VsContainer> result = null; 
                //doAreaVsType  //I order list By Field1, needed to create table. The next comes Grouper SUMMER->Counter! Just does math.
                result = RepCall(prjList, ct, 1);  //repository call! instead of 1 must use enum type.

                Dictionary<AmountTypes, List<VsContainer>> resultDictionary = new Dictionary<AmountTypes, List<VsContainer>>();
                resultDictionary.Add(AmountTypes.AmountRequested, result);

                if (LLVsContainer.ContainsKey(ct))
                {
                    if (result != null) LLVsContainer[ct].Add(AmountTypes.AmountRequested, result);//.AddRange(resultDictionary);
                }
                else
                {
                    LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                }
            }
            if (frepf.IsAwardedAmount != null && frepf.IsAwardedAmount.Value)
            {
                   List<VsContainer> result = null; 
                   switch (ct)
                   {
                       case ContainerType.AreaVsType: result = rep.doAreaVsType(prjList, 2).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRound: result = rep.DoAreaVsCompetitionCode(prjList, 2).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsStatus: result = rep.DoAreaVsStatus(prjList, 2).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRound: result = rep.DoGrantTypeVsCompetitionCode(prjList, 2).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsStatus: result = rep.DoGrantTypeVsStatus(prjList, 2).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsStatus: result = rep.DoCompetitionCodeVsStatus(prjList, 2).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRegion: result = rep.DoAreaVsRegion(prjList, 2).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsRegion: result = rep.DoRoundVsRegion(prjList, 2).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRegion: result = rep.DoTypeVsRegion(prjList, 2).OrderBy(k => k.Field2).ToList(); break; 
                   }
                   Dictionary<AmountTypes, List<VsContainer>> resultDictionary = new Dictionary<AmountTypes, List<VsContainer>>();
                   resultDictionary.Add(AmountTypes.AwardedAmount, result);

                   if (LLVsContainer.ContainsKey(ct))
                   {
                       if (result != null) LLVsContainer[ct].Add(AmountTypes.AwardedAmount, result);//.AddRange(resultDictionary);
                   }
                   else
                   {
                       LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                   }
                  
            }
           
            //....more coming here.
            if (frepf.IsAllTransfered != null && frepf.IsAllTransfered.Value)
            {
                  List<VsContainer> result = null; 
                   switch (ct)
                   {
                       case ContainerType.AreaVsType: result =rep.doAreaVsType(prjList, 3).OrderBy(k => k.Field2).ToList();break;
                       case ContainerType.AreaVsRound: result = rep.DoAreaVsCompetitionCode(prjList, 3).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsStatus: result = rep.DoAreaVsStatus(prjList, 3).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRound: result = rep.DoGrantTypeVsCompetitionCode(prjList, 3).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsStatus: result = rep.DoGrantTypeVsStatus(prjList, 3).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsStatus: result = rep.DoCompetitionCodeVsStatus(prjList, 3).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRegion: result = rep.DoAreaVsRegion(prjList, 3).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsRegion: result = rep.DoRoundVsRegion(prjList, 3).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRegion: result = rep.DoTypeVsRegion(prjList, 3).OrderBy(k => k.Field2).ToList(); break; 
                   }
                   Dictionary<AmountTypes, List<VsContainer>> resultDictionary = new Dictionary<AmountTypes, List<VsContainer>>();
                   resultDictionary.Add(AmountTypes.AllTransfered, result);

                   if (LLVsContainer.ContainsKey(ct))
                   {
                       if (result != null) LLVsContainer[ct].Add(AmountTypes.AllTransfered, result);//.AddRange(resultDictionary);
                   }
                   else
                   {
                       LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                   }
                   //if (LLVsContainer.ContainsKey(ct))
                   //{
                   //    if (result != null) LLVsContainer[ct].AddRange(result);
                   //}
                   //else
                   //{
                   //    LLVsContainer.Add(ct, result); //add result to Container List.
                   //}
            }
            if (frepf.IsUsedAmount != null && frepf.IsUsedAmount.Value)
            {
                  List<VsContainer> result = null; 
                   switch (ct)
                   {
                       case ContainerType.AreaVsType: result  = rep.doAreaVsType(prjList, 4).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRound: result = rep.DoAreaVsCompetitionCode(prjList, 4).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsStatus: result = rep.DoAreaVsStatus(prjList, 4).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRound: result = rep.DoGrantTypeVsCompetitionCode(prjList, 4).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsStatus: result = rep.DoGrantTypeVsStatus(prjList, 4).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsStatus: result = rep.DoCompetitionCodeVsStatus(prjList, 4).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRegion: result = rep.DoAreaVsRegion(prjList, 4).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsRegion: result = rep.DoRoundVsRegion(prjList, 4).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRegion: result = rep.DoTypeVsRegion(prjList, 4).OrderBy(k => k.Field2).ToList(); break; 
                   }
                   Dictionary<AmountTypes, List<VsContainer>> resultDictionary = new Dictionary<AmountTypes, List<VsContainer>>();
                   resultDictionary.Add(AmountTypes.UsedAmount, result);

                   if (LLVsContainer.ContainsKey(ct))
                   {
                       if (result != null) LLVsContainer[ct].Add(AmountTypes.UsedAmount, result);//.AddRange(resultDictionary);
                   }
                   else
                   {
                       LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                   }
                   //if (LLVsContainer.ContainsKey(ct))
                   //{
                   //    if (result != null) LLVsContainer[ct].AddRange(result);
                   //}
                   //else
                   //{
                   //    LLVsContainer.Add(ct, result); //add result to Container List.
                   //}
            }
            if (frepf.IsUnusedAmount != null && frepf.IsUnusedAmount.Value)
            {
                 List<VsContainer> result = null; 
                   switch (ct)
                   {
                       case ContainerType.AreaVsType: result = rep.doAreaVsType(prjList, 5).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRound: result = rep.DoAreaVsCompetitionCode(prjList, 5).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsStatus: result = rep.DoAreaVsStatus(prjList, 5).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRound: result = rep.DoGrantTypeVsCompetitionCode(prjList, 5).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsStatus: result = rep.DoGrantTypeVsStatus(prjList, 5).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsStatus: result = rep.DoCompetitionCodeVsStatus(prjList, 5).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRegion: result = rep.DoAreaVsRegion(prjList, 5).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsRegion: result = rep.DoRoundVsRegion(prjList, 5).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRegion: result = rep.DoTypeVsRegion(prjList, 5).OrderBy(k => k.Field2).ToList(); break; 
                   }
                   Dictionary<AmountTypes, List<VsContainer>> resultDictionary = new Dictionary<AmountTypes, List<VsContainer>>();
                   resultDictionary.Add(AmountTypes.UnusedAmount, result);

                   if (LLVsContainer.ContainsKey(ct))
                   {
                       if (result != null) LLVsContainer[ct].Add(AmountTypes.UnusedAmount, result);//.AddRange(resultDictionary);
                   }
                   else
                   {
                       LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                   }
                   //if (LLVsContainer.ContainsKey(ct))
                   //{
                   //    if (result != null) LLVsContainer[ct].AddRange(result);
                   //}
                   //else
                   //{
                   //    LLVsContainer.Add(ct, result); //add result to Container List.
                   //}
            }
            if (frepf.IsCashOnHand != null && frepf.IsCashOnHand.Value)
            {
                  List<VsContainer> result = null; 
                   switch (ct)
                   {
                       case ContainerType.AreaVsType: result =rep.doAreaVsType(prjList, 6).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRound: result = rep.DoAreaVsCompetitionCode(prjList, 6).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsStatus: result = rep.DoAreaVsStatus(prjList, 6).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRound: result = rep.DoGrantTypeVsCompetitionCode(prjList, 6).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsStatus: result = rep.DoGrantTypeVsStatus(prjList, 6).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsStatus: result = rep.DoCompetitionCodeVsStatus(prjList, 6).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRegion: result = rep.DoAreaVsRegion(prjList, 6).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsRegion: result = rep.DoRoundVsRegion(prjList, 6).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRegion: result = rep.DoTypeVsRegion(prjList, 6).OrderBy(k => k.Field2).ToList(); break; 
                   }
                   Dictionary<AmountTypes, List<VsContainer>> resultDictionary = new Dictionary<AmountTypes, List<VsContainer>>();
                   resultDictionary.Add(AmountTypes.CashOnHand, result);

                   if (LLVsContainer.ContainsKey(ct))
                   {
                       if (result != null) LLVsContainer[ct].Add(AmountTypes.CashOnHand, result);//.AddRange(resultDictionary);
                   }
                   else
                   {
                       LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                   }
                   //if (LLVsContainer.ContainsKey(ct))
                   //{
                   //    if (result != null) LLVsContainer[ct].AddRange(result);
                   //}
                   //else
                   //{
                   //    LLVsContainer.Add(ct, result); //add result to Container List.
                   //}
            }
            if (frepf.IsRefund != null && frepf.IsRefund.Value)
            {
                  List<VsContainer> result = null; 
                   switch (ct)
                   {
                       case ContainerType.AreaVsType: result = rep.doAreaVsType(prjList, 7).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRound: result = rep.DoAreaVsCompetitionCode(prjList, 7).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsStatus: result = rep.DoAreaVsStatus(prjList, 7).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRound: result = rep.DoGrantTypeVsCompetitionCode(prjList, 7).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsStatus: result = rep.DoGrantTypeVsStatus(prjList, 7).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsStatus: result = rep.DoCompetitionCodeVsStatus(prjList, 7).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRegion: result = rep.DoAreaVsRegion(prjList, 7).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsRegion: result = rep.DoRoundVsRegion(prjList, 7).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRegion: result = rep.DoTypeVsRegion(prjList, 7).OrderBy(k => k.Field2).ToList(); break; 
                   }
                   Dictionary<AmountTypes, List<VsContainer>> resultDictionary = new Dictionary<AmountTypes, List<VsContainer>>();
                   resultDictionary.Add(AmountTypes.Refund, result);

                   if (LLVsContainer.ContainsKey(ct))
                   {
                       if (result != null) LLVsContainer[ct].Add(AmountTypes.Refund, result);//.AddRange(resultDictionary);
                   }
                   else
                   {
                       LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                   }
                   //if (LLVsContainer.ContainsKey(ct))
                   //{
                   //    if (result != null) LLVsContainer[ct].AddRange(result);
                   //}
                   //else
                   //{
                   //    LLVsContainer.Add(ct, result); //add result to Container List.
                   //}
            }
            if (frepf.IsCancellation != null && frepf.IsCancellation.Value)
            {
               List<VsContainer> result = null; 
                   switch (ct)
                   {
                       case ContainerType.AreaVsType: result = rep.doAreaVsType(prjList, 8).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRound: result = rep.DoAreaVsCompetitionCode(prjList, 8).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsStatus: result = rep.DoAreaVsStatus(prjList, 8).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRound: result = rep.DoGrantTypeVsCompetitionCode(prjList, 8).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsStatus: result = rep.DoGrantTypeVsStatus(prjList, 8).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsStatus: result = rep.DoCompetitionCodeVsStatus(prjList, 8).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.AreaVsRegion: result = rep.DoAreaVsRegion(prjList, 8).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.RoundVsRegion: result = rep.DoRoundVsRegion(prjList, 8).OrderBy(k => k.Field2).ToList(); break;
                       case ContainerType.TypeVsRegion: result = rep.DoTypeVsRegion(prjList, 8).OrderBy(k => k.Field2).ToList(); break; 
                   }
                   Dictionary<AmountTypes, List<VsContainer>> resultDictionary = new Dictionary<AmountTypes, List<VsContainer>>();
                   resultDictionary.Add(AmountTypes.Cancellation, result);

                   if (LLVsContainer.ContainsKey(ct))
                   {
                       if (result != null) LLVsContainer[ct].Add(AmountTypes.Cancellation, result);//.AddRange(resultDictionary);
                   }
                   else
                   {
                       LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                   }
                   //if (LLVsContainer.ContainsKey(ct))
                   //{
                   //    if (result != null) LLVsContainer[ct].AddRange(result);
                   //}
                   //else
                   //{
                   //    LLVsContainer.Add(ct, result); //add result to Container List.
                   //}
            }

            return LLVsContainer; 
        }

        private static List<VsContainer> RepCall(IQueryable<Project> prjList, ContainerType ct, int Amounts)
        {
            LReportsRepository rep = new LReportsRepository();
            List<VsContainer> result=null;
            switch (ct)
            {
                case ContainerType.AreaVsType:
                    result = rep.doAreaVsType(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
                case ContainerType.AreaVsRound:
                    result = rep.DoAreaVsCompetitionCode(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
                case ContainerType.AreaVsStatus:
                    result = rep.DoAreaVsStatus(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
                case ContainerType.TypeVsRound:
                    result = rep.DoGrantTypeVsCompetitionCode(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
                case ContainerType.TypeVsStatus:
                    result = rep.DoGrantTypeVsStatus(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
                case ContainerType.RoundVsStatus:
                    result = rep.DoCompetitionCodeVsStatus(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
                case ContainerType.AreaVsRegion:
                    result = rep.DoAreaVsRegion(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
                case ContainerType.RoundVsRegion:
                    result = rep.DoRoundVsRegion(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
                case ContainerType.TypeVsRegion:
                    result = rep.DoTypeVsRegion(prjList, Amounts).OrderBy(k => k.Field2).ToList();
                    break;
            }
            return result;
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Indicator()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
          //  IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            IEnumerable<GrantTypeList> GrantType = ServiceDDL.GetGrantTypeList();
            IEnumerable<CompetitionCodeList> CompletionCode = ServiceDDL.GetCompetitionCodeList();
            IEnumerable<ProposalStatusList> Status = ServiceDDL.GetProposalStatusList();
            IEnumerable<RegionList> Region = ServiceDDL.GetRegionList();
            ViewData["ProgramArea"] = ProgramArea;
           // ViewData["ProposalStatus"] = ProposalStatus;
            ViewData["GrantType"] = GrantType;
            ViewData["CompletionCode"] = CompletionCode;
            ViewData["Status"] = Status;
            ViewData["Region"] = Region;

            return View();

        }




        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Indicator(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype, List<String> compete,
            List<String> status, List<String> oblast, List<String> period, List<String> amount,
            List<String> location)
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
            IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            IEnumerable<GrantTypeList> GrantType = ServiceDDL.GetGrantTypeList();
            IEnumerable<CompetitionCodeList> CompletionCode = ServiceDDL.GetCompetitionCodeList();
            IEnumerable<ProposalStatusList> Status = ServiceDDL.GetProposalStatusList();
            IEnumerable<RegionList> Region = ServiceDDL.GetRegionList();
            ViewData["ProgramArea"] = ProgramArea;
            ViewData["ProposalStatus"] = ProposalStatus;
            ViewData["GrantType"] = GrantType;
            ViewData["CompletionCode"] = CompletionCode;
            ViewData["Status"] = Status;
            ViewData["Region"] = Region;

           
         

            if (frepf != null)
            {
                LReportsRepository rep = new LReportsRepository();
               // IQueryable<Project> prj = rep.GetResults(frepf);
                IQueryable<Project> prj = rep.GetResults2(frepf, ID, Area, gtype, compete, 
                                                        status, oblast, period, amount, location);
                List<Project> prjList = prj.ToList(); 

                if (prj != null)
                {
                   // List<LabelByCount> test =  rep.IndicatorByBaseline(prj, 2); 
                    ViewData["report31"] = rep.IndicatorByBaseline(prj);
                    ViewData["report32"] = rep.IndicatorByBenchmark(prj);
                    ViewData["report33"] = rep.IndicatorByFinal(prj);
                    ViewData["prj"] = prj;
                    

                }

                return View(frepf);
            }
            else
            {
                return View();
            }
        }
    }
}
