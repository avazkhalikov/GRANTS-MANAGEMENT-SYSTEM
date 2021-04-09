using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOTACORE.CORE.Services
{
    public interface IWordTemplateService
    {
        string GenerateDocument(string Template, Dictionary<string, string> Values);
    }
}
