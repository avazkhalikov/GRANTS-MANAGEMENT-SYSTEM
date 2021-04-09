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
    public class OrganizationOtherFundersController : BOTAController
    {
       OrganizationService orgservice;
       public OrganizationOtherFundersController()
        {
            orgservice = new OrganizationService();
        
        }

        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo"); 
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int? OtherFundID, int? id)
        {
            //Insert SSPTAFF into Project!

            if (!id.HasValue)
                return View();

            if (OtherFundID.HasValue && id.HasValue)
            {
                bool deleted = orgservice.DeleteOrgOtherFunders(OtherFundID.Value);
            }

            return RedirectToAction("View", new { id = id.Value });
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
            
             Organization _organization = orgservice.GetOrganizationOtherFunderByID(id.Value);
          
             // ViewData["LegalStatList"] = new SelectList(orgservice.GetLegalStatusList(),"LegStatListID","LegName");
              ViewData["OrgID"] = id.Value;
              return View(_organization);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(List<OtherFunder> otherfunders, Organization item/*, ICollection<Contact> Contacts*/, int? id, string actioncase, int? h_contactid)
        {

            switch (actioncase)
            {

                case "Insert":
                   
                    bool status = orgservice.InsertNewOrgOtherFunders(id.Value);
                    break;

                case "Search":
                    IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                    TempData["searchresult"] = orgs;
                    break;

                case "Update":

                    if (otherfunders != null)
                    {
                        IEnumerable<OtherFunder> enumotherfunders = otherfunders.AsEnumerable();

                        bool updstat = orgservice.UpdateOtherFunders(enumotherfunders);
                    }
                    //orgservice.UpdateOrganizationAddress(ConvertBOTA.ToEntitySet(addresses));

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
