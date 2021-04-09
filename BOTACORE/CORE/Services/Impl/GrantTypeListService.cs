using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Impl;
using BOTACORE.CORE.DataAccess;

namespace BOTACORE.CORE.Services.Impl
{
    public class GrantTypeListService:IGrantTypeListService
    {
        private IGrantTypeListRepository _rep;
        public GrantTypeListService()
        {
            _rep = RepositoryFactory.GrantTypeListRepository();
        }

        public IEnumerable<GrantTypeList> GetAll()
        {
            return _rep.GetAll();
        }

    }
}
