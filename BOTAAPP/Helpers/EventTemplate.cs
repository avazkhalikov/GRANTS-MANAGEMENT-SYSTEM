using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOTACORE.CORE.Services.Impl;
using System.Configuration;
using BOTACORE.CORE.Domain;
using System.IO;
using BOTACORE.CORE.Services;

namespace BOTAMVC3.Helpers
{
    public class EventTemplate
    {
        public IUserSession session;
        public EventTemplate()
        {
            session = new UserSession();
        }

        public bool CreateFromTemplate(int TemplateDocsID, int ProjectID, int EventID, int? ReportPeriodID)
        {
            bool result = true;
            try
            {
                TemplateService ts = new TemplateService();
                string dir = ConfigurationManager.AppSettings["PhysicalPath"];
                string WordDoc = ts.GenerateDocumentFromTemplateName(TemplateDocsID, EventID, ProjectID, dir, ReportPeriodID);

                TemplateFile tfile = ts.GetTemplateDocument(TemplateDocsID).TemplateFile;

                string virtdir = "files/" + ProjectID.ToString() + "/"; // "files/A" + DateTime.Now.Year.ToString().Substring(2) + ProjectID.ToString() + "/";
                dir += virtdir.Replace("/", "\\");

                if (!(Directory.Exists(dir)))
                {
                    Directory.CreateDirectory(dir);
                }
                string fileformat = EventID.ToString() + "_" + ProjectID.ToString() + "_";
                int i;
                for (i = 1; System.IO.File.Exists(dir + fileformat + i.ToString() + "." + tfile.fileextension); i++)
                {
                }
                System.IO.File.WriteAllText(dir + fileformat + i.ToString() + "." + tfile.fileextension, WordDoc);
                ProjectEventDocument docItem = new ProjectEventDocument();
                docItem.Author = session.CurrentUser.FirstName + " " + session.CurrentUser.LastName + " " + session.CurrentUser.MiddleName; ;
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

                TemplateDocument tdoc = ts.GetTemplateDocument(TemplateDocsID);
                if (tdoc != null)
                {
                    ProjectService projservice = new ProjectService();

                    //acknowledgement letter - completed project
                    if (tdoc.TemplateDocName.ToLower().Contains("letter") && tdoc.TemplateDocName.ToLower().Contains("acknowledgement"))
                    {
                        AppDropDownsService apservice = new AppDropDownsService();
                        ProposalStatusList psitem = apservice.ProposalStatusListGetItem("Completed");

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
                          //  projservice.ProposalStatusUpdate(_prop);
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


                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
