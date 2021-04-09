
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
using BOTACORE.CORE.DataAccess.Impl;
using System.Data;


namespace BOTAMVC3.Controllers
{
    public class StaffReportController : Controller
    {
        //
        // GET: /StaffReport/

        public ActionResult Index()
        {
            AppDropDownsService ServiceDDL = new AppDropDownsService();
            ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
            ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList(); 
            SSPStaffRepository ss = new SSPStaffRepository();
            List<StaffGrantTypeGrouped> staffgroupedList = new List<StaffGrantTypeGrouped>();
            StaffGrantTypeGrouped sffg = null;

            //ONE Staff!
           // List<ViewStaffMyProject> StaffGrant = ss.GetSSPStaffProjects(2).ToList();
            ViewData["StaffGrant"] = ss.GetSSPStaffProjects(2,1).ToList(); 


           //ALL
            List<StaffGrantHolder> gf = ss.GetSSPStaffsProjects(2).ToList();
         
           foreach(StaffGrantHolder sf in gf)
           {
               try
               {
                   sffg = new StaffGrantTypeGrouped();
                   sffg.grantTypeCount = sf.grantType
                       .GroupBy(o => o.GrantTypeList.GrantTypeCodeID).ToDictionary(g => g.Key, g => g.Count());
                   sffg.FirstName = sf.FirstName;
                   sffg.LastName = sf.LastName;
                   sffg.sspid = sf.sspid;
               }
                catch
                {
                }

               if (sffg != null)
               {
                   staffgroupedList.Add(sffg);
                   sffg = null;
               }
           }
           

            return View(staffgroupedList);
        }


         public ActionResult allstaffgt(int? id, int? rt, int? rti, int? status)
         {
             
             AppDropDownsService ServiceDDL = new AppDropDownsService();
             ViewData["GrantType"] = ServiceDDL.GetGrantTypeList();
             ViewData["ProgramArea"] = ServiceDDL.GetProgramAreaList(); 
             SSPStaffRepository ss = new SSPStaffRepository();
             StaffGrantTypeGrouped sffg = null;

             List<StaffGrantTypeGrouped> staffgroupedList = new List<StaffGrantTypeGrouped>();
             //ALL
             List<StaffGrantHolder> gf = null;

             if (status.HasValue)
             {
                 gf = ss.GetSSPStaffsProjects(status.Value).ToList(); //could be active =1 or all=2.
             }
             else
             {
                 gf = ss.GetSSPStaffsProjects(2).ToList();  //default get all grants!
             }

             if (rt.HasValue && rt.Value > 0)
             {
                 ViewData["ReportType"] = rt.Value;
             }
            
             #region --- test ---
             //foreach (StaffGrantType sf in gf)
             //{
             //    try
             //    {
             //        sffg = new StaffGrantTypeGrouped();
             //        sffg.grantTypeCount = sf.grantType
             //            .GroupBy(o => o.GrantTypeList.GrantTypeCodeID).ToDictionary(g => g.Key, g => g.Count());
             //        sffg.FirstName = sf.FirstName;
             //        sffg.LastName = sf.LastName;
             //        sffg.sspid = sf.sspid;
             //    }
             //    catch
             //    {
             //    }

             //    if (sffg != null)
             //    {
             //        staffgroupedList.Add(sffg);
             //        sffg = null;
             //    }
             //}

#endregion             //All staff.
             ViewData["StaffGrantTypeList"] = gf;
             
             
             //ONE Staff!
             // List<ViewStaffMyProject> StaffGrant = ss.GetSSPStaffProjects(2).ToList();
             int reptype = 2;
             if (rti.HasValue && rti.Value > 0)
             {
                 reptype = rti.Value;
                 ViewData["ReportTypeInd"] = rti.Value;
             }
             //else
             //{
             //    ViewData["ReportTypeInd"] = 1;
             //}

             if (id.HasValue && id.Value > 0)
             {
                 ViewData["StaffGrant"] = ss.GetSSPStaffProjects(id.Value, reptype).ToList();
             }
             //else
             //{
             //    ViewData["StaffGrant"] = ss.GetSSPStaffProjects(1, reptype).ToList();
             //}

                 return View();
             
             //return View(staffgroupedList);
         }
    }
}
