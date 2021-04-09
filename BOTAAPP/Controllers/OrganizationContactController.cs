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
    public class OrganizationContactController : BOTAController
    {
        OrganizationService orgservice;
        public OrganizationContactController()
        {
            orgservice = new OrganizationService();
        
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
            
             Organization _organization = orgservice.GetOrganizationContactByID(id.Value);
          
             // ViewData["LegalStatList"] = new SelectList(orgservice.GetLegalStatusList(),"LegStatListID","LegName");
              ViewData["OrgID"] = id.Value;
              return View(_organization);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(Organization item, ICollection<Contact> Contacts, int? id, string actioncase, int? h_contactid)
        {

            switch (actioncase)
            {
                case "Insert":
                   
                    bool status = orgservice.InsertNewOrgContact(id.Value);
                    break;

                case "Search":
                    IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                    TempData["searchresult"] = orgs;
                    break;
                
                case "Update":
                                     
                     orgservice.UpdateOrganizationContact(ConvertBOTA.ToEntitySet(Contacts)); 
                     break;

                case "Delete":
                     if (h_contactid.HasValue)
                     {
                         bool deleted = orgservice.DeleteOrgContact(h_contactid.Value);
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
