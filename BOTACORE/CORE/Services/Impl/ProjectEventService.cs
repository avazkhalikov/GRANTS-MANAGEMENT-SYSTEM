using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Impl;
using BOTACORE.CORE.DataAccess;

namespace BOTACORE.CORE.Services.Impl
{
    public class ProjectEventService:IProjectEventService
    {
        private IProjectEventRepository _rep;
        public ProjectEventService()
        {
            _rep = RepositoryFactory.ProjectEventRepository();
        }
        public IEnumerable<ProjectEvent> GetProjectEventList(int ProjectID)
        {
            return _rep.GetProjectEventList(ProjectID);
        }
        public ProjectEvent GetProjectEvent(int EventID)
        {
            return _rep.GetProjectEvent(EventID);
        }


        public int GetNumberOfSiteVisits(int? ProjectID)
        {
            IEnumerable<ProjectEvent> projevnts = GetProjectEventList(ProjectID.Value);
            int sitevisits = 0;

          //  List<int> SiteVisit = new List<int>();           

            foreach (ProjectEvent projevnt in projevnts)
            {
                if (projevnt.EventType.EventTypeName == "Site Visit" || projevnt.EventType.EventTypeName == "Site visit")              
                {
                    sitevisits++;
                }
                else if (projevnt.EventDescription != null)
                { 
                    if(projevnt.EventDescription.Contains("Site Visit") || projevnt.EventDescription.Contains("Site visit"))
                    {
                      sitevisits++;
                    }
                }
            }

            return sitevisits; 
        }

        public int GetNumberOfAcceptedNarrative(int? ProjectID)
        {
            IEnumerable<ProjectEvent> projevnts = GetProjectEventList(ProjectID.Value);
            int narrativeaccepted = 0;
           

            foreach (ProjectEvent projevnt in projevnts)
            {
                if (projevnt.EventType.EventTypeName == "Narrative Report" || projevnt.EventType.EventTypeName == "Narrative report")
                {                    
                    if (projevnt.ReportStatus.HasValue)
                    {
                        if (projevnt.ReportStatus.Value == 1)
                            narrativeaccepted++;                        
                    }
                }
                else if (projevnt.EventDescription != null)
                {
                    if(projevnt.EventDescription.Contains("Narrative Report") || projevnt.EventDescription.Contains("Narrative report"))
                    {
                       if (projevnt.ReportStatus.Value == 1)
                            narrativeaccepted++; 
                    }
                }
            }

            return narrativeaccepted; 
        }

        
        public int GetNumberOfRejectedNarrative(int? ProjectID)
        {
            IEnumerable<ProjectEvent> projevnts = GetProjectEventList(ProjectID.Value);
            int narrativerejected = 0;


            foreach (ProjectEvent projevnt in projevnts)
            {
                if (projevnt.EventType.EventTypeName == "Narrative Report" || projevnt.EventType.EventTypeName == "Narrative report")
                {

                    if (projevnt.ReportStatus.HasValue)
                    {
                        if (projevnt.ReportStatus.Value == 0)
                            narrativerejected++;
                    }
                }
                else if (projevnt.EventDescription != null)
                {
                    if (projevnt.EventDescription.Contains("Narrative Report") || projevnt.EventDescription.Contains("Narrative report"))
                    {
                        if (projevnt.ReportStatus.Value == 0)
                            narrativerejected++;
                    }
                }
            }

            return narrativerejected; 
        }
        

        public bool Insert(ProjectEvent item)
        {
            return _rep.Insert(item);
        }

        public bool Update(ProjectEvent item)
        {
            return _rep.Update(item);
        }

        public bool Delete(int EventID)
        {
            return _rep.Delete(EventID);
        }

        public int InsertDocument(ProjectEventDocument item, int EventID)
        {
            return _rep.InsertDocument(item, EventID);
        }
        public bool DeleteDocument(int DocumentID, int EventID)
        {
            return _rep.DeleteDocument(DocumentID, EventID);
        }
        public ProjectEventDocument GetProjectEventDocument(int DocumentID)
        {
            return _rep.GetProjectEventDocument(DocumentID);
        }
        public bool UpdateProjectEventDocument(ProjectEventDocument item)
        {
            return _rep.UpdateProjectEventDocument(item);
        }
        public IEnumerable<ProjectEvent> GetProjectEventListScheduled(int ProjectID)
        {
            return _rep.GetProjectEventListScheduled(ProjectID);
        }
        public IEnumerable<ProjectEvent> GetProjectEventListScheduled()
        {
            return _rep.GetProjectEventListScheduled();
        }
        public IEnumerable<ProjectEvent> GetProjectEventListScheduledMy(int UserID)
        {
            return _rep.GetProjectEventListScheduledMy(UserID);
        }
    }
}
