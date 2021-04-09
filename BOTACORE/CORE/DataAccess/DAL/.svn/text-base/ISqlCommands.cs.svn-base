using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data; 

namespace BOTACORE.CORE.DataAccess.DAL
{
   public interface ISqlCommands
    {
       IDataReader GetUserByName(string username);
       bool Update(string UpdateString);
       IDataReader GetBudgetTransactionByID(int bid);
       IDataReader GetProjectsForFilter(string query);
    }
}
