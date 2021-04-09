using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
namespace BOTACORE.CORE.Services
{
    public interface ISSPStaffService
    {
        IEnumerable<SSPStaff> GetSSPStaffListForProject(int ProjectID);
        SSPStaff GetAccountByID(Int32 AccountID);
        string Login(string Username, string Password);
        bool UsernameInUse(string Username);
        bool CreateNewAccount(SSPStaff account);
        RolesSSPStaff  GetSSPRoleByID(int RoleID);
        IEnumerable<RolesSSPStaff> GetALLSSPRoles();
        IEnumerable<SSPStaff> GetSSPStaffList();
        bool InsertSSPStaffIntoProject(int SSPStaffID, int id);
        bool DeleteSSPStaffFromProject(int SSPStaffID, int id);
        IEnumerable<ViewStaffProject> GetSSPStaffProject(int ProjectID);
        List<PageAccess> GetPageAccessByRole(int roleID);
        bool DeletePageAccess(int? id);
        List<PageAccess> GetPageAccessList();
        PageAccess GetPageAccessByID(int id);
        bool CreatePageAccess(PageAccess pa);
        bool UpdatePageAccess(PageAccess pa);
    }
}
