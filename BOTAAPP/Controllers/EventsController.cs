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
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace BOTAMVC3.Controllers
{
    public class EventsController : BOTAController
    {

        [AcceptVerbs(HttpVerbs.Post)]
        public string WordAutoSave(int FileID, int EventID)
        {
            //depending on .ext it is either doc or excel.
            bool result = true;
            ProjectEventService pes = new ProjectEventService();
            string dir = ConfigurationManager.AppSettings["PhysicalPath"];
            string virpath = dir + pes.GetProjectEventDocument(FileID).PhysicalVirtualPath;

            try
            {
                object filename = virpath; //@"\\192.168.33.17\Download\IT\TEST.docx";
                Word.Application application = new Word.Application();
                application.Visible = true;
              //  int hello = application.Documents.Count(); 
                Word.Document document = application.Documents.Open(filename);
            }
            catch (Exception e)
            {
                return "Exception caught. " + e;
            }


            return virpath;
        }

        
      
/*
        private bool DeleteMyFile(int FileID, int EventID)
        {
            bool result = true;
            ProjectEventService pes = new ProjectEventService();
            string dir = ConfigurationManager.AppSettings["PhysicalPath"];
            string virpath = pes.GetProjectEventDocument(FileID).PhysicalVirtualPath;
            System.IO.File.Delete(dir + virpath);
            pes.DeleteDocument(FileID, EventID);
            return result;
        }

*/
        //
        // GET: /Event/

        public ActionResult Index(int? id)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            EventTypeService et=new EventTypeService();
            ProjectEventService pes = new ProjectEventService();
            SSPStaffService sss = new SSPStaffService();
            ViewData["ddlEventType"]=new SelectList(et.GetEventTypeList(),"EventTypeID","EventTypeName",31);
            //Session["ProposalID"] = "1";
            int ProposalID = Session["ProposalID"] != null ? int.Parse(Session["ProposalID"].ToString()) : 0;
            ViewData["ProjectID"] = ProposalID;
            IEnumerable<ProjectEvent> pe = pes.GetProjectEventList(ProposalID);
            
            IEnumerable<SSPStaff> slist=sss.GetSSPStaffListForProject(ProposalID);

            

            List<SelectListItem> selects=new List<SelectListItem>();
            if (slist!=null)
            {

            //this gets users who is in keyassosiactions.
            foreach(var item in slist)
            {
                selects.Add(new SelectListItem(){Value=item.SSPStaffID.ToString(), Text=item.FirstName+" "+item.LastName+" "+item.MiddleName});
            }
            
            //-----------------This chunk added in order not 
            // =========to lose previous eventHolder.
            //next step we must add users who is participating in this project if they don't exist, 
            //this will cover users who was deleted from keyassociations, but did particular event.          
            //How to do that? we go through all Events, get event Holders and compare HolderID agains slist. if does not exist we add. 
            foreach (ProjectEvent e in pe)
            {   //check if staff in event inside DropDown.
                SelectListItem sitem = (from x in selects 
                               where x.Value.ToString() == e.EventHolderID.ToString()
                               select x).FirstOrDefault();
                
                if (sitem == null) //no staff.
                { 
                    SSPStaff staff = null; 
                
                    //get that user from repository
                   if(e.EventHolderID.HasValue)
                   staff = sss.GetAccountByID(e.EventHolderID.Value); 

                   //insert.
                    if(staff != null)
                    selects.Add(new SelectListItem() { Value = staff.SSPStaffID.ToString(), Text = staff.FirstName + " " + staff.LastName + " " + staff.MiddleName });
                }

            }
                //----------------------

            }
            ViewData["SSPStaffList"] = selects;
            ProjectService projservice = new ProjectService(); ;
            Project _project;
            _project = projservice.GetProposalInfo(ProposalID);
            List<SelectListItem> reportperiods=new List<SelectListItem>();
            try
            {
                foreach (var item in _project.Budget.ReportPeriodListRs)
                {
                    reportperiods.Add(new SelectListItem()
                    {
                        Value = item.ReportPeriodID.ToString(),
                        Text = (item.PeriodStart.HasValue ? item.PeriodStart.Value.ToShortDateString() : "") + " - " +
                    (item.PeriodEnd.HasValue ? item.PeriodEnd.Value.ToShortDateString() : "")
                    });
                }
            } catch (Exception ex){}
            ViewData["ReportPeriodList"] = reportperiods;
            if (id.HasValue==true)
            {
                ViewData["selectedID"] = id;
            }
            return View(pe);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAction(string useraction, ProjectEvent o, string SSPOrGrantee)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }
            
            ProjectEventService pes = new ProjectEventService();
            switch (useraction.ToLower())
            {
                case "generate file from template":
                    if (o.EventID > 0) 
                    pes.Update(o);
                    return RedirectToAction("SelectTemplate", new { ProjectID = o.ProjectID, EventID = o.EventID, EventTypeID = o.EventTypeID, ReportPeriodID=o.ReportPeriodID });
                case "insert":
                   // if (o.EventID > 0) 
                    pes.Insert(o);
                    break;
                    return RedirectToAction("SelectTemplate", new { ProjectID = o.ProjectID, EventID = o.EventID, o.ReportPeriodID });
                case "update":
                    if (o.EventID > 0)   //there must be eventID and user must be logged in.
                    {
                        pes.Update(o);
                        if (Request.Files.Count != 0)
                        {
                            for (int i = 0; i < Request.Files.Count; i++)
                            {
                                if (!String.IsNullOrEmpty(Request.Files[i].FileName))
                                {
                                    string FileKey;
                                    FileKey = Request.Files.GetKey(i);
                                    string FileID;
                                    FileID = FileKey.Replace("files_", "");

                                    string dir = ConfigurationManager.AppSettings["PhysicalPath"];

                                    //string MyString="";
                                    //int FileLen;
                                    //System.IO.Stream MyStream;

                                    //FileLen = Request.Files[i].ContentLength;
                                    //byte[] input = new byte[FileLen];

                                    //// Initialize the stream.
                                    //MyStream = Request.Files[i].InputStream;

                                    //// Read the file into the byte array.
                                    //MyStream.Read(input, 0, FileLen);

                                    //// Copy the byte array into a string.
                                    //for (int Loop1 = 0; Loop1 < FileLen; Loop1++)
                                    //    MyString = MyString + input[Loop1].ToString();
                                     string fileOriginalName = Request.Files[i].FileName;
                                     string ExtenSion = Path.GetExtension(fileOriginalName);

                                    if (FileID == "0")
                                    {
                                        #region InsertNewDocument
                                       

                                        ProjectEventDocument myDoc = new ProjectEventDocument();
                                        string virtdir = "files/" + o.ProjectID.ToString() + "/";//"files/A" + DateTime.Now.Year.ToString().Substring(2) + o.ProjectID.ToString() + "/";
                                        dir += virtdir.Replace("/", "\\");
                                        if (!(Directory.Exists(dir)))
                                        {
                                            Directory.CreateDirectory(dir);
                                        }
                                        string fileformat = o.EventID.ToString() + "_" + o.ProjectID.ToString() + "_";
                                        int fi;
                                        //checking other file names to come up with new file number.
                                        for (fi = 1; System.IO.File.Exists(dir + fileformat + fi.ToString() + ExtenSion); fi++)
                                        {
                                        }



                                        Request.Files[i].SaveAs(dir + fileformat + fi.ToString() + ExtenSion);

                                        myDoc.Author = session.CurrentUser.FirstName + " " + session.CurrentUser.LastName + " " + session.CurrentUser.MiddleName;
                                        myDoc.CreatedDate = DateTime.Now;
                                        myDoc.fileextension = ExtenSion.Replace(".", ""); //needs "doc"
                                        myDoc.FileName = fileformat + fi.ToString() + ExtenSion;
                                        myDoc.PhysicalAbsolutePath = dir + fileformat + fi.ToString() + ExtenSion;
                                        myDoc.PhysicalVirtualPath = "\\" + virtdir.Replace("/", "\\") + fileformat + fi.ToString() + ExtenSion;
                                        myDoc.UpdatedDate = myDoc.CreatedDate;
                                        myDoc.WebVirtualPath = "/" + virtdir + fileformat + fi.ToString() + ExtenSion;
                                        pes.InsertDocument(myDoc, o.EventID);
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Update Document  //with different file extension it won't work.
                                        //get object by fileid
                                        //update object

                                        ProjectEventDocument myDoc = pes.GetProjectEventDocument(int.Parse(FileID));
                                        //System.IO.File.WriteAllText(dir + myDoc.PhysicalVirtualPath, MyString);

                                        if (myDoc.fileextension == ExtenSion.Replace(".", ""))  //if extensions are the same then Update allowed.
                                        {
                                            Request.Files[i].SaveAs(dir + myDoc.PhysicalVirtualPath);
                                            myDoc.Author = session.CurrentUser.FirstName + " " + session.CurrentUser.LastName + " " + session.CurrentUser.MiddleName; ;
                                            myDoc.UpdatedDate = DateTime.Now;
                                            pes.UpdateProjectEventDocument(myDoc);
                                        }
                                        //get object by fileid
                                        //update object
                                        #endregion
                                    }

                                }
                            }
                        }
                    }
                    break;
                case "delete":
                    string mydir = Request.MapPath("~");
                    foreach (var item in o.ProjectEventDocs)
                    {
                        try
                        {
                            System.IO.File.Delete(mydir + item.ProjectEventDocument.PhysicalVirtualPath);
                        }
                        catch (Exception ex) { }
                        DeleteMyFile(item.ProjectEventDocumentID, o.EventID);
                    }
                    pes.Delete(o.EventID);
                    break;
            }

            return RedirectToAction("Index", new { id=o.EventID});
        }

        public ActionResult SelectTemplate(int ProjectID, int EventID, int? EventTypeID, int? ReportPeriodID)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            TemplateService ts = new TemplateService();
            ViewData["ProjectID"] = ProjectID;
            ViewData["EventID"] = EventID;
            IEnumerable<TemplateDocument> templates;
            if (ReportPeriodID.HasValue) ViewData["ReportPeriodID"] = ReportPeriodID.Value;
            if (EventTypeID.HasValue)
            {
                ViewData["EventTypeID"] = EventTypeID.Value;
                templates = ts.GetTemplates(EventTypeID.Value);
            }
            else
            {
                templates = ts.GetTemplates();
            }
            if (templates == null)
            {
                return RedirectToAction("Index", new { id=EventID});
            }
            return View(templates);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SelectTemplate(int TemplateDocsID, int ProjectID, int EventID, int? ReportPeriodID)
        {
            if (Session["ProposalID"] == null)
            {
                return RedirectToAction("Search", "ProposalInfo");
            }

            /*
            TemplateService ts = new TemplateService();
            string dir = ConfigurationManager.AppSettings["PhysicalPath"];
            string WordDoc = ts.GenerateDocumentFromTemplateName(TemplateDocsID, EventID, ProjectID, dir, ReportPeriodID);
            
            TemplateFile tfile = ts.GetTemplateDocument(TemplateDocsID).TemplateFile;
            
            string virtdir = "files/A" + DateTime.Now.Year.ToString().Substring(2) + ProjectID.ToString()+"/";
            dir += virtdir.Replace("/", "\\");
            
            if (!(Directory.Exists(dir)))
            {
                Directory.CreateDirectory(dir);
            }
            string fileformat=EventID.ToString()+"_"+ProjectID.ToString()+"_";
            int i;
            for (i = 1; System.IO.File.Exists(dir+fileformat+i.ToString()+"."+tfile.fileextension); i++)
            {
            }
            System.IO.File.WriteAllText(dir + fileformat + i.ToString() + "." + tfile.fileextension, WordDoc);
            ProjectEventDocument docItem = new ProjectEventDocument();
            docItem.Author = session.CurrentUser.FirstName + " " + session.CurrentUser.LastName + " " + session.CurrentUser.MiddleName;;
            docItem.CreatedDate = DateTime.Now;
            docItem.fileextension = tfile.fileextension;
            docItem.FileName = fileformat + i.ToString() + "." + tfile.fileextension;
            docItem.PhysicalAbsolutePath = dir + fileformat + i.ToString() + "." + tfile.fileextension;
            docItem.PhysicalVirtualPath = "\\" + virtdir.Replace("/", "\\") + fileformat + i.ToString() + "." + tfile.fileextension;
            docItem.UpdatedDate = docItem.CreatedDate;
            docItem.WebVirtualPath = "/" + virtdir + fileformat + i.ToString() + "." + tfile.fileextension;
            //docItem.
            ProjectEventService pes = new ProjectEventService();
            pes.InsertDocument(docItem, EventID);
            

            TemplateDocument tdoc=ts.GetTemplateDocument(TemplateDocsID);
            if (tdoc!=null)
            {
                ProjectService projservice = new ProjectService();
                
                //acknowledgement letter - completed project
                if (tdoc.TemplateDocName.ToLower().Contains("letter") && tdoc.TemplateDocName.ToLower().Contains("acknowledgement"))
                {
                    AppDropDownsService apservice=new AppDropDownsService();
                    ProposalStatusList psitem=apservice.ProposalStatusListGetItem("Completed");
                    
                    //if we have needed status in DB then
                    if (psitem != null)
                    {
                        Project _project = projservice.GetProposalInfo(ProjectID);
                        ProposalStatus _prop = _project.ProposalStatus;
                        _prop.PropStatusID = psitem.ProposalStatusID;
                        projservice.ProposalStatusUpdate(_prop);  
                    }
                    
                }

                //award letter - Active project
                if (tdoc.TemplateDocName.ToLower().Contains("letter") && tdoc.TemplateDocName.ToLower().Contains("award"))
                {
                    AppDropDownsService apservice = new AppDropDownsService();
                    ProposalStatusList psitem = apservice.ProposalStatusListGetItem("Active");

                    //if we have needed status in DB then
                    if (psitem != null)
                    {
                        Project _project = projservice.GetProposalInfo(ProjectID);
                        ProposalStatus _prop = _project.ProposalStatus;
                        _prop.PropStatusID = psitem.ProposalStatusID;
                        projservice.ProposalStatusUpdate(_prop);
                    }

                }

                //close out letter - Closed project
                if (tdoc.TemplateDocName.ToLower().Contains("letter") && tdoc.TemplateDocName.ToLower().Contains("close") && tdoc.TemplateDocName.ToLower().Contains("out"))
                {
                    AppDropDownsService apservice = new AppDropDownsService();
                    ProposalStatusList psitem = apservice.ProposalStatusListGetItem("Closed");

                    //if we have needed status in DB then
                    if (psitem != null)
                    {
                        Project _project = projservice.GetProposalInfo(ProjectID);
                        ProposalStatus _prop = _project.ProposalStatus;
                        _prop.PropStatusID = psitem.ProposalStatusID;
                        projservice.ProposalStatusUpdate(_prop);
                    }

                }

                //suspended letter - Active project
                if (tdoc.TemplateDocName.ToLower().Contains("letter") && tdoc.TemplateDocName.ToLower().Contains("suspended"))
                {
                    AppDropDownsService apservice = new AppDropDownsService();
                    ProposalStatusList psitem = apservice.ProposalStatusListGetItem("Suspended");

                    //if we have needed status in DB then
                    if (psitem != null)
                    {
                        Project _project = projservice.GetProposalInfo(ProjectID);
                        ProposalStatus _prop = _project.ProposalStatus;
                        _prop.PropStatusID = psitem.ProposalStatusID;
                        projservice.ProposalStatusUpdate(_prop);
                    }

                }

                //reject letter - Rejected project
                if (tdoc.TemplateDocName.ToLower().Contains("letter") && tdoc.TemplateDocName.ToLower().Contains("reject"))
                {
                    AppDropDownsService apservice = new AppDropDownsService();
                    ProposalStatusList psitem = apservice.ProposalStatusListGetItem("Rejected");

                    //if we have needed status in DB then
                    if (psitem != null)
                    {
                        Project _project = projservice.GetProposalInfo(ProjectID);
                        ProposalStatus _prop = _project.ProposalStatus;
                        _prop.PropStatusID = psitem.ProposalStatusID;
                        projservice.ProposalStatusUpdate(_prop);
                    }

                }


            }*/
            EventTemplate ehelper = new EventTemplate();
            ehelper.CreateFromTemplate(TemplateDocsID, ProjectID, EventID, ReportPeriodID);

            return RedirectToAction("Index", new { id=EventID});
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public bool DeleteFile(int FileID, int EventID)
        {
            return DeleteMyFile(FileID, EventID);
        }

        private bool DeleteMyFile(int FileID, int EventID)
        {
            bool result=true;
            ProjectEventService pes = new ProjectEventService();
            string dir = ConfigurationManager.AppSettings["PhysicalPath"];
            string virpath=pes.GetProjectEventDocument(FileID).PhysicalVirtualPath;
            System.IO.File.Delete(dir + virpath);
            pes.DeleteDocument(FileID, EventID);
            return result;
        }
    }
}
