using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
namespace BOTACORE.CORE.DataAccess.Impl
{
    public class EventTypeRepository:IEventTypeRepository
    {
       private string connectString;
       public EventTypeRepository()
       {
            //string connString = Settings.Default.EntityConnection; 
          //  Context = new BOTADBEntities1();
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();
       
       }
       public IEnumerable<EventType> GetEventTypeList()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from et in db.EventTypes
                        select et;
           return result;
       }

       /*public List<string> GetEventStatusesAll()
       {
           List<string
       }*/

    }
}
