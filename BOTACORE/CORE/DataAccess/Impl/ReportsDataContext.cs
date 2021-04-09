using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;
using System.Data;
using BOTACORE.CORE.Services.Impl;
using StructureMap;
using BOTACORE.CORE;
using BOTACORE.CORE.DataAccess.DAL;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace BOTACORE.CORE.DataAccess.Impl
{
   public static class ReportsDataContext
    {
        private static string connectString;
        static BOTADataContext db;

       static ReportsDataContext()
       {
           //string connString = Settings.Default.EntityConnection; 
          //  Context = new BOTADBEntities1();
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();
           db = new BOTADataContext(connectString);
          
       }

      public static BOTADataContext GetDataContext()
       {
           return db; 
       }
    }
}
