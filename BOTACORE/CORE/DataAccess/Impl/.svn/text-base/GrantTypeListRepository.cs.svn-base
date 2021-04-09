using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.DataAccess.Impl
{
    public class GrantTypeListRepository:IGrantTypeListRepository
    {
        private string connectString;
        private BOTADataContext db;

        public GrantTypeListRepository()
        {
            Connection conn = new Connection();
            connectString = conn.GetDirectConnString();
            db = new BOTADataContext(connectString);
        }

       

        public IEnumerable<GrantTypeList> GetAll()
        {
            IEnumerable<GrantTypeList> result;
            var query = (from gt in db.GrantTypeLists
                        select gt);
            result = query;
            return result;
        }
        
    }
}
