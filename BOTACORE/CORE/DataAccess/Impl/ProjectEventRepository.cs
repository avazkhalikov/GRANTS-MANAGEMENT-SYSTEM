using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;
namespace BOTACORE.CORE.DataAccess.Impl
{
    public class ProjectEventRepository:IProjectEventRepository
    {
       private string connectString;
       private BOTADataContext db;
       public ProjectEventRepository()
       {
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();
           db = new BOTADataContext(connectString);
       }
       public IEnumerable<ProjectEvent> GetProjectEventList(int ProjectID)
       {
           var result = from pe in db.ProjectEvents
                        //join et in db.EventTypes on pe.EventTypeID equals et.EventTypeID
                        //join peds in db.ProjectEventDocs on pe.EventID equals peds.EventID
                        //join ped in db.ProjectEventDocuments on peds.ProjectEventDocumentID equals ped.ProjectEventDocumentID
                        where pe.ProjectID==ProjectID && pe.EventStatus >= 0 && pe.EventStatus <= 1
                        orderby pe.EventID
                        select pe;

           
           return result;
       }

       public List<VsContainer> DoAreaVsCompetitionCode(IQueryable<Project> result, int Amounts)
       {

               var query = result
               .GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
               {
                   g.CompetitionCode.CompetCodeID,
                   g.ProgramArea.ProgramAreaCodeID
               })
               .Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
               {
                   ProjId = group.Select(i => i.ProjectID).ToList(),
                   Field1 = group.Key.CompetCodeID.Value,
                   Field2 = group.Key.ProgramAreaCodeID.Value,
                   iAmount = group.Sum(c => c.ProjectEvents.Count(projevnt => projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2)),
                   dAmount = group.Select(i => i.ProjectID).Count()
               });

               List<VsContainer> vsc = query.ToList();   //test.

               return query.ToList();
          
       }

       public IEnumerable<ProjectEvent> GetProjectEventU(int ProjectID)
       {
           var result = from pe in db.ProjectEvents
                        where (pe.EventType.EventTypeID == 1 || pe.EventType.EventTypeID ==2) && pe.EventStatus >= 0 && pe.EventStatus <= 1
                        select pe;
             
              //result.GroupBy(g => new            //GROUP BY GrantType, ProgramArea Field1, Field2 and SUM AmtRequested.
              // {
              //     g.ProjectID
              // }).Select(group => new VsContainer()     //Select all Grouped into VersusContainer.
              // {
              //     ProjId = group., 
              //     Field1 = group.Key.GrantTypeCodeID.Value,
              //     Field2 = group.Key.ProgramAreaCodeID.Value,
              // });
          // p.ProjectEvents.Count(projevnt => projevnt.EventType.EventTypeID == 1 || projevnt.EventType.EventTypeID == 2)
           return result;
       }

     
       public ProjectEvent GetProjectEvent(int EventID)
       {
           ProjectEvent result = (from pe in db.ProjectEvents
                        where pe.EventID == EventID
                        select pe).First();
           return result;
       }

       public bool Insert(ProjectEvent item)
       {
           bool result = true;
           try
           {
               db.ProjectEvents.InsertOnSubmit(item);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectEventRepository), "----------------------------------------------", ex); 
               result = false;
           }
           return result;
       }

       public bool Update(ProjectEvent item)
       {
           bool result = true;
           try
           {
               /*var oldItem  = (from p in db.ProjectEvents
                              where p.EventID == item.EventID
                              select p).First();*/
               db.ProjectEvents.Attach(item);
               db.Refresh(RefreshMode.KeepCurrentValues,item);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectEventRepository), "----------------------------------------------", ex); 
               result = false;   
           }
           return result;
       }

       public bool Delete(int EventID)
       {
           bool result = true;
           try
           {
               var toDelete = (from p in db.ProjectEvents
                               where p.EventID == EventID
                               select p).First();
               db.ProjectEvents.DeleteOnSubmit(toDelete);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectEventRepository), "----------------------------------------------", ex); 
               result = false;   
           }
           
           return result;
       }

       public int InsertDocument(ProjectEventDocument item, int EventID)
       {
           db.ProjectEventDocuments.InsertOnSubmit(item);
           db.SubmitChanges();
           ProjectEventDoc refitem = new ProjectEventDoc();
           refitem.EventID = EventID;
           refitem.ProjectEventDocumentID = item.ProjectEventDocumentID;
           db.ProjectEventDocs.InsertOnSubmit(refitem);
           db.SubmitChanges();
           return item.ProjectEventDocumentID;
       }

       public bool DeleteDocument(int DocumentID, int EventID)
       {
           bool result = true;
           try
           {
               var pdocitem = (from d in db.ProjectEventDocs
                               where d.EventID == EventID && d.ProjectEventDocumentID == DocumentID
                               select d).FirstOrDefault();
               db.ProjectEventDocs.DeleteOnSubmit(pdocitem);
               var docitem = (from d in db.ProjectEventDocuments
                              where d.ProjectEventDocumentID == DocumentID
                              select d).FirstOrDefault();
               db.ProjectEventDocuments.DeleteOnSubmit(docitem);
               db.SubmitChanges();
           }
           catch (Exception ex) {
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectEventRepository), "----------------------------------------------", ex); 
               result = false; }
           return result;
       }

       public ProjectEventDocument GetProjectEventDocument(int DocumentID)
       {
           ProjectEventDocument result;
           
               var item = (from d in db.ProjectEventDocuments
                           where d.ProjectEventDocumentID == DocumentID
                           select d).FirstOrDefault();
               result = item;
           
           
           return result;
       }

       public bool UpdateProjectEventDocument(ProjectEventDocument item)
       {
           bool result = true;
           try
           {
               ProjectEventDocument p = new ProjectEventDocument();
               db.ProjectEventDocuments.Attach(p);
               p = item;
               db.Refresh(RefreshMode.KeepCurrentValues, p);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectEventRepository), "----------------------------------------------", ex); 
               result = false;
           }
           return result;
       }

       public IEnumerable<ProjectEvent> GetProjectEventListScheduled(int ProjectID)
       {
           IEnumerable<ProjectEvent> result;
           var items=(from p in db.ProjectEvents
                       where p.EventStatus==0
                       && p.ProjectID==ProjectID
                       select p);
           result = items;
           return result;
       }

       public IEnumerable<ProjectEvent> GetProjectEventListScheduled()
       {
           IEnumerable<ProjectEvent> result;
           var items = (from p in db.ProjectEvents
                        where p.EventStatus == 0
                        select p);
           result = items;
           return result;
       }

       public IEnumerable<ProjectEvent> GetProjectEventListScheduledMy(int UserID)
       {
           IEnumerable<ProjectEvent> result;
           var items = (from p in db.ProjectEvents
                        join s in db.SSPStaffProjects 
                        on p.ProjectID equals s.ProjectID
                        where s.SSPStaffID==UserID
                        && p.EventStatus == 0
                        select p);
           result = items;
           return result;
       }

    }
}
