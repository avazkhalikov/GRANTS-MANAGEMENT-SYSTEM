using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;
using System.Data;
using StructureMap;
using BOTACORE.CORE;
using BOTACORE.CORE.DataAccess.DAL;
using log4net;
using log4net.Config;

namespace BOTACORE.CORE.DataAccess.Impl
{
    public class UserActionLogRep
    {
       private string connectString;
       private ISqlCommands sqlcmd;
       protected static readonly ILog log = LogManager.GetLogger(typeof(ProjectRepository));
       private BOTADataContext db;

       public UserActionLogRep()
       {
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();
           db = new BOTADataContext(connectString);
       }


       public IEnumerable<UserActionLog> GetUserAction()
       {
           IEnumerable<UserActionLog> result = (from ul in db.UserActionLogs
                                                orderby ul.Date descending 
                                                select ul).Take(500);
           return result;
       }


       public bool Insert(UserActionLog item)
       {
           bool result = true;
           try
           {
               db.UserActionLogs.InsertOnSubmit(item);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectEventRepository), "-------------User Action Logs- Insert-------------------------", ex);
               result = false;
           }
           return result;
       }



       public UserActionLog getUlogByID(int LogID)
       {
          
           try
           {
               var toReturn = (from p in db.UserActionLogs
                               where p.LogID == LogID
                               select p).First();
               return toReturn;
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();               
               Log.Error(typeof(ProjectEventRepository), "-------------User Action Logs- Select---------" + LogID + "--------------", ex);
               return null;
           }

          
       }


       public bool Delete(int LogID)
       {
           bool result = true;
           try
           {
               var toDelete = (from p in db.UserActionLogs
                               where p.LogID == LogID
                               select p).First();
               db.UserActionLogs.DeleteOnSubmit(toDelete);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(ProjectEventRepository), "-------------User Action Logs Delete---------------------------------", ex);
               result = false;
           }

           return result;
       }




    }
}
