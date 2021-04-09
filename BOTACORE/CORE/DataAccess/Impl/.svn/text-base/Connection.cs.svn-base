using System;
using System.Configuration;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Xml;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.DataAccess.Impl
{
   public class Connection
    {
       public string GetDirectConnString()
       {             
             try
             {
                 XmlDocument doc = new XmlDocument();
                 doc.Load(HttpContext.Current.Request.PhysicalApplicationPath + "bin/ConnectionStringToUse.xml");

                 XmlNodeList xnl = doc.GetElementsByTagName("directconstring");
                 XmlElement xe = (XmlElement)xnl[0];

                 return xe.InnerText.ToString();
             }
          catch (Exception e)
             {
                 return  Settings1.Default.DBMLCONNECT;
             }
             
       }

       public BOTADataContext GetContext()
       {
           string connString = "";
           try
           {
               XmlDocument doc = new XmlDocument();
               doc.Load(HttpContext.Current.Request.PhysicalApplicationPath + "bin/ConnectionStringToUse.xml");

               XmlNodeList xnl = doc.GetElementsByTagName("environment");
               XmlElement xe = (XmlElement)xnl[0];

               switch (xe.InnerText.ToString().ToLower())
               {
                   case "local":
                       connString = Settings1.Default.DBMLCONNECT;  
                       break;

                   case "development":
                       connString = Settings1.Default.DBMLCONNECT;  //TODO: to be changed.
                       break;

                   case "production":
                       connString = Settings1.Default.DBMLCONNECT;    //TODO: to be changed. 
                       break;

                   default:
                       throw new Exception("No connection string defined in app.config!");
               }
           }
           catch (Exception e)
           {
               connString = Settings1.Default.DBMLCONNECT;
           }

           BOTADataContext db = new BOTADataContext(connString);
         //  fdc.Log = new DebuggerWriter();
           return db;
       }
    }
}
