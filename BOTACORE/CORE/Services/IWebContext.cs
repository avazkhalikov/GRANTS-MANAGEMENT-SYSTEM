using System;
using BOTACORE.CORE.Domain;
namespace BOTACORE.CORE.Services
{
   public interface IWebContext
    {
        int AccountID { get; }
        void ClearSession();
        bool ContainsInSession(string key);
        SSPStaff CurrentUser { get; set; }
        int EventID { get; }
        string FilePath { get; }
        string FilePathToAttachments { get; }
        int FileTypeID { get; }
        bool LoggedIn { get; set; }
        int ProposalID { get; set; }
        int OrgID { get; set; }
        string ProjectLabel { get; set; }
        string OrganizationName { get; set; }
        void RemoveFromSession(string key);
        string SearchText { get; }
    }
}
