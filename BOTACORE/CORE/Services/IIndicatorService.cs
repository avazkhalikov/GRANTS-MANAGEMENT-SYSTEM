using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.Services
{
    public interface IIndicatorService
    {
        IndicatorLabel GetIndicatorLabel(int IndicatorLabelID);
        IndicatorCategoryLabel GetIndicatorCategoryLabel(int IndicatorCategoryLabelID);
        IEnumerable<IndicatorLabel> GetIndicatorLabelsByProjectID(int ProjectID);
        IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabelsByProjectID(int ProjectID);
        IndicatorItem GetIndicatorItem(int IndicatorItemID);
        IEnumerable<IndicatorItem> GetIndicatorItems(int IndicatorCategoryLabelID, int ProposalID);
        IEnumerable<IndicatorItem> GetIndicatorItems(int ProposalID);
        IEnumerable<IndicatorCategoryLabel> GetIndicatorCategories(int ProposalID);
        Indicator GetIndicator(int IndicatorID);
        bool InsertIndicatorTemplateItem(IndicatorTemplateItem item);
        bool InsertIndicatorItem(IndicatorItem item);
        bool InsertIndicatorCategoryLabel(IndicatorCategoryLabel item);
        bool InsertIndicatorLabel(IndicatorLabel item);
        bool DeleteIndicatorItem(int IndicatorItemID);
        bool UpdateIndicatorItem(IndicatorItem item);
        bool UpdateIndicatorItems(IEnumerable<IndicatorItem> items);

        bool UpdateIndicatorLabel(IndicatorLabel item);
        bool DeleteIndicatorLabel(int IndicatorLabelID);
        bool UpdateIndicatorCategoryLabel(IndicatorCategoryLabel item);
        bool DeleteIndicatorCategoryLabel(int IndicatorCategoryLabelID);

        IEnumerable<IndicatorLabel> GetIndicatorLabelsAll();
        IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabelsAll();
        IEnumerable<IndicatorCategoryLabel> GetIndicatorTemplateCategories(int GrantTypeCodeID);

        IEnumerable<IndicatorLabel> GetIndicatorLabels(int GrantTypeCodeID);
        IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabels(int GrantTypeCodeID);
        IEnumerable<IndicatorTemplateItem> GetIndicatorTemplateItems(int IndicatorCategoryLabelID);
        bool DeleteIndicatorTemplateItem(int IndicatorTemplateItemID);
        bool InsertFromTemplate(int ProjectID);
        bool DeleteIndicator(int ProjectID);
    }
}
