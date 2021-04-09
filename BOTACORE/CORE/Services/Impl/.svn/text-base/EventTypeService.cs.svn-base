using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Impl;
using BOTACORE.CORE.DataAccess;

namespace BOTACORE.CORE.Services.Impl
{
    public class EventTypeService:IEventTypeService
    {
        #region IEventTypeService Members
        private IEventTypeRepository _EventTypeRepository;
        public EventTypeService()
        {
            _EventTypeRepository = RepositoryFactory.EventTypeRepository();
        }
        public IEnumerable<EventType> GetEventTypeList()
        {
            return _EventTypeRepository.GetEventTypeList();
        }

        #endregion
    }
}
