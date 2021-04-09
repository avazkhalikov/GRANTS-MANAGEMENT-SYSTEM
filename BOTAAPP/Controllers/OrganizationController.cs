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
   
    public class OrganizationController : BOTAController
    {
        //
        // GET: /Organization/
        // GET: /Organization/Index
        // GET: /Organization/Proposal
        OrganizationService orgservice;
        public OrganizationController()
        {
            orgservice = new OrganizationService();
        
        }

        public ActionResult Index()
        {
            return View();
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
            
            Organization _organization = orgservice.GetOrganizationOfCurrentProposal(id.Value);
            /*COMMENT:  get from model way. ViewData in View.aspx //works.
             <%= Html.DropDownListFor(
    model => model.SelectedBikeValue,
    Model.Bikes.Select(
        x => new SelectListItem {
                 Text = x.Name,
                 Value = Url.Action("Details", "Bike", new { bikeId = x.ID }),
                 Selected = x.ID == Model.ID,
        }
)) %>             
             */
            ViewData["LegalStatList"] = new SelectList(orgservice.GetLegalStatusList(),"LegStatListID","LegName");
            ViewData["ProposalID"] = id.Value;
            return View(_organization);
        }


        /*
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult View(int? id,Organization item)
        {
            return View();
        } */


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(Organization item, ICollection<Contact> Contacts, int? id, string actioncase)
        {

            switch (actioncase)
            {

                case "Update":
                    item.Contacts = ConvertBOTA.ToEntitySet(Contacts);
                    orgservice.UpdateOrganization(item, id.Value);
                    break;
                           
              case "Delete": orgservice.DeleteOrganization(id.Value);
                             break; 
            
            }

            return RedirectToAction("View", new { id = id });

          /*  Organization _org = orgservice.GetOrganizationOfCurrentProposal(id.Value);
            if (_org != null)
            {
                item.EntityKey = _org.EntityKey;
                
            } */
            
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult test(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<form method=\"post\" name=\"form1\" id=\"form1\" action=\"/Organization/test/"+id.ToString()+"?test1=one&test2=two\">");
            sb.AppendLine("<input type=\"text\" name=\"i1\" id=\"i1\" />");
            sb.AppendLine("<input type=\"submit\" name=\"i2\" id=\"i2\" value=\"Submit\" />");
            sb.AppendLine("</form>");
            sb.AppendLine("Test!"+id.ToString());
            return Content(sb.ToString());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult test(FormCollection form1, string test1, string test2)
        {
            string test;
            test = test1 + " " + test2;

            for (int i = 0; i < form1.Count;i++)
            {
                test += "<br />" + form1[i].ToString();
            }
            return Content(test);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Test3(int id)
        {
            
            Organization o = orgservice.GetOrganizationOfCurrentProposal(id);
            
            return View(o);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Test3(Organization o, IEnumerable<Contact> contacts)
        {
            o.Contacts = ConvertBOTA.ToEntitySet(contacts);
            return View(o);
        }

        
    }
    
    
}
