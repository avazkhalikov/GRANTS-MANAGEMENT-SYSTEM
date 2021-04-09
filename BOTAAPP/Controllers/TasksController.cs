using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using BOTAMVC3.Helpers;

namespace BOTAMVC3.Controllers
{
    public class TasksController : BOTAController
    {
        //
        // GET: /Tasks/

        public ActionResult Index(int? filter, int? alltasks)
        {
            /*if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }*/

            ProjectEventService pes = new ProjectEventService();
            //int ProjectID = Session["ProposalID"] != null ? int.Parse(Session["ProposalID"].ToString()) : 0;
            IEnumerable<ProjectEvent> pe;
           // List<ProjectEvent> pelist; 
            if (alltasks.HasValue)
            {
                if (alltasks.Value==0)
                {
                    pe = pes.GetProjectEventListScheduledMy(session.CurrentUser.SSPStaffID);
                }
                else
                {
                    pe = pes.GetProjectEventListScheduled();
                  //  pelist = pes.GetProjectEventListScheduled().ToList(); 
                }
            }
            else
            {
                pe = pes.GetProjectEventListScheduledMy(session.CurrentUser.SSPStaffID);
            }
            

            SSPStaffService sss = new SSPStaffService();
            IEnumerable<SSPStaff> slist = sss.GetSSPStaffList();
            ViewData["staff"] = slist;

            IEnumerable<ProjectEvent> OverdueEvents= from p in pe 
                                where p.ScheduledDate < DateTime.Now 
                                select p;
            IEnumerable<ProjectEvent> OverdueGrantEvents = from p in OverdueEvents
                                                           where p.SSPOrGrantee == true
                                                           select p;
            IEnumerable<ProjectEvent> OverdueNonGrantEvents = from p in OverdueEvents
                                                              where p.SSPOrGrantee == false
                                                           select p;
            ViewData["OverdueGrantEvents"] = OverdueGrantEvents;
            ViewData["OverdueNonGrantEvents"] = OverdueNonGrantEvents;
            List<SelectListItem> ff = new List<SelectListItem>();
            ff.Add(new SelectListItem() { Value = "-1", Text = "ALL" });
            ff.Add(new SelectListItem() { Value="1", Text="OVERDUE SCHEDULED GRANT EVENTS"});
            ff.Add(new SelectListItem() { Value = "2", Text = "OVERDUE SCHEDULED NON-GRANT EVENTS" });
            ViewData["FilterItems"] = ff.AsEnumerable();

            List<SelectListItem> ev = new List<SelectListItem>();
            ev.Add(new SelectListItem() { Value = "0", Text = "My Tasks" });
            ev.Add(new SelectListItem() { Value = "1", Text = "All Tasks" });
       
            ViewData["iftasks"] = ev.AsEnumerable();

            if (alltasks.HasValue)
            {
                ViewData["task"] = alltasks.Value;
            }
            else
            {
                ViewData["task"] = 0;
            }

            if (filter.HasValue)
            {
                ViewData["filter"] = filter.Value;
            }
            else
            {
                ViewData["filter"] = -1;
            }
            return View();
        }

    }
}
