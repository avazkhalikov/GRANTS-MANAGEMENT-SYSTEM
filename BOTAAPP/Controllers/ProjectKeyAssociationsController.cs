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
    public class ProjectKeyAssociationsController : Controller // BOTAController
    {
        //
        // GET: /ProjectKeyAssociations/

      
        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo"); 
        }

        ProjectService projservice;
        SSPStaffService sspsservice; 
        public ProjectKeyAssociationsController()
        {
            projservice = new ProjectService();
            sspsservice = new SSPStaffService(); 
        
        }

        // GET: /ProposalInfo/View/1
        public ActionResult View(int? id)
        {
            

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }


           IEnumerable<ViewStaffProject> sspproj = sspsservice.GetSSPStaffProject(id.Value);
           ViewData["SSPStaffs"] =  sspsservice.GetSSPStaffListForProject(id.Value);
           ViewData["ProposalID"] = id.Value;

           return View(sspproj);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Insert(int? id)
        {

          //Get all SSP Staff List.
          IEnumerable<SSPStaff> slist = sspsservice.GetSSPStaffList();
          
          List<SelectListItem> selects=new List<SelectListItem>();
           
          if (slist!=null)
            {
               foreach(var item in slist)
               {
                  selects.Add(new SelectListItem(){Value=item.SSPStaffID.ToString(), Text=item.FirstName+" "+item.LastName+" "+item.MiddleName});
               }
            }


            ViewData["SSPStaffList"] = selects;
 

          return View(id.Value);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(int? SSPStaffID, int? id)
        {
            //Insert SSPTAFF into Project!

            if (!id.HasValue)
            return View();

            if (SSPStaffID.HasValue && id.HasValue)
            {
                bool StaffInserted = sspsservice.InsertSSPStaffIntoProject(SSPStaffID.Value, id.Value);
            }

            return RedirectToAction("View", new { id = id.Value });  
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int? SSPStaffID, int? id)
        {
            //Insert SSPTAFF into Project!

            if (!id.HasValue)
                return View();

            if (SSPStaffID.HasValue && id.HasValue)
            {
                bool StaffDeleted= sspsservice.DeleteSSPStaffFromProject(SSPStaffID.Value, id.Value);
            }

            return RedirectToAction("View", new { id = id.Value });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Update(int? id)
        {
            if (!id.HasValue)
                return View();

           
            List<int> current = (List<int>)TempData["current"]; 

            if (id.HasValue)
            {
              
               bool status =  sspsservice.UpdateStaffProject(id.Value, current);

                //  bool StaffDeleted = sspsservice.DeleteSSPStaffFromProject(SSPStaffID.Value, id.Value);
            }

            return RedirectToAction("View", new { id = id.Value });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(int? SSPStaffID, int? id, string actioncase, List<int> current)
        {


            switch (actioncase)
            {

                case "Insert":
                    
                    return RedirectToAction("Insert", new { id = id });
                //   IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                //   TempData["searchresult"] = orgs;


                case "Search":
                    //   IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                    //   TempData["searchresult"] = orgs;
                    break;
                case "Delete":

                    return RedirectToAction("Delete", new { id = id, SSPStaffID = SSPStaffID});
             
                case "Update":
                    TempData["current"] = current; 
                    return RedirectToAction("Update", new { id = id});

            }


            return RedirectToAction("View", new { id = id });

        }


    }
}
