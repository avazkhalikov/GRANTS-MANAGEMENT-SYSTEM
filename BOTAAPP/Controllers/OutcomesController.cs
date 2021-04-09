using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services.Impl;
using System.IO;
using BOTAMVC3.Helpers;
using System.Configuration;
//using System.Text.RegularExpressions;

namespace BOTAMVC3.Controllers
{
    public class OutcomesController : BOTAController
    {
        //
        // GET: /Outcomes/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {


            if (session.ProjectID < 0)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            ProjectService projs = new ProjectService();
            OutComeStatement outcom = projs.GetOutComeStatements(session.ProjectID);

            if (outcom != null)
                return View(outcom); 
            else
                return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(OutComeStatement outcom, string sensitive)
        {
            //clean the outcome text. Leave only text, remove all word symbols and everything else.
            outcom.OutcomeR = CleanHtml(outcom.OutcomeR);  
            if(sensitive== "on")
            {
                outcom.sensitive = true; 
            }
            if (session.ProjectID < 0)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            ProjectService projs = new ProjectService();
            outcom.ProjectID = session.ProjectID;
            bool status = projs.UpdateOutComeStatements(outcom);
          
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Removes all FONT and SPAN tags, and all Class and Style attributes.
        /// Designed to get rid of non-standard Microsoft Word HTML tags.
        /// </summary>
        private string CleanHtml(string html)
        {
            // start by completely removing all unwanted tags 
          //  html = Regex.Replace(html, @"<[/]?(font|span|xml|del|ins|[ovwxp]:\w+)[^>]*?>", "", RegexOptions.IgnoreCase);
            // then run another pass over the html (twice), removing unwanted attributes 
        //    html = Regex.Replace(html, @"<([^>]*)(?:class|lang|style|size|face|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>", RegexOptions.IgnoreCase);
         //   html = Regex.Replace(html, @"<([^>]*)(?:class|lang|style|size|face|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>", RegexOptions.IgnoreCase);
            return html;
        }


    }
}
