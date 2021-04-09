using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using BOTAMVC3.Helpers;

namespace BOTAMVC3.Controllers
{
    public class IndicatorsController : BOTAController
    {
        //
        // GET: /Indicators/
        private IndicatorService IService;
        public IndicatorsController()
        {
            IService = new IndicatorService();
        }


        #region main actions
        public ActionResult Index()
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            //Session["ProjectID"] = "69";
            int ProjectID = Session["ProposalID"] != null ? int.Parse(Session["ProposalID"].ToString()) : 0;
            ViewData["ProjectID"] = ProjectID;
            IEnumerable<IndicatorCategoryLabel> m = IService.GetIndicatorCategories(ProjectID);
            ProjectService pe = new ProjectService();
          //  GrantType gt=pe.GetProposalInfo(ProjectID).ProjectInfo.Project.GrantType;
          //  GrantType gt = pe.GetProposalInfo(ProjectID).GrantType;
            GrantType gt = pe.GetGrantType(ProjectID);

            try
            {
                if (m != null && gt != null)
                {
                    if (gt.GrantTypeCodeID.HasValue)
                    {    //This First does not work in kaz server.
                        if (m.First().GrantTypeCodeID != gt.GrantTypeCodeID.Value)
                        {
                            IService.DeleteIndicator(ProjectID);
                            m = IService.GetIndicatorCategories(ProjectID);
                        }
                    }
                }
            }
            catch
            { 
            
            }
            
            //If doesn't exists in db, then take colection from Template
            if (m == null)
            {
                IService.InsertFromTemplate(ProjectID);
            }
            else
                if (m.Count() == 0)
                {
                    IService.InsertFromTemplate(ProjectID);
                }


            SelectList ddlIndicator = new SelectList(IService.GetIndicatorLabelsByProjectID(ProjectID), "IndicatorLabelID", "Text");
            ViewData["ddlIndicator"] = ddlIndicator;
            SelectList ddlCategory = new SelectList(IService.GetIndicatorCategoryLabelsByProjectID(ProjectID), "IndicatorCategoryLabelID", "Text");
            ViewData["ddlCategory"] = ddlCategory;
            return View(m);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult NewIndicatorLabel(string NewIndicatorName)
        //{
        //    if (Session["ProposalID"] == null)
        //    {
        //        return RedirectToAction("Search", "ProposalInfo");
        //    }

        //    IndicatorLabel item = new IndicatorLabel();
        //    item.Text = NewIndicatorName;
        //    //item.IndicatorID = int.Parse(Session["ProposalID"].ToString());
        //    IService.InsertIndicatorLabel(item);
        //    return RedirectToAction("Index");
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult NewIndicatorCategoryLabel(string NewIndicatorCategoryName)
        //{
        //    if (Session["ProposalID"] == null)
        //    {
        //        return RedirectToAction("Search", "ProposalInfo");
        //    }

        //    IndicatorCategoryLabel item = new IndicatorCategoryLabel();
        //    //item.IndicatorID = int.Parse(Session["ProposalID"].ToString());
        //    item.Text = NewIndicatorCategoryName;
        //    IService.InsertIndicatorCategoryLabel(item);
        //    return RedirectToAction("Index");
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewIndicatorItem(int ddlIndicator, int ddlCategory,
            int NewBaseline, int NewBenchmark, int NewFinal)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            IndicatorItem item = new IndicatorItem();
            //item.IndicatorID = int.Parse(Session["ProposalID"].ToString());
            item.IndicatorLabelID = ddlIndicator;
            item.IndicatorCategoryLabelID = ddlCategory;
            item.IndicatorID=int.Parse(Session["ProposalID"].ToString());
            item.Baseline = NewBaseline;
            item.Benchmark = NewBenchmark;
            item.Final = NewFinal;
            IService.InsertIndicatorItem(item);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            IndicatorItem item = IService.GetIndicatorItem(id);
            return View(item);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IndicatorItem item)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            IService.UpdateIndicatorItem(item);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
           if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            IService.DeleteIndicatorItem(id);
            return RedirectToAction("Index");
        }

        public ActionResult UserAction(List<IndicatorItem> IItems, string actioncase)
        {
            switch (actioncase.ToLower())
            {
                case "update":
                    if (IItems!=null)
                    {
                        List<IndicatorItem> newitems = new List<IndicatorItem>();
                        for (int i = 0; i < IItems.Count; i++)
                        {
                            IndicatorItem newitem = new IndicatorItem();
                            newitem.IndicatorID = IItems[i].IndicatorID;
                            newitem.IndicatorItemID = IItems[i].IndicatorItemID;
                            newitem.IndicatorLabelID = IItems[i].IndicatorLabelID;
                            newitem.IndicatorCategoryLabelID = IItems[i].IndicatorCategoryLabelID;
                            newitem.Baseline = IItems[i].Baseline;
                            newitem.Benchmark = IItems[i].Benchmark;
                            newitem.Final = IItems[i].Final;
                            
                            newitems.Add(newitem);
                        }
                        IService.UpdateIndicatorItems(newitems);
                    }
                    break;
                default: break;
            }

            return RedirectToAction("Index");
        }
        #endregion
/*
        #region IndicatorLabel
        public ActionResult IndicatorLabels()
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorService IService=new IndicatorService();
            //int ProjectID = Session["ProposalID"] != null ? int.Parse(Session["ProposalID"].ToString()) : 0;
            IEnumerable<IndicatorLabel> il = IService.GetIndicatorLabels();
            return View(il);
        }

        public ActionResult EditIndicatorLabel(int id)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorService IService=new IndicatorService();
            IndicatorLabel ilabel = IService.GetIndicatorLabel(id);
            return View(ilabel);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditIndicatorLabel(IndicatorLabel item)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorService IService = new IndicatorService();
            IService.UpdateIndicatorLabel(item);
            return RedirectToAction("IndicatorLabels");
        }

        public ActionResult DeleteIndicatorLabel(int id)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorService IService = new IndicatorService();
            IService.DeleteIndicatorLabel(id);
            return RedirectToAction("IndicatorLabels");
        }

        public ActionResult CreateIndicatorLabel()
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorLabel il = new IndicatorLabel();
            return View(il);
        }
        #endregion

        #region IndicatorCategoryLabel
        public ActionResult IndicatorCategoryLabels()
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorService IService = new IndicatorService();
            //int ProjectID = Session["ProposalID"] != null ? int.Parse(Session["ProposalID"].ToString()) : 0;
            IEnumerable<IndicatorCategoryLabel> il = IService.GetIndicatorCategoryLabels();
            return View(il);
        }

        public ActionResult EditIndicatorCategoryLabel(int id)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorService IService = new IndicatorService();
            IndicatorCategoryLabel ilabel = IService.GetIndicatorCategoryLabel(id);
            return View(ilabel);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditIndicatorCategoryLabel(IndicatorCategoryLabel item)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorService IService = new IndicatorService();
            IService.UpdateIndicatorCategoryLabel(item);
            return RedirectToAction("IndicatorCategoryLabels");
        }

        public ActionResult DeleteIndicatorCategoryLabel(int id)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorService IService = new IndicatorService();
            IService.DeleteIndicatorCategoryLabel(id);
            return RedirectToAction("IndicatorCategoryLabels");
        }

        public ActionResult CreateIndicatorCategoryLabel()
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            IndicatorCategoryLabel il = new IndicatorCategoryLabel();
            return View(il);
        }
        #endregion

*/
    }
}
