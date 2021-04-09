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


namespace BOTAMVC3.Controllers
{
    public class ProjectKeysController : BOTAController
    {
        //
        // GET: /ProjectKeys/

        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo"); 
        }

        ProjectService projservice;
        public ProjectKeysController()
        {
            projservice = new ProjectService();
        
        }

        // GET: /ProposalInfo/View/1
        public ActionResult View(int? id)
        {
            

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

         
            IEnumerable<TheKey> theKey_ = projservice.GetProjectKeysByID(id.Value); 
                    
           
            ViewData["ProposalID"] = id.Value;
            ViewData["TheKeys"] = theKey_;
                        
            return View();
        }
       

        

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InsertKeyList(int? id, string NewKey)
        {
           bool Inserted = projservice.InsertProjectKey(NewKey, id.Value);
           return RedirectToAction("View", new { id = id.Value });
        }


        public ActionResult DeleteKeyList(int? KeyID, int? id)
        {

           bool Inserted = projservice.DeleteProjectKeyByID(KeyID.Value, id.Value);
           return RedirectToAction("View", new { id = id.Value });

        }

        


    }
}
