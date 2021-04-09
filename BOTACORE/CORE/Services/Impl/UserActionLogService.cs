using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.Impl;

namespace BOTACORE.CORE.Services.Impl
{
    public class UserActionLogService
    {
        ProjectRepository projrepository;
        OrganizationRepository orgrep;
        UserSession session;
        UserActionLogRep LogRep;

        public UserActionLogService()
        {
            session = new UserSession();
            projrepository = new ProjectRepository();
            orgrep = new OrganizationRepository();
            LogRep = new UserActionLogRep();
        }
        
        public IEnumerable<UserActionLog> GetUserAction()
        {
            return LogRep.GetUserAction();
        }
        
        public bool Insert(UserActionLog item)
        {
            return LogRep.Insert(item);
        }
        
        public bool Delete(int LogID)
        {

            return LogRep.Delete(LogID);
        }

        public UserActionLog getUlogByID(int id)
        {

            return LogRep.getUlogByID(id);
        }

        
    }
}
