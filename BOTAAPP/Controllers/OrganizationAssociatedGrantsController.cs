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
    public class OrganizationAssociatedGrantsController : BOTAController
    {
      OrganizationService orgservice;
      ProjectService projservice; 

       public OrganizationAssociatedGrantsController()
        {
            orgservice = new OrganizationService();
            projservice = new ProjectService(); 


        
        }

        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo"); 
        }

        // GET: /Organization/View
        /*public ActionResult View()
        {
            return View();
        }*/

        // GET: /Organization/View/1
        public ActionResult View(int? id)
        {

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            
            IEnumerable<Project> _proposal = orgservice.GetAssociatedGrants(id.Value);
            //ViewData["GrantTypeList"] = new SelectList(projservice.GrantTypeList(), "GrantTypeCodeID", "GrantTypeText");
            ViewData["GrantTypeList"] = projservice.GrantTypeList();
            // ViewData["LegalStatList"] = new SelectList(orgservice.GetLegalStatusList(),"LegStatListID","LegName");
            ViewData["OrgID"] = id.Value;
            return View(_proposal.ToList());
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(Organization item/*, ICollection<Contact> Contacts*/, int? id, string actioncase, int? h_contactid)
        {

            switch (actioncase)
            {

                case "Search":
                    IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                    TempData["searchresult"] = orgs;
                    break;
                
                case "Update":

                     break;

                case "Delete":
                    if (h_contactid.HasValue)
                    {
                        int hello; 
                      //  orgservice.DeleteOrganization(h_contactid);
                    }
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
