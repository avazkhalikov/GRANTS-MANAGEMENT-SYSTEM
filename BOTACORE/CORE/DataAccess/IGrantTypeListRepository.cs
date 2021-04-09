using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.DataAccess
{
    public interface IGrantTypeListRepository
    {
        IEnumerable<GrantTypeList> GetAll();
    }
}
