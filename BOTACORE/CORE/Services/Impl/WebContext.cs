using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Services;

namespace BOTACORE.CORE.Services.Impl
{
   public class WebContext : IWebContext
    {

       public SSPStaff CurrentUser
       {
           get
           {
               if (ContainsInSession("CurrentUser"))
               {
                   return GetFromSession("CurrentUser") as SSPStaff;
               }

               return null;
           }
           set
           {
               SetInSession("CurrentUser", value);
           }
       }


       public string ProjectLabel
       {
           get
           {
               if (ContainsInSession("ProjectLabel"))
               {
                   return (string)GetFromSession("ProjectLabel");
               }

               return null;
           }
           set
           {
               SetInSession("ProjectLabel", value);
           }
       }

       public string OrganizationName
       {
           get
           {
               if (ContainsInSession("OrganizationName"))
               {
                   return (string)GetFromSession("OrganizationName");
               }

               return null;
           }
           set
           {
               SetInSession("OrganizationName", value);
           }
       }


       public int OrgID
       {
           get
           {
               if (ContainsInSession("OrgID"))
               {
                   return (int)GetFromSession("OrgID"); 
               }

               return 0; 
               
           }
           set
           {
               SetInSession("OrgID", value);
           }
       }



       public int ProposalID
       {
           get
           {
               if (ContainsInSession("ProposalID"))
               {
                   return (int)GetFromSession("ProposalID");
               }

               return 0;

           }
           set
           {
               SetInSession("ProposalID", value);
           }
       }

        public string FilePath
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString();
            }
        }

        /// <summary>
        /// Final->  Attachments\\A1234567\\
        /// </summary>
        public string FilePathToAttachments
        {
            get
            {
                return this.FilePath + "Attachments\\";
            }
        }


        public Int32 FileTypeID
        {
            get
            {
                Int32 result;
                if (!string.IsNullOrEmpty(GetQueryStringValue("FileTypeID")))
                    result = Convert.ToInt32(GetQueryStringValue("FileTypeID"));
                else
                    result = 0;

                return result;
            }
        }

        public Int32 EventID
        {
            get
            {
                Int32 result;
                if (!string.IsNullOrEmpty(GetQueryStringValue("EventID")))
                    result = Convert.ToInt32(GetQueryStringValue("EventID"));
                else
                    result = 0;
                return result;
            }
        }

      

        public string SearchText
        {
            get
            {
                string result;
                if (!string.IsNullOrEmpty(GetQueryStringValue("s")))
                {
                    result = GetQueryStringValue("s");
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }


        public Int32 AccountID
        {
            get
            {
                if (!string.IsNullOrEmpty(GetQueryStringValue("AccountID")))
                {
                    return Convert.ToInt32(GetQueryStringValue("AccountID"));
                }
                return 0;
            }
        }

       

        public bool LoggedIn
        {
            get
            {
                if (ContainsInSession("LoggedIn"))
                {
                    if ((bool)GetFromSession("LoggedIn"))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                SetInSession("LoggedIn", value);
            }
        }

       /// <summary>
       /// Session Actions.
       /// </summary>
        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public bool ContainsInSession(string key)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null)
                return true;
            return false;
        }

        public void RemoveFromSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        private string GetQueryStringValue(string key)
        {
            return HttpContext.Current.Request.QueryString.Get(key);
        }

        private void SetInSession(string key, object value)
        {
            ////////////////////BUG//////////////////////
            HttpContext.Current.Session.Timeout = 620;
            //////////////////////////////////////////
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return;
            }
            HttpContext.Current.Session[key] = value;
        }

        private object GetFromSession(string key)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return null;
            }
            return HttpContext.Current.Session[key];
        }

        private void UpdateInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }
}
