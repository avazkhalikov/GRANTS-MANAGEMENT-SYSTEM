using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
namespace BOTACORE.CORE.DataAccess
{
    public interface ISSPStaffRepository
    {
        IEnumerable<SSPStaff> GetSSPStaffListForProject(int ProjectID);
        bool DeleteSSPAccount(int SSPStaffID);
        SSPStaff GetAccountByID(int AccountID);
        bool SaveAccount(SSPStaff account);
        SSPStaff GetSSPStaffByName(string UserName);
        IEnumerable<RolesSSPStaff> GetALLSSPRoles();
        RolesSSPStaff GetSSPRoleByID(int RoleID);
        IEnumerable<SSPStaff> GetSSPStaffList();
        bool InsertSSPStaffIntoProject(int SSPStaffID, int id);
        bool DeleteSSPStaffFromProject(int SSPStaffID, int id);
        IEnumerable<ViewStaffProject> GetSSPStaffProject(int ProjectID);
        IEnumerable<SSPStaffProject> GetSSPStaffProjectt(int id);
        bool UpdateStaffProject(int id, List<int> current);
        List<PageAccess> GetPageAccessByRole(int roleID);
        bool DeletePageAccess(int? id);
        List<PageAccess> GetPageAccessList();
        PageAccess GetPageAccessByID(int id);        
        bool CreatePageAccess(PageAccess pa);
        bool UpdatePageAccess(PageAccess pa);
        

    }
}
