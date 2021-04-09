using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTAMVC3.Helpers;
using BOTACORE.CORE.Services.Impl;
using BOTACORE.CORE.Domain;

namespace BOTAMVC3.Controllers
{
    public class IndicatorTemplatesController : BOTAController
    {
        //
        // GET: /IndicatorTemplates/

        public ActionResult Index()
        {
            GrantTypeListService st = new GrantTypeListService();
            IEnumerable<GrantTypeList> gt = st.GetAll();
            return View(gt);
        }

        public ActionResult Edit(int id)
        {
            IndicatorService ins = new IndicatorService();
            IEnumerable<IndicatorCategoryLabel> m=ins.GetIndicatorTemplateCategories(id);

            SelectList ddlIndicator = new SelectList(ins.GetIndicatorLabels(id), "IndicatorLabelID", "Text");
            ViewData["ddlIndicator"] = ddlIndicator;
            SelectList ddlCategory = new SelectList(ins.GetIndicatorCategoryLabels(id), "IndicatorCategoryLabelID", "Text");
            ViewData["ddlCategory"] = ddlCategory;
            ViewData["GrantTypeCodeID"] = id;
            ViewData["grantid"] = id;
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewIndicatorTemplateItem(int ddlIndicator, int ddlCategory, int GrantTypeCodeID)
        {
            IndicatorService ins = new IndicatorService();
            IndicatorTemplateItem item = new IndicatorTemplateItem();
            item.IndicatorLabelID = ddlIndicator;
            item.IndicatorCategoryLabelID = ddlCategory;
            ins.InsertIndicatorTemplateItem(item);
            return RedirectToAction("Edit", new { id = GrantTypeCodeID });
        }

        public ActionResult DeleteIndicatorTemplateItem(int id, int grantid)
        {
            IndicatorService ins = new IndicatorService();
            ins.DeleteIndicatorTemplateItem(id);

            return RedirectToAction("Edit", new { id = grantid });
        }

    }
}
