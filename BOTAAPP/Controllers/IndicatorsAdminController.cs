using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Services.Impl;
using BOTACORE.CORE.Domain;
using BOTAMVC3.Helpers;

namespace BOTAMVC3.Controllers
{
    public class IndicatorsAdminController : BOTAController
    {
        //
        // GET: /IndicatorsAdmin/

        public ActionResult Index()
        {
            GrantTypeListService st = new GrantTypeListService();
            IEnumerable<GrantTypeList> gt = st.GetAll();
            return View(gt);
        }

        public ActionResult Edit(int id)
        {
            IndicatorService ins = new IndicatorService();
            ViewData["IndicatorLabels"]=ins.GetIndicatorLabels(id);
            ViewData["IndicatorCategoryLabels"]=ins.GetIndicatorCategoryLabels(id);
            ViewData["grantid"] = id;
            return View();
        }

        public ActionResult EditIndicatorCategoryLabel(int id, int grantid)
        {
            IndicatorService ins = new IndicatorService();
            IndicatorCategoryLabel m=ins.GetIndicatorCategoryLabel(id);
            ViewData["grantid"] = grantid;
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditIndicatorCategoryLabel(IndicatorCategoryLabel item)
        {
            IndicatorService IService = new IndicatorService();
            IService.UpdateIndicatorCategoryLabel(item);
            return RedirectToAction("Edit", new { id=item.GrantTypeCodeID});
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewIndicatorCategoryLabel(IndicatorCategoryLabel item)
        {
            IndicatorService IService = new IndicatorService();
            IService.InsertIndicatorCategoryLabel(item);
            return RedirectToAction("Edit", new { id=item.GrantTypeCodeID});
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewIndicatorLabel(IndicatorLabel item)
        {
            IndicatorService IService = new IndicatorService();
            IService.InsertIndicatorLabel(item);
            return RedirectToAction("Edit", new { id = item.GrantTypeCodeID });
        }

        public ActionResult DeleteIndicatorCategoryLabel(int id, int grantid)
        {
            IndicatorService IService = new IndicatorService();
            IService.DeleteIndicatorCategoryLabel(id);
            return RedirectToAction("Edit", new { id=grantid});
        }

        //
        public ActionResult EditIndicatorLabel(int id, int grantid)
        {
            IndicatorService IService = new IndicatorService();
            IndicatorLabel ilabel = IService.GetIndicatorLabel(id);
            ViewData["grantid"] = grantid;
            return View(ilabel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditIndicatorLabel(IndicatorLabel item)
        {
            IndicatorService IService = new IndicatorService();
            IService.UpdateIndicatorLabel(item);
            return RedirectToAction("Edit", new { id = item.GrantTypeCodeID });
        }

        public ActionResult DeleteIndicatorLabel(int id, int grantid)
        {
            IndicatorService IService = new IndicatorService();
            IService.DeleteIndicatorLabel(id);
            return RedirectToAction("Edit", new { id=grantid});
        }//

    }
}
