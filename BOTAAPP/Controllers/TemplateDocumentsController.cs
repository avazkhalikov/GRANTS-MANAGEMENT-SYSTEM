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

namespace BOTAMVC3.Controllers
{
    public class TemplateDocumentsController : BOTAController
    {
        //
        // GET: /Templates/
        private TemplateService ts;
        public TemplateDocumentsController()
        {
             ts= new TemplateService();
        }

        public ActionResult Index()
        {
            if (!session.LoggedIn)
            {
                return RedirectToAction("Login", "SSPStaff");
            }
            EventTypeService ets = new EventTypeService();
            IEnumerable<EventType> m;
            m=ets.GetEventTypeList();
            return View(m);
        }
        
        
        public ActionResult Templates(int EventTypeID)
        {
            if (!session.LoggedIn)
            {
                return RedirectToAction("Login", "SSPStaff");
            }

            IEnumerable<TemplateDocument> Templates=ts.GetTemplates(EventTypeID);
            ViewData["EventTypeID"] = EventTypeID;
            return View(Templates);
        }

        public ActionResult Create(int EventTypeID)
        {
            if (!session.LoggedIn)
            {
                return RedirectToAction("Login", "SSPStaff");
            }

            TemplateDocument t = new TemplateDocument();
            t.EventTypeID = EventTypeID;
            return View(t);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(TemplateDocument item, string fileextension)
        {
            if (!session.LoggedIn)
            {
                return RedirectToAction("Login", "SSPStaff");
            }

            if (Request.Files.Count != 0)
            {
                if (!String.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    TemplateFile tf = new TemplateFile();
                    tf.Author = session.CurrentUser.FirstName + " " + session.CurrentUser.LastName + " " + session.CurrentUser.MiddleName;;
                    tf.CreatedDate = DateTime.Now;
                    /*string ext;
                    string[] a = Request.Files[0].FileName.ToLower().Split('.');
                    ext = a[a.Length - 1];*/
                    tf.fileextension = fileextension;
                    tf.FileName = Request.Files[0].FileName;
                    //string dir = Request.MapPath("~");
                    string dir = ConfigurationManager.AppSettings["PhysicalPath"];
                    string virtdir = "Templates/";
                    dir += virtdir.Replace("/", "\\");
                    if (!(Directory.Exists(dir)))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    Request.Files[0].SaveAs(dir+Request.Files[0].FileName);
                    tf.PhysicalVirtualPath = "\\" + virtdir.Replace("/", "\\") + Request.Files[0].FileName;
                    tf.UpdatedDate = DateTime.Now;
                    tf.WebVirtualPath = "/" + virtdir + Request.Files[0].FileName;
                    item.TemplateFile = tf;
                }
            }
            ts.InsertTemplateDocument(item);
            return RedirectToAction("Templates", new { EventTypeID=item.EventTypeID });
        }

        public ActionResult Edit(int id)
        {
            if (!session.LoggedIn)
            {
                return RedirectToAction("Login", "SSPStaff");
            }

            TemplateDocument t = ts.GetTemplateDocument(id);
            return View(t);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(TemplateDocument item, string fileextension)
        {
            if (!session.LoggedIn)
            {
                return RedirectToAction("Login", "SSPStaff");
            }

            TemplateDocument t = ts.GetTemplateDocument(item.TemplateDocsID);
            t.TemplateDocName = item.TemplateDocName;
            t.EventTypeID = item.EventTypeID;
            t.TemplateFile.fileextension = fileextension;
            if (Request.Files.Count != 0)
            {
                if (!String.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    //TemplateFile tf = new TemplateFile();
                    t.TemplateFile.Author = session.CurrentUser.FirstName + " " + session.CurrentUser.LastName + " " + session.CurrentUser.MiddleName;;
                    t.TemplateFile.CreatedDate = DateTime.Now;
                    /*string ext;
                    string[] a = Request.Files[0].FileName.ToLower().Split('.');
                    ext = a[a.Length - 1];*/
                    
                    t.TemplateFile.FileName = Request.Files[0].FileName;
                    string dir = ConfigurationManager.AppSettings["PhysicalPath"];
                    try
                    {
                        System.IO.File.Delete(dir + t.TemplateFile.PhysicalVirtualPath);
                    }
                    catch (Exception ex)
                    {
                    }
                    string virtdir = "Templates/";
                    dir += virtdir.Replace("/", "\\");
                    if (!(Directory.Exists(dir)))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    Request.Files[0].SaveAs(dir + Request.Files[0].FileName);
                    t.TemplateFile.PhysicalVirtualPath = "\\" + virtdir.Replace("/", "\\") + Request.Files[0].FileName;
                    t.TemplateFile.UpdatedDate = DateTime.Now;
                    t.TemplateFile.WebVirtualPath = "/" + virtdir + Request.Files[0].FileName;
                }
            }
            ts.UpdateTemplateDocument(t);
            return RedirectToAction("Templates", new { EventTypeID=item.EventTypeID });
        }

        public ActionResult Delete(int id, int EventTypeID)
        {
            if (!session.LoggedIn)
            {
                return RedirectToAction("Login", "SSPStaff");
            }

            ts.DeleteTemplateDocument(id);
            return RedirectToAction("Templates", new { EventTypeID = EventTypeID });
        }

    }
}
