using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.DataAccess;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.Services.Impl
{
    public class IndicatorService:IIndicatorService
    {
        private IIndicatorRepository _rep;
        public IndicatorService()
        {
            _rep = RepositoryFactory.IndicatorRepository();
        }
        #region IIndicatorService Members

        public IndicatorLabel GetIndicatorLabel(int IndicatorLabelID)
        {
            return _rep.GetIndicatorLabel(IndicatorLabelID);
        }

        public IndicatorCategoryLabel GetIndicatorCategoryLabel(int IndicatorCategoryLabelID)
        {
            return _rep.GetIndicatorCategoryLabel(IndicatorCategoryLabelID);
        }

        public IEnumerable<IndicatorLabel> GetIndicatorLabelsByProjectID(int ProjectID)
        {
            return _rep.GetIndicatorLabelsByProjectID(ProjectID);
        }

        public IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabelsByProjectID(int ProjectID)
        {
            return _rep.GetIndicatorCategoryLabelsByProjectID(ProjectID);
        }

        public IndicatorItem GetIndicatorItem(int IndicatorItemID)
        {
            return _rep.GetIndicatorItem(IndicatorItemID);
        }

        public IEnumerable<IndicatorItem> GetIndicatorItems(int IndicatorCategoryLabelID, int ProposalID)
        {
            return _rep.GetIndicatorItems(IndicatorCategoryLabelID, ProposalID);
        }

        public IEnumerable<IndicatorItem> GetIndicatorItems(int ProposalID)
        {
            return _rep.GetIndicatorItems(ProposalID);
        }
        public IEnumerable<IndicatorCategoryLabel> GetIndicatorCategories(int ProposalID)
        {
            return _rep.GetIndicatorCategories(ProposalID);
        }

        public Indicator GetIndicator(int IndicatorID)
        {
            return _rep.GetIndicator(IndicatorID);
        }
        public bool InsertIndicatorItem(IndicatorItem item)
        {
            return _rep.InsertIndicatorItem(item);
        }
        public bool InsertIndicatorCategoryLabel(IndicatorCategoryLabel item)
        {
            return _rep.InsertIndicatorCategoryLabel(item);
        }
        public bool InsertIndicatorLabel(IndicatorLabel item)
        {
            return _rep.InsertIndicatorLabel(item);
        }
        public bool DeleteIndicatorItem(int IndicatorItemID)
        {
            return _rep.DeleteIndicatorItem(IndicatorItemID);
        }
        public bool UpdateIndicatorItem(IndicatorItem item)
        {
            return _rep.UpdateIndicatorItem(item);
        }
        
        public IEnumerable<IndicatorLabel> GetIndicatorLabelsAll()
        {
            return _rep.GetIndicatorLabelsAll();
        }

        public IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabelsAll()
        {
            return _rep.GetIndicatorCategoryLabelsAll();
        }

        public bool UpdateIndicatorLabel(IndicatorLabel item)
        {
            return _rep.UpdateIndicatorLabel(item);
        }
        public bool DeleteIndicatorLabel(int IndicatorLabelID)
        {
            return _rep.DeleteIndicatorLabel(IndicatorLabelID);
        }
        public bool UpdateIndicatorCategoryLabel(IndicatorCategoryLabel item)
        {
            return _rep.UpdateIndicatorCategoryLabel(item);
        }
        public bool DeleteIndicatorCategoryLabel(int IndicatorCategoryLabelID)
        {
            return _rep.DeleteIndicatorCategoryLabel(IndicatorCategoryLabelID);
        }
        public IEnumerable<IndicatorCategoryLabel> GetIndicatorTemplateCategories(int GrantTypeCodeID)
        {
            return _rep.GetIndicatorTemplateCategories(GrantTypeCodeID);
        }

        public IEnumerable<IndicatorLabel> GetIndicatorLabels(int GrantTypeCodeID)
        {
            return _rep.GetIndicatorLabels(GrantTypeCodeID);
        }

        public IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabels(int GrantTypeCodeID)
        {
            return _rep.GetIndicatorCategoryLabels(GrantTypeCodeID);
        }

        public bool InsertIndicatorTemplateItem(IndicatorTemplateItem item)
        {
            return _rep.InsertIndicatorTemplateItem(item);
        }

        public IEnumerable<IndicatorTemplateItem> GetIndicatorTemplateItems(int IndicatorCategoryLabelID)
        {
            return _rep.GetIndicatorTemplateItems(IndicatorCategoryLabelID);
        }

        public bool DeleteIndicatorTemplateItem(int IndicatorTemplateItemID)
        {
            return _rep.DeleteIndicatorTemplateItem(IndicatorTemplateItemID);
        }

        public bool InsertFromTemplate(int ProjectID)
        {
            return _rep.InsertFromTemplate(ProjectID);
        }

        public bool UpdateIndicatorItems(IEnumerable<IndicatorItem> items)
        {
            return _rep.UpdateIndicatorItems(items);
        }
        public bool DeleteIndicator(int ProjectID)
        {
            return _rep.DeleteIndicator(ProjectID);
        }

        #endregion
    }
}
