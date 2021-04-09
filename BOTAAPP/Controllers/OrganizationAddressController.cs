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
    public class OrganizationAddressController : BOTAController
    {
        OrganizationService orgservice;
        BOTACORE.CORE.Services.Impl.AppDropDownsService ddservice; 
        public OrganizationAddressController()
        {
            orgservice = new OrganizationService();
            ddservice = new AppDropDownsService(); 
        
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

              IEnumerable<Address> _adds = orgservice.GetOrganizationAddressByID(id.Value);

              if (_adds != null)
              {
                  ViewData["RegionList"] = new SelectList(ddservice.GetRegionList(), "DDID", "DDNAME");
              } 

              ViewData["OrgID"] = id.Value;

             // Address ad;
              //ad.Region1.DDID; ad.Region1.AddressID
             if(_adds!=null)
                  return View(_adds);
             else
                 return View(null); 
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(List<Address> addresses,  Organization item/*, ICollection<Contact> Contacts*/, int? id, string actioncase, int? h_contactid)
        {

            switch (actioncase)
            {
                case "Insert": 
                    
                     bool status =  orgservice.InsertNewOrgAddress(id.Value);
                     break;

                case "Search":
                    IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                    TempData["searchresult"] = orgs;
                    break;
                
                case "Update":

                    if (addresses != null)
                    {
                        IEnumerable<Address> enumaddresses = addresses.AsEnumerable();

                        bool updstat = orgservice.UpdateOrgAdress(enumaddresses); 
                    }
                    //orgservice.UpdateOrganizationAddress(ConvertBOTA.ToEntitySet(addresses));

                     break;

                case "Delete":
                    if (h_contactid.HasValue)
                    {
                        bool deleted = orgservice.DeleteOrgAdress(h_contactid.Value); 
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
