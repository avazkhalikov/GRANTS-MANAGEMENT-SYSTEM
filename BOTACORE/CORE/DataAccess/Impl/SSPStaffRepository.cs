using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using StructureMap;
using System.Data.Linq;

namespace BOTACORE.CORE.DataAccess.Impl
{
    public class SSPStaffRepository:ISSPStaffRepository
    {
       private string connectString;
       public SSPStaffRepository()
       {
            //string connString = Settings.Default.EntityConnection; 
          //  Context = new BOTADBEntities1();
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString();
       
       }


      public bool DeleteSSPAccount(int SSPStaffID)
      {
            BOTADataContext db = new BOTADataContext(connectString);

            bool result = true;

            try
            {
                var toDelete = (from s in db.SSPStaffs
                                where s.SSPStaffID == SSPStaffID
                                select s).FirstOrDefault();

                if (toDelete != null)
                {
                    db.SSPStaffs.DeleteOnSubmit(toDelete);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


       public bool DeleteSSPStaffFromProject(int SSPStaffID, int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from s in db.SSPStaffProjects
                     where s.ProjectID == id && s.SSPStaffID == SSPStaffID
                     select s).FirstOrDefault();

               if (toDelete != null)
               {
                   db.SSPStaffProjects.DeleteOnSubmit(toDelete);
                   db.SubmitChanges();
               }
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }

      /// <summary>
      /// Inserts staff into project.
      /// </summary>
      /// <param name="SSPStaffID"></param>
      /// <param name="id"></param>
      /// <returns></returns>
       public bool InsertSSPStaffIntoProject(int SSPStaffID, int id)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {  
               //TODO: I should also check if such project and user exists.

               if (!db.SSPStaffProjects.Any(l => l.SSPStaffID == SSPStaffID && l.ProjectID == id))  // !if exists. 
               {
                   SSPStaffProject sspproject = new SSPStaffProject();
                   sspproject.ProjectID = id;
                   sspproject.SSPStaffID = SSPStaffID; 
                  
                   db.SSPStaffProjects.InsertOnSubmit(sspproject);

               }
               else
               {
                   return false; 
               }

               db.SubmitChanges();


           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }

    
       /// <summary>
       /// Get the Role Name for given RoleID.
       /// </summary>
       /// <param name="RoleID"></param>
       /// <returns></returns>
       public RolesSSPStaff  GetSSPRoleByID(int RoleID)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           RolesSSPStaff result = (from r in db.RolesSSPStaffs
                             where r.RoleID == RoleID
                             select r).FirstOrDefault(); 
           return result;    
       }

        /// <summary>
        ///For Drop Down.
        /// </summary>
        /// <returns></returns>
       public IEnumerable<RolesSSPStaff> GetALLSSPRoles()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           var result = from ls in db.RolesSSPStaffs
                        select ls;
           return result;
       
       }

       public SSPStaff GetSSPStaffByName(string UserName)
       {
           SSPStaff account = null;

           BOTADataContext db = new BOTADataContext(connectString);
           {
               try
               {
                   account = (from a in db.SSPStaffs
                              where a.username == UserName
                              select a).FirstOrDefault();
               }
               catch
               {
                   //oops
               }
           }

           return account;
       }


       #region Page ACCESS ====================================================

       public PageAccess GetPageAccessByID(int id)
       {
           PageAccess pa = null;
           BOTADataContext db = new BOTADataContext(connectString);
           {
               try
               {
                   pa = (from a in db.PageAccesses
                         where a.ID == id
                         select a).FirstOrDefault();
                   return pa; 
               }
               catch
               {
                   return null;
               }
           }                    
       }

       public bool CreatePageAccess(PageAccess pa)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);

           try
           {
               //TODO: I should also check if such project and user exists.

               if (!db.PageAccesses.Any(l => l.Controller == pa.Controller && l.Action == pa.Action && l.RoleId == pa.RoleId))  // !if exists. 
               {
                  
                   db.PageAccesses.InsertOnSubmit(pa);

               }
               else
               {
                   return false;
               }

               db.SubmitChanges();


           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }
       public bool UpdatePageAccess(PageAccess pa)
       {

           BOTADataContext db = new BOTADataContext(connectString);
           {
               try
               {

                   if (pa.ID > 0)
                   {
                       db.PageAccesses.Attach(pa);
                       db.Refresh(RefreshMode.KeepCurrentValues, pa);

                   }
                 
                   db.SubmitChanges();
                   return true;
               }
               catch
               {
                   return false;
               }
           }
       }


       public List<PageAccess> GetPageAccessList()
       {
           IEnumerable<PageAccess> pa = null;

           BOTADataContext db = new BOTADataContext(connectString);
           {
               try
               {
                   pa = (from a in db.PageAccesses
                          select a); //.FirstOrDefault();
               }
               catch
               {
                   return null;
               }
           }

           if (pa.Any())
               return pa.ToList();
           else
               return null; 
       }

       public bool DeletePageAccess(int? id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           bool result = true;

           try
           {
               var toDelete = (from s in db.PageAccesses
                               where s.ID == id 
                               select s).FirstOrDefault();

               if (toDelete != null)
               {
                   db.PageAccesses.DeleteOnSubmit(toDelete);
                   db.SubmitChanges();
               }
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result;
       }


       public List<PageAccess> GetPageAccessByRole(int roleID)
       {
           IEnumerable<PageAccess> pa = null;

           BOTADataContext db = new BOTADataContext(connectString);
           {
               try
               {
                   pa = (from a in db.PageAccesses
                              where a.RoleId == roleID
                              select a); //.FirstOrDefault();
               }
               catch
               {
                   return null; 
               }
           }

           if (pa.Any())
               return pa.ToList();
           else
               return null; 
       }
      
       
       #endregion ==============================================================


       /// <summary>
        /// It either updates existing account or
        /// if such account does not exist then inserts.
        /// </summary>
        /// <param name="account"></param>
       public bool SaveAccount(SSPStaff account)
       {

           BOTADataContext db = new BOTADataContext(connectString);
           {
               try
               {

                   if (account.SSPStaffID > 0)
                   {
                       db.SSPStaffs.Attach(account);
                       db.Refresh(RefreshMode.KeepCurrentValues, account);

                   }
                   else
                   {
                       db.SSPStaffs.InsertOnSubmit(account);
                   }
                   db.SubmitChanges();

                   return true;
               }
               catch
               {
                   return false; 
               }
           }
       }


       public SSPStaff GetAccountByID(int AccountID)
       {
           SSPStaff account = null;

           BOTADataContext db = new BOTADataContext(connectString);
           {
                account = (from a in db.SSPStaffs
                          where a.SSPStaffID == AccountID
                          select a).FirstOrDefault();
           }

           return account;
       }


       public bool UpdateBudgetInitialAmount(int initialamt, int budgetID)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               Budget budget = (from b in db.Budgets
                                where b.BudgetID == budgetID
                                select b).FirstOrDefault();

               budget.ContractInitialAmt = initialamt;

               //db.Budgets.Attach();
               db.Refresh(RefreshMode.KeepCurrentValues, budget);
               db.SubmitChanges();
               return true;
           }
           catch (Exception ex)
           {
               return false;
           }

           //return result;
       }

       public bool UpdateStaffProject(int id, List<int> current)
       {

           BOTADataContext db = new BOTADataContext(connectString);
           {
               try
               {
                 var sspproj  = from a in db.SSPStaffProjects
                               where a.ProjectID == id
                               select a;

                   foreach (var vsp in sspproj)
                   {
                       if (current != null)
                       {
                           if(current.Any(s=>s==vsp.SSPStaffID))
                           {
                               vsp.Active = true;
                           }
                           else
                           {
                               vsp.Active = false; 
                           }
                       }
                       else
                       {
                           vsp.Active = false;
                       }
                   }

                 
                  // db.SSPStaffProjects.AttachAll(sspproj);
                   db.Refresh(RefreshMode.KeepCurrentValues, sspproj);
                   db.SubmitChanges();

                   return true;
               }
               catch
               {
                   return false;
               }
           }
           
           return false; 
       }

       //№	Grant #	/Organization name/	Grant type/	Area/	Round /Status
       public IEnumerable<ViewStaffMyProject> GetSSPStaffProjects(int StaffID, int all)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           if (all == 1) //active and checked only
           {
               IEnumerable<ViewStaffMyProject> result = (from s in db.SSPStaffs
                                                         join r in db.RolesSSPStaffs on s.RoleID equals r.RoleID
                                                         join p in db.SSPStaffProjects on s.SSPStaffID equals p.SSPStaffID
                                                         join prj in db.Projects on p.ProjectID equals prj.ProjectID
                                                         where p.SSPStaffID == StaffID &&
                                                         p.Active == true // && prj.ProposalStatus.PropStatusID == 2  //only checked and active grants
                                                         && prj.ProposalStatus.PropStatusID == 2 //Active grant
                                                         select new ViewStaffMyProject
                                                         {
                                                             sspid = s.SSPStaffID,
                                                             OrgName = prj.Organization.General.NameRu,
                                                             GrantType = prj.GrantType.GrantTypeList.GrantTypeText,
                                                             Area = prj.ProgramArea.ProgramAreaList.ProgramAreaText,
                                                             Round = prj.CompetitionCode.CompetitionCodeList.CodeText,
                                                             Status = prj.ProposalStatus.ProposalStatusList.ProposalStatusText,
                                                             projid = p.ProjectID,
                                                             projectLabel = prj.Label
                                                         }
                            ).Distinct();
               return result;
           }
           else  //gets all where his name appears.
           {
               IEnumerable<ViewStaffMyProject> result = (from s in db.SSPStaffs
                                                         join r in db.RolesSSPStaffs on s.RoleID equals r.RoleID
                                                         join p in db.SSPStaffProjects on s.SSPStaffID equals p.SSPStaffID
                                                         join prj in db.Projects on p.ProjectID equals prj.ProjectID
                                                         where p.SSPStaffID == StaffID && p.Active == true
                                                         select new ViewStaffMyProject
                                                         {
                                                             sspid = s.SSPStaffID,
                                                             OrgName = prj.Organization.General.NameRu,
                                                             GrantType = prj.GrantType.GrantTypeList.GrantTypeText,
                                                             Area = prj.ProgramArea.ProgramAreaList.ProgramAreaText,
                                                             Round = prj.CompetitionCode.CompetitionCodeList.CodeText,
                                                             Status = prj.ProposalStatus.ProposalStatusList.ProposalStatusText,
                                                             projid = p.ProjectID,
                                                             projectLabel = prj.Label
                                                         }
                              ).Distinct();
               return result;
           }

           
       }

       //№	Grant #	/Organization name/	Grant type/	Area/	Round /Status
       public IEnumerable<StaffGrantHolder> GetSSPStaffsProjects(int status)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           IEnumerable<ViewStaffMyProject> result = null;

           if (status == 1)  //active.
           {
               result = (from s in db.SSPStaffs
                         join p in db.SSPStaffProjects on s.SSPStaffID equals p.SSPStaffID
                         join prj in db.Projects on p.ProjectID equals prj.ProjectID
                         where p.Active == true // && prj.ProposalStatus.PropStatusID == 2  //only checked and active grants
                         && prj.ProposalStatus.PropStatusID == 2 //Active grant
                         select new ViewStaffMyProject
                         {
                             sspid = s.SSPStaffID,
                             OrgName = prj.Organization.General.NameRu,
                             grantType = prj.GrantType,
                             programArea = prj.ProgramArea,
                             Round = prj.CompetitionCode.CompetitionCodeList.CodeText,
                             Status = prj.ProposalStatus.ProposalStatusList.ProposalStatusText,
                             projid = p.ProjectID,
                             FirstName = s.FirstName,
                             LastName = s.LastName
                         }
                           ).Distinct();
           }
           if (status == 2) //all 
           {
               result = (from s in db.SSPStaffs
                         join p in db.SSPStaffProjects on s.SSPStaffID equals p.SSPStaffID
                         join prj in db.Projects on p.ProjectID equals prj.ProjectID
                         where p.Active == true // && prj.ProposalStatus.PropStatusID == 2  //only checked and active grants                         
                         select new ViewStaffMyProject
                         {
                             sspid = s.SSPStaffID,
                             OrgName = prj.Organization.General.NameRu,
                             grantType = prj.GrantType,
                             programArea = prj.ProgramArea,
                             Round = prj.CompetitionCode.CompetitionCodeList.CodeText,
                             Status = prj.ProposalStatus.ProposalStatusList.ProposalStatusText,
                             projid = p.ProjectID,
                             FirstName = s.FirstName,
                             LastName = s.LastName
                         }
                            ).Distinct();
           }

           var query = result
                 .GroupBy(g => new 
                 {
                     g.sspid
                 })
                 .Select(group => new StaffGrantHolder() //Select all Grouped into VersusContainer.
                 {
                     ProgramArea = group.Select(i=>i.programArea).ToList(),
                     grantType= group.Select(i => i.grantType).ToList(),
                     sspid = group.Key.sspid,
                     FirstName = group.Select(i=>i.FirstName).FirstOrDefault(),
                     LastName = group.Select(i => i.LastName).FirstOrDefault(),
                 });


           return query;
       }


     
    


         /// <summary>
        /// Get SSP Staff List of asked Project.
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
       public IEnumerable<SSPStaff> GetSSPStaffListForProject(int ProjectID)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           IEnumerable<SSPStaff> result;
           result = (from s in db.SSPStaffs
                    join r in db.RolesSSPStaffs on s.RoleID equals r.RoleID
                    join p in db.SSPStaffProjects on s.SSPStaffID equals p.SSPStaffID
                    where p.ProjectID == ProjectID
                    select s).Distinct(); 

           /* result = (from s in db.SSPStaffProjects
                     where s.ProjectID == ProjectID
                     select s.SSPStaff).Distinct();  */

           return result;
       }


       public IEnumerable<SSPStaffProject> GetSSPStaffProjectt(int id)
       {

           BOTADataContext db = new BOTADataContext(connectString);

           var ssppr = from a in db.SSPStaffProjects
                        where a.ProjectID == id
                        select a;
           return ssppr; 

       }



       public IEnumerable<ViewStaffProject> GetSSPStaffProject(int ProjectID)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           IEnumerable<ViewStaffProject> result = (from s in db.SSPStaffs
                                                   join r in db.RolesSSPStaffs on s.RoleID equals r.RoleID
                                                   join p in db.SSPStaffProjects on s.SSPStaffID equals p.SSPStaffID
                                                   where p.ProjectID == ProjectID
                                                   select new ViewStaffProject
                                                   {
                                                       sspid = s.SSPStaffID,
                                                       roleId = r.RoleID,
                                                       RoleName = r.RoleName,
                                                       FirstName = s.FirstName,
                                                       LastName = s.LastName,
                                                       Active = p.Active,
                                                       projid = p.ProjectID
                                                   }
                        ).Distinct();

           return result;
       }

        /// <summary>
        /// Get All SSP Staff
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
       public IEnumerable<SSPStaff> GetSSPStaffList()
       {
           BOTADataContext db = new BOTADataContext(connectString);
           IEnumerable<SSPStaff> result;
           result = (from s in db.SSPStaffs
                     join r in db.RolesSSPStaffs on s.RoleID equals r.RoleID
                     select s).Distinct();
           return result;
       }

      
    }
}
