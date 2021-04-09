using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace BOTACORE.CORE.Services.Impl
{
    public class WordTemplateService:IWordTemplateService
    {
        public string GenerateDocument(string Template, Dictionary<string, string> Values)
        {
            //string result = "";
            /*//            XmlTextReader reader = new XmlTextReader(path);
            StreamReader SR;
            string S = "";
            SR = File.OpenText(path);
            S = SR.ReadLine();
            while (S != null)
            {
                result += S + System.Environment.NewLine;
                S = SR.ReadLine();
            }
            SR.Close();
            */
            foreach (var item in Values)
            {
                Template = Template.Replace(item.Key, item.Value);
            }

            return Template;
        }

        private static string GetFields(string Template)
        {
            StringBuilder sb = new StringBuilder();
            string param = "";
            XmlTextReader reader = new XmlTextReader(new StringReader(Template));
            while (reader.Read())
            {
                if (!((reader.NodeType == XmlNodeType.Whitespace) | (reader.NodeType == XmlNodeType.SignificantWhitespace) | (reader.NodeType == XmlNodeType.Element) | (reader.NodeType == XmlNodeType.EndElement)))
                {
                    if (reader.Value.Contains("{") & reader.Value.Contains("}"))
                    {
                        param = reader.Value;
                        while (param.Contains("{") & param.Contains("}"))
                        {

                            sb.AppendLine("Parametr name : " + param.Substring(param.IndexOf("{"), param.IndexOf("}") - param.IndexOf("{") + 1) + " &nbsp;&nbsp;");

                            //param.Replace(param.Substring(param.IndexOf("{"), param.IndexOf("}") - param.IndexOf("{") + 1), "");
                            param = param.Remove(param.IndexOf("{"), (param.IndexOf("}") - param.IndexOf("{") + 1));
                            //param = "";
                        }
                        sb.AppendLine("<br />");
                    }

                    sb.AppendLine("reader.Value: " + reader.Value + "<br />");
                    sb.AppendLine("reader.LineNumber: " + reader.LineNumber.ToString() + "<br />");
                    sb.AppendLine("reader.LinePosition: " + reader.LinePosition.ToString() + "<br />");
                    sb.AppendLine("reader.LocalName: " + reader.LocalName.ToString() + "<br />");
                    sb.AppendLine("reader.Name: " + reader.Name.ToString() + "<br />");
                    sb.AppendLine("reader.NodeType: " + reader.NodeType.ToString() + "<br />");
                    sb.AppendLine("reader.ValueType: " + reader.ValueType.ToString() + "<br />");

                    sb.AppendLine("reader.Prefix: " + reader.Prefix.ToString() + "<br />");
                    sb.AppendLine("reader.AttributeCount: " + reader.AttributeCount.ToString() + "<br />");
                    sb.AppendLine("Attributes: ");
                    for (int i = 0; i < reader.AttributeCount; i++)
                    {
                        sb.AppendLine(i.ToString() + " - " + reader.GetAttribute(i) + "; ");
                    }
                    sb.AppendLine("<br />");
                    sb.AppendLine("reader.EOF: " + reader.EOF.ToString() + "<br />");
                    sb.AppendLine("reader.reader.HasAttributes: " + reader.HasAttributes.ToString() + "<br />");
                    sb.AppendLine("reader.HasValue: " + reader.HasValue.ToString() + "<br />");
                    sb.AppendLine("reader.IsEmptyElement: " + reader.IsEmptyElement.ToString() + "<br />");
                    sb.AppendLine("<hr /><br />");
                }
                //                i++;
            }
            return sb.ToString();
        }

    }
}
