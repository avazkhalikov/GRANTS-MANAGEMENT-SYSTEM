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
    public class ProjectFundingSourceController : BOTAController
    {
            // GET: /ProposalInfo/

        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Search", "ProposalInfo"); 
            }

            return View(id.Value);
        }

        ProjectService projservice;
        public ProjectFundingSourceController()
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

           ViewData["ProposalID"] = id;
           IEnumerable<FundingSource> _fundingSource = projservice.GetProjAllFundingSource(id.Value);
           FundingOrganization f; 
     
           ViewData["FundingOrganizationList"] = new SelectList(projservice.GetFundingOrganizationList(), "FundingSourceOrgID", "FundingOrgName");
           
           return View(_fundingSource);
        }

       

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Insert(int id)
        {
            ViewData["ProposalID"] = id;
            FundingSource f = new FundingSource();
            ViewData["FundingOrganizationList"] =  new SelectList(projservice.GetFundingOrganizationList(), "FundingSourceOrgID", "FundingOrgName");
            return View(f);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(FundingSource fundingSource, int id)
        {
            //Insert INTO DB.
            bool status = projservice.CreateProjFundingSource(fundingSource);

            return RedirectToAction("View", new { id = id });
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int FundingSourceID, int id)
        {
            bool status = projservice.DeleteProjFundingSource(FundingSourceID);

            return RedirectToAction("View", new { id = id });

        }





        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(List<FundingSource> fundingslist, int? id, string actioncase)
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
                case "Update":

                    //Update Initial Amount 

                    projservice.UpdateProjFundingSource(fundingslist);

                    //projservice.UpdateProjectBudget(proj); 
                    break;

                case "Delete":
                    // Delete organization GeneralInfo.
                    //  orgservice.DeleteOrganization(h_contactid);
                    break;

            }

            return RedirectToAction("View", new { id = id });

            /*  Organization _org = orgservice.GetOrganizationOfCurrentProposal(id.Value);
              if (_org != null)
              {
                  item.EntityKey = _org.EntityKey;
                
              } */

        }







    }
}
