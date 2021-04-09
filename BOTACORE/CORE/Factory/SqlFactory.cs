using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using BOTACORE.CORE.Services.Impl;
using BOTACORE.CORE.Services;
using BOTACORE.CORE.DataAccess.DAL;

namespace BOTACORE.CORE
{
    public class SqlFactory
    {
        public static ISqlCommands MSSQL()
        {
            Container container = new Container();
            return container.GetInstance<ISqlCommands>(); // ObjectFactory.GetInstance<IAccountRepository>();
        }
    }
}
