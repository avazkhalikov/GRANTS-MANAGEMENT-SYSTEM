using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BOTACORE.CORE.Domain;

namespace BOTACORE.CORE.Services.Impl
{
   public class UserSession : BOTACORE.CORE.Services.IUserSession
    {

        private IWebContext _webContext;

        public UserSession()
        {
            _webContext = ServiceFactory.WebContext();
        }


  
        public int OrgID
        {
            get
            {
                return _webContext.OrgID;
            }
            set
            {
                _webContext.OrgID = value;
            }
        }

        public string ProjectLabel
        {
            get
            {
                return _webContext.ProjectLabel;
            }
            set
            {
                _webContext.ProjectLabel = value;
            }
        }

        public string OrganizationName
        {
            get
            {
                return _webContext.OrganizationName;
            }
            set
            {
                _webContext.OrganizationName = value;
            }
        }


       
        public int ProjectID
        {
            get
            {
                return _webContext.ProposalID;
            }
            set
            {
                _webContext.ProposalID = value;
            }
        }

        public bool LoggedIn
        {
            get
            {
                return _webContext.LoggedIn;
            }
            set
            {
                _webContext.LoggedIn = value;
            }
        }


        public SSPStaff CurrentUser
        {
            get
            {
                return _webContext.CurrentUser;
            }
            set
            {
                _webContext.CurrentUser = value;
            }
        }

       

    }
}
