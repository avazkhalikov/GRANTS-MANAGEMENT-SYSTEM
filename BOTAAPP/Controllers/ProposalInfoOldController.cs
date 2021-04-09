using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using BOTACORE.CORE.Services;
using System.Text;
using System.Data.Linq;
using BOTAMVC3.Helpers;

namespace BOTAMVC2.Controllers
{
    public class ProposalInfoController : BOTAController
    {
        //
        // GET: /ProposalInfo/

        public ActionResult Index()
        {
            return RedirectToAction("Search", "ProposalInfo"); 
        }

        ProjectService projservice;
        OrganizationService orgservice; 
        public ProposalInfoController()
        {
            projservice = new ProjectService();
            orgservice = new OrganizationService();         
        }

        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Search(/*IEnumerable<ProposalInfo> propinfo*/)
        {
            
            if (session.LoggedIn)
            {
                ViewData["UserWelcome"] = "Welcome " + session.CurrentUser.FirstName + "  " + session.CurrentUser.LastName + " " + session.CurrentUser.MiddleName; 
            }
          
            /*
            if (TempData["searchresult"] != null)
            {
                ViewData["Results"] = TempData["searchresult"];
            }
            return View(_organization.General);
            */
           return View(); 
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(int? id, bool? isChecked)
        {          

            int num;
            bool isNumeric = int.TryParse(Request.Form["SearchString"], out num);
            IEnumerable<ProposalInfo> propInfo = null; 
            
           // if(Request.Form["SearchString"].ToString().Contains(

            if (isChecked!=null && isChecked.Value)// && !isNumeric)
            {
                propInfo = projservice.SearchProposalKeyword(Request.Form["SearchString"].ToString()); 
            }
            else if (isNumeric)
            {
                propInfo = projservice.SearchProposalByID(num);
            

            }
            else
            {
               propInfo = projservice.SearchProposalByName(Request.Form["SearchString"].ToString());
            }


            ViewData["Results"] = propInfo.ToList(); // propInfo.OrderByDescending(x =>
               // Convert.ToInt32(x.Project.Label.ToString().Split('-').ElementAt(2))).ToList(); 
    

            if (session.LoggedIn)
            {
                ViewData["UserWelcome"] = "Welcome " + session.CurrentUser.FirstName + session.CurrentUser.LastName;
            }
           
            return View();
        }

     



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Insert(int? id)
        {
            //Get all Organizations List.
            IEnumerable<General> ResultOrgList = projservice.GetAllOrganizationList();
            ViewData["LastEnteredProject"] = projservice.LastEnteredProject(); 
         

        //    var lst = new SelectList(ResultOrgList, "OrgID", "Name").ToList();
  
 //           lst.Insert(0, new SelectListItem { Text = "Not Selected", Value = "0" });


   //         ViewData["OrganizationList"] = lst; 

            IEnumerable<General> ResultOrgListSorted = ResultOrgList.OrderBy(x => x.NameRu);

            ViewData["OrganizationList"] = new SelectList(ResultOrgListSorted, "OrgID", "NameRu");


            return View(new { id = id });  
        }



        /// <summary>
        /// Creates the new Proposal!! 
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(int? OrgID, string projtitle, string projorganization, string projectlabel)
        {
            int? id = -1; ;
            int OrganizationID = 0;


            if (string.IsNullOrEmpty(projtitle))
            {
                projtitle = "-";
            }
            
            if (!OrgID.HasValue)
            {
                OrgID = -1; 
            }

            //Create or select ORGID Foreign key.
            if (OrgID.Value == 1 && projorganization == "")
            {
                OrganizationID = 1; 
            }

            if (OrgID.Value > 1)
            {
                // Any other OrgID selected.   
                OrganizationID = Convert.ToInt32(OrgID);
            }


            if (projorganization != "")  //dirty.
            {
                //New OrgID creation required. 
                 OrgID = orgservice.CreateNewOrganization(projorganization); 
            }


            //Create New Project.
            if (OrgID != -1 && projtitle.Any() && projectlabel.Any())
            {
                id = projservice.CreateNewProposal(OrgID.Value, projtitle, projectlabel);
              
            }


            if (id.HasValue)
            {
                if (id.Value > 0)
                {
                    return RedirectToAction("Proposal", new { id = id });
                }
                else
                {
                    ViewData["Error"] = "Error: Such Project Already Exists, please try different project ID! or Fatal Error may have happened";
                }
               
            }


         
            ViewData["PropID"] = id.Value;
            return View(new { id = id });
           
           
        }


        public ActionResult ProposalDelete(int? id)
        {
            if(id.HasValue)
            projservice.ProposalDelete(id.Value); 
            
            
            return RedirectToAction("Index");
        }
            


        // GET: /ProposalInfo/View/1
        public ActionResult Proposal(int? id)
        {
            

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            Project _project = projservice.GetProposalInfo(id.Value);   //This will happen In MenuITEM Controller orgservice.GetOrganizationGeneralOfCurrentProposal(id.Value);

            //DataBinding: 'BOTACORE.CORE.Domain.ProgramAreaList' does not contain a property with the name 'ProgramAreaListID'.
           

 
            // CompetitionCodeList comp;
            //comp.CompetitionCodeID
            //comp.CodeText
            ViewData["ProgramAreaList"] = new SelectList(projservice.GetProgramAreaList(), "ProgramAreaCodeID", "ProgramAreaText");
            ViewData["CompetitionCodeList"] = new SelectList(projservice.CompetitionCodeList(), "CompetitionCodeID", "CodeText");
            ViewData["ProposalStatusList"] = new SelectList(projservice.ProposalStatusList(), "ProposalStatusID", "ProposalStatusText");
            ViewData["GrantTypeList"] = new SelectList(projservice.GrantTypeList(),"GrantTypeCodeID", "GrantTypeText");
            ViewData["LFIndicatorList"] = new SelectList(projservice.LFIndicatorList(), "LFIndicatorID", "CodeText");
           
            List<ProjLocationList> l = projservice.ProjLocationList().ToList(); 

            ViewData["ProjLocList"] = new SelectList(projservice.ProjLocationList(), "ID", "Name");
            
          

            ViewData["ProposalID"] = id.Value;

            //Now we can get from session any time the selected ProposalID.
            if(_project != null)
            {
                
              session.ProjectID = _project.ProjectID;
              try
              {
                  session.OrgID = _project.OrgID.Value;
                  session.OrganizationName = _project.Organization.General.NameRu;
              }
              catch
              {
                  session.OrgID = -1;
                  session.OrganizationName = "No Org Was Assigned, Please assign organization to this project";
              }
              session.ProjectLabel = _project.Label;
              

            // Session["ProposalID"] = 
           //  Session["OrgID"] = _project.OrgID; 
            }

            return View(_project);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int? id, Project project)
        {
                       
            //Update Proposal Info.
            projservice.ProposalInfoUpdate(project);

            return RedirectToAction("Proposal", new { id = id });
    
        

        }


    }
}
