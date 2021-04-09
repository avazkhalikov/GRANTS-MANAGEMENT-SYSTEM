using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.DataAccess;
using BOTACORE.CORE.Domain;
namespace BOTACORE.CORE.Services.Impl
{
    public class TemplateService:ITemplateService
    {
        private ITemplateRepository _rep;

        public TemplateService()
        {
            _rep = RepositoryFactory.TemplateRepository();
        }

        public IEnumerable<TemplateDocument> GetTemplates()
        {
            return _rep.GetTemplates();
        }

        public IEnumerable<TemplateDocument> GetTemplates(int EventTypeID)
        {
            return _rep.GetTemplates(EventTypeID);
        }

        public TemplateDocument GetTemplateDocument(int id)
        {
            return _rep.GetTemplateDocument(id);
        }
        public TemplateDocument GetTemplateDocument(string TemplateName)
        {
            return _rep.GetTemplateDocument(TemplateName);
        }

        public bool InsertTemplateDocument(TemplateDocument item)
        {
            return _rep.InsertTemplateDocument(item);
        }

        public bool UpdateTemplateDocument(TemplateDocument item)
        {
            return _rep.UpdateTemplateDocument(item);
        }

        public bool DeleteTemplateDocument(int id)
        {
            return _rep.DeleteTemplateDocument(id);
        }
        public bool UpdateTemplateFile(TemplateFile item)
        {
            return _rep.UpdateTemplateFile(item);
        }
        public bool DeleteTemplateFile(int id)
        {
            return _rep.DeleteTemplateFile(id);
        }

        public IEnumerable<EventFieldMapper> GetMapperValues()
        {
            return _rep.GetMapperValues();
        }

        public string GenerateDocumentFromTemplateName(int TemplateDocsID, int EventID, int ProjectID, int? ReportPeriodID)
        {
            return _rep.GenerateDocumentFromTemplateName(TemplateDocsID, EventID, ProjectID, ReportPeriodID);
        }

        public string GenerateDocumentFromTemplateName(int TemplateDocsID, int EventID, int ProjectID, string PhysicalAbsolutePathOfSite, int? ReportPeriodID)
        {
            return _rep.GenerateDocumentFromTemplateName(TemplateDocsID, EventID, ProjectID, PhysicalAbsolutePathOfSite, ReportPeriodID);
        }

        public string ReplaceTemplateKeywordsWithValues(string TemplatePath, int EventID, int ProjectID, int? ReportPeriodID)
        {
            return _rep.ReplaceTemplateKeywordsWithValues(TemplatePath, EventID, ProjectID, ReportPeriodID);
        }

        public List<string> GetTemplateKeywords(string path)
        {
            return GetTemplateKeywords(path);
        }

    }
}
