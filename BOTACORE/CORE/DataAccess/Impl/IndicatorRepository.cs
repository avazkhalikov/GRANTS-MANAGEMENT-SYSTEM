using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;

namespace BOTACORE.CORE.DataAccess.Impl
{
    public class IndicatorRepository:IIndicatorRepository
    {
        #region IIndicatorRepository Members
        private string connectString;
        private BOTADataContext db;
        public IndicatorRepository()
       {
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();
           db = new BOTADataContext(connectString);
       }
        public IndicatorLabel GetIndicatorLabel(int IndicatorLabelID)
        {
            IndicatorLabel result;
            var query = (from i in db.IndicatorLabels
                          where i.IndicatorLabelID == IndicatorLabelID
                          select i).FirstOrDefault();
            result = query;
            return result;
        }

        public IEnumerable<IndicatorLabel> GetIndicatorLabelsAll()
        {
            IEnumerable<IndicatorLabel> result;
            var query = (from i in db.IndicatorLabels
                         select i).Distinct();
            result = query;
            return result;
        }

        public IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabelsAll()
        {
            IEnumerable<IndicatorCategoryLabel> result;
            var query = (from i in db.IndicatorCategoryLabels
                         select i).Distinct();
            result = query;
            return result;
        }


        public bool InsertIndicatorLabel(IndicatorLabel item)
        {
            bool result=true;
            try
            {
                
                db.IndicatorLabels.InsertOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex);
                result = false;
               // throw ex;
            }
            return result;
        }

        private void IfIndicatorCreate(int IndicatorID)
        {
            var inditem = (from i in db.Indicators
                           where i.IndicatorID == IndicatorID
                           select i).FirstOrDefault();
            if (inditem == null)
            {
                Indicator item = new Indicator();
                item.IndicatorID = IndicatorID;
                item.IndicatorName = "Indicator";
                db.Indicators.InsertOnSubmit(item);
                db.SubmitChanges();
            }
        }

        public IndicatorCategoryLabel GetIndicatorCategoryLabel(int IndicatorCategoryLabelID)
        {
            IndicatorCategoryLabel result;
            var query = (from i in db.IndicatorCategoryLabels
                         where i.IndicatorCategoryLabelID == IndicatorCategoryLabelID
                         select i).FirstOrDefault();
            result = query;
            return result;
        }

        public bool InsertIndicatorCategoryLabel(IndicatorCategoryLabel item)
        {
            bool result = true;
            try
            {
                db.IndicatorCategoryLabels.InsertOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
               // throw ex;
            }
            return result;
        }

        public IEnumerable<IndicatorLabel> GetIndicatorLabelsByProjectID(int ProjectID)
        {
            IEnumerable<IndicatorLabel> result;
            var query = (from i in db.IndicatorLabels
                         join gt in db.GrantTypes
                         on i.GrantTypeCodeID equals gt.GrantTypeCodeID
                         where gt.ProjectID == ProjectID
                         select i);
            result = query;
            return result;
        }

        public IEnumerable<IndicatorLabel> GetIndicatorLabels(int GrantTypeCodeID)
        {
            IEnumerable<IndicatorLabel> result;
            var query = (from i in db.IndicatorLabels
                         where i.GrantTypeCodeID==GrantTypeCodeID
                         select i);
            result = query;
            return result;
        }
        
        public IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabels(int GrantTypeCodeID)
        {
            IEnumerable<IndicatorCategoryLabel> result;
            var query = (from i in db.IndicatorCategoryLabels
                         where i.GrantTypeCodeID == GrantTypeCodeID
                         select i);
            result = query;
            return result;
        }

        public IEnumerable<IndicatorCategoryLabel> GetIndicatorCategoryLabelsByProjectID(int ProjectID)
        {
            IEnumerable<IndicatorCategoryLabel> result;
            var query = (from i in db.IndicatorCategoryLabels
                         join gt in db.GrantTypes
                         on i.GrantTypeCodeID equals gt.GrantTypeCodeID
                         where gt.ProjectID==ProjectID
                         select i);
            result = query;
            return result;
        }

        public IndicatorItem GetIndicatorItem(int IndicatorItemID)
        {
            IndicatorItem result;
            var query=(from i in db.IndicatorItems
                           where i.IndicatorItemID==IndicatorItemID
                           select i).FirstOrDefault();
            result = query;
            return result;
        }

        public bool InsertIndicatorItem(IndicatorItem item)
        {
            bool result = true;
            IfIndicatorCreate(item.IndicatorID);
            try
            {
                db.IndicatorItems.InsertOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
               // throw ex;
            }
            return result;
        }

        public bool InsertIndicatorTemplateItem(IndicatorTemplateItem item)
        {
            bool result = true;
            try
            {
                db.IndicatorTemplateItems.InsertOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
              //  throw ex;
            }
            return result;
        }

        public bool UpdateIndicatorItem(IndicatorItem item)
        {
            bool result = true;
            try
            {
               db.IndicatorItems.Attach(item);
               db.Refresh(RefreshMode.KeepCurrentValues,item);
               db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
              //  throw ex;
            }
            return result;
        }

        public bool UpdateIndicatorItems(IEnumerable<IndicatorItem> items)
        {
            bool result = true;
            try
            {
                db.IndicatorItems.AttachAll(items);
                db.Refresh(RefreshMode.KeepCurrentValues, items);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
                //throw ex;
            }
            return result;
        }

        public bool UpdateIndicatorLabel(IndicatorLabel item)
        {
            bool result = true;
            try
            {
                db.IndicatorLabels.Attach(item);
                db.Refresh(RefreshMode.KeepCurrentValues, item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
               // throw ex;
            }
            return result;
        }

        public bool DeleteIndicatorLabel(int IndicatorLabelID)
        {
            bool result = true;

            try
            {
                var item = (from i in db.IndicatorLabels
                            where i.IndicatorLabelID == IndicatorLabelID
                            select i).FirstOrDefault();
                db.IndicatorLabels.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
                //throw ex;
            }
            return result;
        }

        public bool UpdateIndicatorCategoryLabel(IndicatorCategoryLabel item)
        {
            bool result = true;
            try
            {
                db.IndicatorCategoryLabels.Attach(item);
                db.Refresh(RefreshMode.KeepCurrentValues, item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
               // throw ex;
            }
            return result;
        }

        public bool DeleteIndicatorCategoryLabel(int IndicatorCategoryLabelID)
        {
            bool result = true;

            try
            {
                var item = (from i in db.IndicatorCategoryLabels
                            where i.IndicatorCategoryLabelID == IndicatorCategoryLabelID
                            select i).FirstOrDefault();
                db.IndicatorCategoryLabels.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
               // throw ex;
            }
            return result;
        }

        public bool DeleteIndicatorItem(int IndicatorItemID)
        {
            bool result = true;

            try
            {
                var item = (from i in db.IndicatorItems
                            where i.IndicatorItemID == IndicatorItemID
                            select i).FirstOrDefault();
                db.IndicatorItems.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
               // throw ex;
            }
            return result;
        }

        public bool DeleteIndicatorTemplateItem(int IndicatorTemplateItemID)
        {
            bool result = true;

            try
            {
                var item = (from i in db.IndicatorTemplateItems
                            where i.IndicatorTemplateItemID == IndicatorTemplateItemID
                            select i).FirstOrDefault();
                db.IndicatorTemplateItems.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
              //  throw ex;
            }
            return result;
        }

        public IEnumerable<IndicatorItem> GetIndicatorItems(int IndicatorCategoryLabelID, int ProposalID)
        {
            IEnumerable<IndicatorItem> result;
            var query=(from i in db.IndicatorItems
                           where i.IndicatorCategoryLabelID == IndicatorCategoryLabelID
                           && i.IndicatorID==ProposalID
                           select i);
            result = query;
            return result;
        }

        public IEnumerable<IndicatorTemplateItem> GetIndicatorTemplateItems(int IndicatorCategoryLabelID)
        {
            IEnumerable<IndicatorTemplateItem> result;
            var query=(from i in db.IndicatorTemplateItems
                           where i.IndicatorCategoryLabelID == IndicatorCategoryLabelID
                           select i);
            result = query;
            return result;
        }

        public IEnumerable<IndicatorCategoryLabel> GetIndicatorCategories(int ProposalID)
        {
            IEnumerable<IndicatorCategoryLabel> result;
            var query = (from i in db.IndicatorItems
                         join c in db.IndicatorCategoryLabels
                         on i.IndicatorCategoryLabelID equals c.IndicatorCategoryLabelID
                         where i.IndicatorID == ProposalID
                         select c).Distinct();
            result = query;
            return result;
        }

        
        public bool InsertFromTemplate(int ProjectID)
        {
            bool result = true;
            var query = (from t in db.IndicatorTemplateItems
                         join ic in db.IndicatorCategoryLabels
                             on t.IndicatorCategoryLabelID equals ic.IndicatorCategoryLabelID
                             join gt in db.GrantTypes
                             on ic.GrantTypeCodeID equals gt.GrantTypeCodeID
                             where gt.ProjectID==ProjectID
                         select t);
            IfIndicatorCreate(ProjectID);
            List<IndicatorItem> tosubmit = new List<IndicatorItem>();
            if (query != null)
            {
                if (query.Count() != 0)
                {
                    foreach (var item in query)
                    {
                        if (item != null)
                        {
                            IndicatorItem newitem = new IndicatorItem();
                            newitem.Baseline = 0;
                            newitem.Benchmark = 0;
                            newitem.Final = 0;
                            newitem.IndicatorID = ProjectID;
                            newitem.IndicatorLabelID = item.IndicatorLabelID;
                            newitem.IndicatorCategoryLabelID = item.IndicatorCategoryLabelID;
                            tosubmit.Add(newitem);
                        }
                    }
                }
            }
            try
            {
                db.IndicatorItems.InsertAllOnSubmit(tosubmit.AsEnumerable());
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
               // result = false;
            }
            return result;
        }

        public IEnumerable<IndicatorCategoryLabel> GetIndicatorTemplateCategories(int GrantTypeCodeID)
        {
            IEnumerable<IndicatorCategoryLabel> result;
            var query = (from c in db.IndicatorCategoryLabels
                         join it in db.IndicatorTemplateItems
                         on c.IndicatorCategoryLabelID equals it.IndicatorCategoryLabelID
                         where c.GrantTypeCodeID==GrantTypeCodeID
                         select c).Distinct();
            result = query;
            return result;
        }



        public IEnumerable<IndicatorItem> GetIndicatorItems(int ProposalID)
        {
            IEnumerable<IndicatorItem> result;
            var query = (from i in db.IndicatorItems
                         where i.IndicatorID == ProposalID
                         orderby i.IndicatorCategoryLabelID, i.IndicatorLabelID
                         select i);
            result = query;
            return result;
        }

        public Indicator GetIndicator(int IndicatorID)
        {
            Indicator result;
            var query=(from i in db.Indicators
                           where i.IndicatorID==IndicatorID
                           select i).FirstOrDefault();
            result = query;
            return result;
        }
        
        public bool DeleteIndicator(int ProjectID)
        {
            bool result = true;

            try
            {
                var item = (from i in db.Indicators
                            where i.IndicatorID == ProjectID
                            select i).FirstOrDefault();
                db.Indicators.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Log.EnsureInitialized();
                Log.Error(typeof(IndicatorRepository), "----------------------------------------------", ex); 
                result = false;
              //  throw ex;
            }
            return result;
        }

        #endregion
    }
}
