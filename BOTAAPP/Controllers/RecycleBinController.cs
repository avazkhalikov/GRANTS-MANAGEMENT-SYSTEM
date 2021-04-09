using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services;
using BOTACORE.CORE.DataAccess.Impl;


namespace BOTAMVC3.Controllers
{
   
    public class RecycleBinController : Controller
    {
        ProjectRepository prrep; 
        public RecycleBinController()
        {
           prrep = new ProjectRepository();
        }
        //
        // GET: /RecycleBin/
        public ActionResult Index()
        {
            //get List with isDeleted = true;
            //TODO: this is bad, must be refactored to Service which implements IService.
            //direct coupling.         

           IEnumerable<Project> proj = prrep.GetAllProjectIsDeleted();

           return View(proj);
        }

        public ActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
              bool result =  prrep.DeleteProjectPermanent(id.Value);
            }

           return RedirectToAction("Index"); 
        }


        public ActionResult Restore(int? id)
        {
            if (id.HasValue)
            {
                bool result = prrep.RestoreDeletedProject(id.Value);
            }

            return RedirectToAction("Index");
            
        }
    }
}
