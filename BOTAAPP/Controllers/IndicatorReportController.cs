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
      
   

    public class IndicatorReportController : Controller
    {
        //
        // GET: /IndicatorReport/

        public ActionResult Index()
        {
            return RedirectToAction("Indicator2");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Indicator2()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
            IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            IEnumerable<GrantTypeList> GrantType = ServiceDDL.GetGrantTypeList();

            IEnumerable<CompetitionCodeList> CompletionCode = ServiceDDL.GetCompetitionCodeList();

            IEnumerable<ProposalStatusList> Status = ServiceDDL.GetProposalStatusList();
            IEnumerable<RegionList> Region = ServiceDDL.GetRegionList();
            IEnumerable<IndicatorLabelContentCategory> IndicatorCategoryList =
                ServiceDDL.IndicatorLabelContentCategoryList();

            ViewData["ProgramArea"] = ProgramArea;
            ViewData["ProposalStatus"] = ProposalStatus;
            ViewData["GrantType"] = GrantType;

            ViewData["CompletionCode"] = CompletionCode;

            ViewData["Status"] = Status;
            ViewData["Region"] = Region;

            ViewData["IndicatorCategoryList"] = IndicatorCategoryList;

            ViewData["results"] = null;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Indicator2(FinReportFilter frepf, int? ID, List<String> Area, List<String> gtype,
                                       List<String> compete,
                                       List<String> status, List<String> oblast, List<String> period,
                                       List<String> amount, List<String> indicatorcategory)
        {

                     
            LReportsRepository rep = new LReportsRepository();

           
            //IndRepHolder inp = new IndRepHolder();
            // inp.Column = "Akmola obl"; 
            // inp.Row ="ECD";
            // inp.Val = 1;



            AppDropDownsService ServiceDDL = new AppDropDownsService();
            #region drops
            IEnumerable<ProgramAreaList> ProgramArea = ServiceDDL.GetProgramAreaList();
            IEnumerable<ProposalStatusList> ProposalStatus = ServiceDDL.GetProposalStatusList();
            IEnumerable<GrantTypeList> GrantType = ServiceDDL.GetGrantTypeList();
            IEnumerable<CompetitionCodeList> CompletionCode = ServiceDDL.GetCompetitionCodeList();
            IEnumerable<ProposalStatusList> Status = ServiceDDL.GetProposalStatusList();
            IEnumerable<RegionList> Region = ServiceDDL.GetRegionList();
            IEnumerable<IndicatorLabelContentCategory> IndicatorCategoryList =
                ServiceDDL.IndicatorLabelContentCategoryList();
         
            
            ViewData["ProgramArea"] = ProgramArea;
            //take only ECD and Youth.
            IEnumerable<ProgramAreaList> ProgramArea2 = ProgramArea.Where(s => s.ProgramAreaText == "ECD" || s.ProgramAreaText == "Youth");
            ViewData["ProgramArea2"] = ProgramArea2;
            ViewData["ProposalStatus"] = ProposalStatus;
            ViewData["GrantType"] = GrantType;
            ViewData["CompletionCode"] = CompletionCode;
            ViewData["Status"] = Status;
            ViewData["Region"] = Region;
            ViewData["IndicatorCategoryList"] = IndicatorCategoryList;
            #endregion
            //if (frepf != null)
            //{           
         

            IQueryable<Project> prj = rep.GetResults3(frepf, ID, Area, gtype, compete, status, period, oblast, amount);
                //1. gets resulting project list after filtering.


            List<Project> prjList = prj.ToList();
            ViewData["prj"] = prj;

            
          //  List<IndRepHolder> zz = rep.IndicatorsByRoundArea(prj);
            List<IndRepHolder> zz = rep.IndicatorsByCompetitionContentCategory(prj);
            List<IndRepHolder> zz2 = rep.IndicatorsByRegionContentCategory(prj);
            

            ViewData["LIndRep"] = zz;
            ViewData["LIndRep2"] = zz2;

            if (prjList != null)
            {
                //2. generates/calculates VS amounts.
                Dictionary<IndicatorContainerType, Dictionary<IndicatorLabelContentCategory, List<IndicatorRepContainer>>> results = IndicatorReportCore(
                    prj, frepf, Area, gtype, compete, status, oblast, period,amount, indicatorcategory);

                    ViewData["results2"] = results;
            }



            //add List Filters.  //enable Report View enabled if not null.

            if (frepf.isIndicator != null && frepf.isIndicator.Value == true)
            {
                frepf.isIndicator = true;
            }
            else
            {
                frepf.isIndicator = false; 
            }

            if (Area != null)
            {
                frepf.IsArea = true;
            }
            if (gtype != null)
            {
                frepf.IsGrantType = true;
            }
            if (compete != null)
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
            //=====

            return View(frepf);

        }

        public Dictionary<IndicatorContainerType, Dictionary<IndicatorLabelContentCategory, List<IndicatorRepContainer>>> IndicatorReportCore(
            IQueryable<Project> prjList, FinReportFilter frepf, List<String> Area, List<String> gtype,
            List<String> compete, List<String> status, List<String> oblast, List<String> period, List<String> amount, List<String> indicatorcategory)
        {

            AppDropDownsService ServiceDDL = new AppDropDownsService();
            List<IndicatorLabelContentCategory> IndContCats = new List<IndicatorLabelContentCategory>();
             var regions = new List<RegionList>();
             var areas = new List<ProgramAreaList>();
             var statusList = new List<ProposalStatusList>();
             var gtypeList = new List<GrantTypeList>();
             var competeList= new List<CompetitionCodeList>();

            //create IndicatorContentCategory List to be passed.
             if (indicatorcategory != null)
             {
                 foreach (string s in indicatorcategory)
                 {
                     IndContCats.Add(ServiceDDL.GetIndicatorLabelContentCategory(Convert.ToInt32(s)));
                 }
             }

            var LLVsContainer= new Dictionary<IndicatorContainerType, Dictionary<IndicatorLabelContentCategory, List<IndicatorRepContainer>>>();
            // Dictionary<ContainerType, List<VsContainer>> LLVsContainer = null; 
            if (oblast != null && oblast.Count > 0 && indicatorcategory != null && indicatorcategory.Count > 0)
            {
                //create RegionsList List to be passed.
                foreach (string s in oblast)
                {   if(!s.Contains("All"))   //skip ALL.
                    regions.Add(ServiceDDL.GetRegionList().FirstOrDefault(w => w.DDID == Convert.ToInt32(s)));
                }
                var result = FillContainer(prjList, frepf, IndicatorContainerType.OblastVsIndicatorLabelCategory,
                                              IndContCats, regions, null, null, null);
                LLVsContainer.Add(IndicatorContainerType.OblastVsIndicatorLabelCategory, result);

            }

            if (Area != null && Area.Count > 0 && indicatorcategory != null && indicatorcategory.Count > 0)
            {
                foreach (string s in Area)
                {
                    if (!s.Contains("All"))   //skip ALL.
                        areas.Add(ServiceDDL.GetProgramAreaList().FirstOrDefault(w => w.ProgramAreaCodeID == Convert.ToInt32(s)));
                }
                var result = FillContainer(prjList, frepf, IndicatorContainerType.AreaVsIndicatorLabelCategory,
                                              IndContCats, null, areas, null, null);
                LLVsContainer.Add(IndicatorContainerType.AreaVsIndicatorLabelCategory, result);
            }

            if (gtype != null && gtype.Count > 0 && indicatorcategory != null && indicatorcategory.Count > 0)
            {
                foreach (string s in gtype)
                {
                    if (!s.Contains("All"))   //skip ALL.
                        gtypeList.Add(ServiceDDL.GetGrantTypeList().FirstOrDefault(w => w.GrantTypeCodeID == Convert.ToInt32(s)));
                }
                var result = FillContainer(prjList, frepf, IndicatorContainerType.TypeVsIndicatorLabelCategory,
                                              IndContCats, null, null,  gtypeList, null);
                LLVsContainer.Add(IndicatorContainerType.TypeVsIndicatorLabelCategory, result);
            }

            if (compete != null && compete.Count > 0 && indicatorcategory != null && indicatorcategory.Count > 0)
            {
                foreach (string s in compete)
                {
                    if (!s.Contains("All"))   //skip ALL.
                        competeList.Add(ServiceDDL.GetCompetitionCodeList().FirstOrDefault(w => w.CompetitionCodeID == Convert.ToInt32(s)));
                }
                var result = FillContainer(prjList, frepf, IndicatorContainerType.RoundVsIndicatorLabelCategory,
                                              IndContCats, null, null,  null, competeList);
                LLVsContainer.Add(IndicatorContainerType.RoundVsIndicatorLabelCategory, result);
            }
            bool areaVsRound = false;
            //more coming here.


            return LLVsContainer;
        }

       // private static Dictionary<IndicatorContainerType, Dictionary<IndicatorLabelContentCategory, List<IndicatorRepContainer>>> 
        private static Dictionary<IndicatorLabelContentCategory, List<IndicatorRepContainer>> FillContainer(
            IQueryable<Project> prjList, FinReportFilter frepf, IndicatorContainerType ct, List<IndicatorLabelContentCategory> IndContCats, List<RegionList> regions,
            List<ProgramAreaList> areas,  List<GrantTypeList> gtypeList, List<CompetitionCodeList> competeList)
            //Dictionary<ContainerType, List<VsContainer>> LLVsContainer)
        {
            bool areaVsType = false;
            // List<List<VsContainer>> LLVsContainer = new List<List<VsContainer>>();  //container to hold List Results.

          

            var grandDictionary = new Dictionary<IndicatorContainerType, Dictionary<IndicatorLabelContentCategory, List<IndicatorRepContainer>>>();
            var dictResult = new Dictionary<IndicatorLabelContentCategory, List<IndicatorRepContainer>>();
            var rep = new LReportsRepository();
            List<IndicatorRepContainer> result2 = null;
                //doAreaVsType  //I order list By Field1, needed to create table. The next comes Grouper SUMMER->Counter! Just does math.
                switch (ct)
                {
                    case IndicatorContainerType.OblastVsIndicatorLabelCategory:
                       
                        foreach (IndicatorLabelContentCategory indContCat in IndContCats)
                        {
                            result2 = rep.OblastVsIndicatorLabelCategory(prjList, indContCat.ID, regions);
                            dictResult.Add(indContCat, result2);
                        }
                       // grandDictionary.Add(IndicatorContainerType.OblastVsIndicatorLabelCategory, dictResult); 
                        break;

                    case IndicatorContainerType.AreaVsIndicatorLabelCategory:
                        foreach (IndicatorLabelContentCategory indContCat in IndContCats)
                        {
                            result2 = rep.AreaVsIndicatorLabelCategory(prjList, indContCat.ID, areas);
                            dictResult.Add(indContCat, result2);
                        }
                      //  grandDictionary.Add(IndicatorContainerType.AreaVsIndicatorLabelCategory, dictResult);
                        break;
                    case IndicatorContainerType.RoundVsIndicatorLabelCategory:
                        foreach (IndicatorLabelContentCategory indContCat in IndContCats)
                        {
                            result2 = rep.RoundVsIndicatorLabelCategory(prjList, indContCat.ID, competeList);
                            dictResult.Add(indContCat, result2);
                        }
                      //  grandDictionary.Add(IndicatorContainerType.RoundVsIndicatorLabelCategory, dictResult);
                        break;
                    case IndicatorContainerType.TypeVsIndicatorLabelCategory:
                        foreach (IndicatorLabelContentCategory indContCat in IndContCats)
                        {
                            result2 = rep.TypeVsIndicatorLabelCategory(prjList, indContCat.ID, gtypeList);
                            dictResult.Add(indContCat, result2);
                        }
                      //  grandDictionary.Add(IndicatorContainerType.TypeVsIndicatorLabelCategory, dictResult);
                        break;
                }

                //Dictionary<AmountTypes, List<VsContainer>> resultDictionary =
                //    new Dictionary<AmountTypes, List<VsContainer>>();
                //resultDictionary.Add(AmountTypes.AmountRequested, result);

                //if (LLVsContainer.ContainsKey(ct))
                //{
                //    if (result != null)
                //        LLVsContainer[ct].Add(AmountTypes.AmountRequested, result); //.AddRange(resultDictionary);
                //}
                //else
                //{
                //    LLVsContainer.Add(ct, resultDictionary); //add result to Container List.
                //}

            return dictResult; 
            //return grandDictionary; 
        }
    }
}