using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BOTACORE.CORE.Services.Impl
{
    public class Redirector : BOTACORE.CORE.Services.IRedirector
    {

        public void GoToDefault(int AccountID)
        {
            //redirect to User Profile.
            Redirect("~/Default/Default.aspx?id=" + AccountID.ToString());
        }

        public void GoToErrorPage()
        {
            Redirect("~/Errors/Error.aspx");
        }

        public void GoToAccountLoginPage()
        {
            //redirect to Login page
            Redirect("~/Accounts/Login.aspx");
        }

        private void Redirect(string path)
        {
            HttpContext.Current.Response.Redirect(path);
        }
    }
}
