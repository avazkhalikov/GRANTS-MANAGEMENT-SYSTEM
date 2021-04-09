using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.DataAccess
{
    public interface ITemplateRepository
    {
        IEnumerable<TemplateDocument> GetTemplates();
        IEnumerable<TemplateDocument> GetTemplates(int EventTypeID);
        TemplateDocument GetTemplateDocument(int id);
        TemplateDocument GetTemplateDocument(string TemplateName);
        bool InsertTemplateDocument(TemplateDocument item);
        bool UpdateTemplateDocument(TemplateDocument item);
        bool DeleteTemplateDocument(int id);
        bool UpdateTemplateFile(TemplateFile item);
        bool DeleteTemplateFile(int id);

        IEnumerable<EventFieldMapper> GetMapperValues();
        string GenerateDocumentFromTemplateName(int TemplateDocsID, int EventID, int ProjectID, int? ReportPeriodID);
        string GenerateDocumentFromTemplateName(int TemplateDocsID, int EventID, int ProjectID, string PhysicalAbsolutePathOfSite, int? ReportPeriodID);
        string ReplaceTemplateKeywordsWithValues(string TemplatePath, int EventID, int ProjectID, int? ReportPeriodID);
        List<string> GetTemplateKeywords(string path);
    }
}
