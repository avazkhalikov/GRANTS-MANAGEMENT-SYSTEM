using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
namespace BOTACORE.CORE.Services
{
    public interface IProjectEventService
    {
        IEnumerable<ProjectEvent> GetProjectEventList(int ProjectID);
        ProjectEvent GetProjectEvent(int EventID);
        bool Insert(ProjectEvent item);
        bool Update(ProjectEvent item);
        bool Delete(int EventID);
        int InsertDocument(ProjectEventDocument item, int EventID);
        bool DeleteDocument(int DocumentID, int EventID);
        ProjectEventDocument GetProjectEventDocument(int DocumentID);
        bool UpdateProjectEventDocument(ProjectEventDocument item);
        IEnumerable<ProjectEvent> GetProjectEventListScheduled(int ProjectID);
        IEnumerable<ProjectEvent> GetProjectEventListScheduled();
        IEnumerable<ProjectEvent> GetProjectEventListScheduledMy(int UserID);
    }
}
