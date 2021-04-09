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
    public class OrganizationGeneralController : BOTAController
    {
        //
        // GET: /OrganizationGeneral/

      //
        // GET: /Organization/
        // GET: /Organization/Index
        // GET: /Organization/Proposal
        OrganizationService orgservice;
        UserSession session;
        ProjectService projservice; 
        public OrganizationGeneralController()
        {
            projservice = new ProjectService();
            orgservice = new OrganizationService();
            session = new UserSession();
        
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


            [AcceptVerbs(HttpVerbs.Get)]
            public ActionResult CreateOrganization()
            {
              return View();
            }

           [AcceptVerbs(HttpVerbs.Get)]
            public ActionResult AssignOrgToProj(int? id)
            {
                int ProjectID = 0;
                Project proj = null;
                Organization org = null;

                if (session != null)
                {
                    ProjectID = session.ProjectID;
                }
                else
                {
                    RedirectToAction("Search", "ProposalInfo"); 
                }
              
               if (ProjectID > 0)  //get the project.
               {
                  proj = projservice.GetProjectByID(ProjectID);
               }
               else
               {
                   RedirectToAction("Search", "ProposalInfo");
               }


               if (id.HasValue)
               {
                   if (id.Value > 0)
                   { 
                       //get Org ByID.
                       org = orgservice.getOrganizationByID(id.Value); 
                   }
               }
               else
               {
                   RedirectToAction("Search", "ProposalInfo");
               }

               if (proj != null && org != null)
               {
                   proj.OrgID = org.OrgID;
                   projservice.UpdateProject(proj);
               }
               else
               {
                   RedirectToAction("Search", "ProposalInfo");
               }

               return RedirectToAction("View", new{id=id.Value}); 

            }


            [AcceptVerbs(HttpVerbs.Post)]
            public ActionResult CreateOrganization(string projorganization)
            {
                if (projorganization != "")  //dirty.
                {
                    OrganizationService orgservice = new OrganizationService();
                    //New OrgID creation required. 
                    int  OrgID = orgservice.CreateNewOrganization(projorganization);

                    if (OrgID > 0)
                    {
                        Organization _organization = orgservice.GetOrganizationGeneralByID(OrgID);

                        if (_organization != null)
                        {
                            return RedirectToAction("View", "OrganizationGeneral", new { id=OrgID});  //View(_organization.General);
                        }
                    }
                }


                return View(); 

            }

  

        // GET: /Organization/View/1
        public ActionResult View(int? id)
        {

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

              Organization _organization = orgservice.GetOrganizationGeneralByID(id.Value);   //This will happen In MenuITEM Controller orgservice.GetOrganizationGeneralOfCurrentProposal(id.Value);
          
              ViewData["LegalStatList"] = new SelectList(orgservice.GetLegalStatusList(),"LegStatListID","LegName");
              ViewData["OrgID"] = id.Value;


              if (TempData["searchresult"] != null)
              {
                  ViewData["Results"] = TempData["searchresult"];
              }              
            
               return View(_organization.General);
              
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(General item/*, ICollection<Contact> Contacts*/, int? id, string actioncase)
        {
            
            switch (actioncase)
            {

                case "Search": 
                           IEnumerable<Organization> orgs = orgservice.SearchOrganizationByName(Request.Form["SearchString"]);
                           TempData["searchresult"] = orgs;
                    break;
                case "Update":
                           orgservice.UpdateOrganizationGeneral(item);
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
