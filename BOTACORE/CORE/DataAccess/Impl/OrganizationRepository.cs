using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOTACORE.CORE.Domain;
using System.Data.Linq;
using System.Configuration;
//using BOTACORE.Properties;

namespace BOTACORE.CORE.DataAccess.Impl
{
   public class OrganizationRepository : IOrganizationRepository
    {
         ////////////////------------------
        //db.ExecuteCommand("DELETE FROM addresses WHERE ContactID={0} and AddressTypeID={1}", contactID, addressTypeID);
        //-----------------------------------

       string connectString; 
       public OrganizationRepository()
       {
            //string connString = Settings.Default.EntityConnection; 
          //  Context = new BOTADBEntities1();
         //  connectString = Settings1.Default.DBMLCONNECT;
           Connection conn = new Connection();
           connectString = conn.GetDirectConnString(); //ConfigurationManager.AppSettings["BOTACORE.Settings1.DBMLCONNECT"]; // "Data Source=192.168.33.8;Initial Catalog=BOTADB;Persist Security Info=True;User ID=sa;Password=Laptop1";
               
               //ConfigurationManager.AppSettings["BOTACORE.Settings1.DBMLCONNECT"];
       }




       #region Other Funders Update Delete Insert 

       public bool DeleteOrgOtherFunders(int id)
       {
           bool result = true;
           try
           {
               BOTADataContext db = new BOTADataContext(connectString);

               //using (BOTADataContext db)
               {
                   OtherFunder item = (from o in db.OtherFunders
                                   where o.OtherFundID == id
                                   select o).FirstOrDefault();
                   if (item != null)
                   {
                       db.OtherFunders.DeleteOnSubmit(item);
                       db.SubmitChanges();
                   }


               }
           }
           catch (Exception ex)
           {
               //Log4Net uses its dll, bin/Log4Net.config -> has C:/Logs/errors2.txt
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }


           return result;
       }

       
       public bool InsertNewOrgOtherFunders(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               OtherFunder o = new OtherFunder(); 
               o.OrgID = id;
               db.OtherFunders.InsertOnSubmit(o);
               db.SubmitChanges();

               return true;
           }
           catch(Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               return false;
           }

       }



       /// <summary>
       /// Updates All addreses of current organization.
       /// </summary>
       /// <param name="OrgAddresses"></param>
       /// <returns></returns>
       public bool UpdateOtherFunders(IEnumerable<OtherFunder> enumotherfunders)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {

               db.OtherFunders.AttachAll(enumotherfunders);
               db.Refresh(RefreshMode.KeepCurrentValues, enumotherfunders);
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }

           return result;

       }

    #endregion 


       #region Address Update Delete Insert.

       public bool DeleteOrgAdress(int id)
       {
           bool result = true;
           try
           {
               BOTADataContext db = new BOTADataContext(connectString);

               //using (BOTADataContext db)
               {
                   Address item = (from addrs in db.Addresses
                                   where addrs.AddessID == id
                                   select addrs).First();
                   if (item != null)
                   {
                       db.Addresses.DeleteOnSubmit(item);
                       db.SubmitChanges();
                   }


               }
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }


           return result;
       }



       public bool InsertNewOrgAddress(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               Address addr = new Address();
               
               if (!db.Addresses.Any(l => l.OrgID == id))  // if first address of org then it is the default.
               {
                   addr.isLegalAddress = true;                   
                   
               }
               
               addr.OrgID = id;
               db.Addresses.InsertOnSubmit(addr);
               db.SubmitChanges();

               return true;
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               return false;
           }

       }

   

       /// <summary>
       /// Updates All addreses of current organization.
       /// </summary>
       /// <param name="OrgAddresses"></param>
       /// <returns></returns>
       public bool UpdateOrgAdress(IEnumerable<Address> OrgAddresses)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {

               db.Addresses.AttachAll(OrgAddresses);
               db.Refresh(RefreshMode.KeepCurrentValues, OrgAddresses);

              
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }

           return result;

       }

       #endregion 

       #region Contact Update Delete Insert
       public bool InsertNewOrgContact(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               Contact cont = new Contact();
               cont.OrgID = id;
               db.Contacts.InsertOnSubmit(cont);
               db.SubmitChanges();

               return true;
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               return false;
           }
       }


       public bool DeleteOrgContact(int id)
       {
           bool result = true;
           try
           {
               BOTADataContext db = new BOTADataContext(connectString);

               //using (BOTADataContext db)
               {
                   Contact item = (from conts in db.Contacts
                                   where conts.ContactID== id
                                   select conts).First();
                   if (item != null)
                   {
                       db.Contacts.DeleteOnSubmit(item);
                       db.SubmitChanges();
                   }


               }
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }


           return result;
       }



       #endregion 






       public int CreateNewOrganization(string projorganization)
       {
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
               Organization org = new Organization();
               db.Organizations.InsertOnSubmit(org);
               db.SubmitChanges();

               General gen = new General();
               gen.OrgID = org.OrgID;
               gen.NameRu = projorganization;
               db.Generals.InsertOnSubmit(gen);
               db.SubmitChanges();
               
                return org.OrgID;
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               return -1; 
           }
           
            
       }

       public IEnumerable<Project> GetAssociatedGrants(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           IEnumerable<Project> result;
         
           result = from proj in db.Projects
                    join projinfo in db.ProjectInfos
                    on proj.ProjectID equals projinfo.ProjectInfoID
                    join projproposal in db.ProposalInfos
                    on proj.ProjectID equals projproposal.ProjectID
                    where proj.OrgID == id
                    select proj;

           return result;
          
       }


       public Organization GetOrganizationOtherFunderByID(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           Organization result2;

           result2 = (from org in db.Organizations
                      join funder in db.OtherFunders
                      on org.OrgID equals funder.OrgID
                      where org.OrgID == id
                      select org).FirstOrDefault();

           return result2;
       }


       public IEnumerable<Address> GetOrganizationAddressByID(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

         

           try
           {
           /*    result = (from org in db.Organizations
                         join cont in db.Addresses
                         on org.OrgID equals cont.OrgID
                         where org.OrgID == id
                         select org).FirstOrDefault();
               return result; */
            var result = from addr in db.Addresses
                        where addr.OrgID == id
                        select addr;

            return result; 

           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               return null; 
           }

       }

       /// <summary>
       /// Get the Organization by ID with Contact Loaded
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public Organization GetOrganizationContactByID(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           Organization result;

           result = (from org in db.Organizations
                     join cont in db.Contacts
                     on org.OrgID equals cont.OrgID
                     where org.OrgID == id
                     select org).FirstOrDefault();

           return result;

       }


       public Organization getOrganizationByID(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           Organization result;

           result = (from org in db.Organizations                  
                     where org.OrgID == id
                     select org).FirstOrDefault();

           return result;
       }


       public Organization GetOrganizationGeneralByID(int id)
       {
           BOTADataContext db = new BOTADataContext(connectString);

           Organization result;

           result = (from org in db.Organizations
                     join gen in db.Generals
                     on org.OrgID equals gen.OrgID
                     where org.OrgID == id
                     select org).FirstOrDefault();

           return result;
       
       }

       
       public IEnumerable<Organization> SearchOrganizationByName(string Name)
       {
           
           BOTADataContext db = new BOTADataContext(connectString);

           IEnumerable<Organization> result;    
           result =  from org in db.Organizations
                     join gen in db.Generals 
                     on org.OrgID equals gen.OrgID
                            where gen.NameRu.Contains(Name)
                            select org;
                      
               return result;
       }


       public IEnumerable<LegalStatusList> GetLegalStatusList()
       {
          // BOTADBEntities1 db = new BOTADBEntities1();
           //Settings.Default.FisharooConnectionStringLocal
           
           BOTADataContext db = new BOTADataContext(connectString); 

           var result = from ls in db.LegalStatusLists
                        select ls;
           return result;

           //public IEnumerable<SelectListItem> StatusSelectList {get.....
           
       }

       public bool DeleteOrganization(int ProposalID)
       {
            /*
           // Get movie to delete
           var movieToDelete = _db.MovieSet.First(m => m.Id == id);

           // Delete 
           _db.DeleteObject(movieToDelete);
           _db.SaveChanges();

           // Show Index view
           return RedirectToAction("Index");  */



           bool result = true;
           try
           {
               BOTADataContext db = new BOTADataContext(connectString);

               //using (BOTADataContext db)
               {
                  Organization item = (from projects in db.Projects
                                        join organization in db.Organizations
                                        on projects.Organization.OrgID equals organization.OrgID
                                        where projects.ProjectID == ProposalID
                                        select organization).First();
                  db.Organizations.DeleteOnSubmit(item);
                  db.SubmitChanges();
                  

               }
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }


           return result;  
       }






       
       public Organization GetOrganizationContactOfCurrentProposal(int ProposalID)
       {
           Organization organization1 = null;
           BOTADataContext db = new BOTADataContext(connectString);
           {
               organization1 = (from projects in db.Projects
                                join organization in db.Organizations //.Include("General").Include("LegalStatus")
                                on projects.Organization.OrgID equals organization.OrgID
                                where projects.ProjectID == ProposalID
                                select organization).First();

           }

           return organization1;

       }


       /// <summary>
       /// Update Organization All Contacts.
       /// </summary>
       /// <param name="Contacts"></param>
       /// <returns></returns>
       public bool UpdateOrganizationContact(IEnumerable<Contact> Contacts)
       {
           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {

               db.Contacts.AttachAll(Contacts);
               db.Refresh(RefreshMode.KeepCurrentValues, Contacts);

               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }

           return result;
       }
       
          


       public General GetOrganizationGeneralOfCurrentProposal(int ProposalID)
       {
           General general1 = null;
           BOTADataContext db = new BOTADataContext(connectString);
           {
                 general1 = (from projects in db.Projects
                                join organization in db.Organizations //.Include("General").Include("LegalStatus")
                                on projects.Organization.OrgID equals organization.OrgID
                                join general in db.Generals
                                on organization.OrgID equals general.OrgID
                                where projects.ProjectID == ProposalID
                                select general).First();                           

           }

           return general1;

       }

       /// <summary>
       /// Updates Organization General. 
       /// LegalStatus: If exists then update, if no then insert. 
       /// </summary>
       /// <param name="gen"></param>
       /// <returns></returns>
       public bool UpdateOrganizationGeneral(General gen)
       {

           bool result = true;
           BOTADataContext db = new BOTADataContext(connectString);
           try
           {
             
               db.Generals.Attach(gen);
               db.Refresh(RefreshMode.KeepCurrentValues, gen);
             
              //db.ExecuteCommand("DELETE FROM addresses WHERE ContactID={0} and AddressTypeID={1}", contactID, addressTypeID);
              // gen.Organization.LegalStatus.Detach(); 

               //check if legalstaus exists.

              /* var legstat = (from l in db.LegalStatus
                              where l.OrgID == gen.OrgID
                              select l).FirstOrDefault(); */

              
               //if (legstat == null) //if it does not exist.
               if (!db.LegalStatus.Any(l => l.OrgID == gen.OrgID))  // !if exists. 
               { 
                  //Insert 
                   LegalStatus lstat = new LegalStatus();
                   lstat.OrgID = gen.OrgID;
                   lstat.LegSListID = gen.Organization.LegalStatus.LegSListID;
                   lstat.SelectedDate = gen.Organization.LegalStatus.SelectedDate;
                   db.LegalStatus.InsertOnSubmit(lstat);
                   
               }
               else
               {   //try to update. 
                  // legstat.Detach(); 
                  // legstat.LegSListID = gen.Organization.LegalStatus.LegSListID; 
                  // db.LegalStatus.Attach(legstat);
                   db.Refresh(RefreshMode.KeepCurrentValues, gen.Organization.LegalStatus);
               }
            
               db.SubmitChanges();
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }

           return result;

/*     
           try
           {

               BOTADataContext db = new BOTADataContext(connectString);

               { General genitem = (from organization in db.Organizations
                                      join general in db.Generals
                                      on organization.OrgID equals general.OrgID
                                      where general.OrgID == gen.Organization.OrgID
                                      select general).First();

                   genitem.Name = gen.Name;
                   genitem.Organization.LegalStatus.LegSListID = gen.Organization.LegalStatus.LegSListID;
                   genitem.FiscalYearEnd = gen.FiscalYearEnd;
                   genitem.Notes = gen.Notes;
                   db.SubmitChanges();

               }
           }
           catch (Exception ex)
           {
               result = false;
           }

           return result; */
       }



       public bool UpdateOrganization(Organization org, int ProposalID)
       {
           bool result = true;
           try
           {

               BOTADataContext db = new BOTADataContext(connectString);

               {

                   Organization item = (from projects in db.Projects
                                        join organization in db.Organizations //.Include("General")
                                        on projects.Organization.OrgID equals organization.OrgID
                                        where projects.ProjectID == ProposalID 
                                        select organization).First();

                    //join legstatus in db.LegalStatus
                      //                  on projects.Organization.OrgID equals legstatus.OrgID
               
               
                   //General Info Update.  
                   item.General.Name = org.General.Name;
                   item.LegalStatus.LegSListID = org.LegalStatus.LegSListID;
                   item.General.FiscalYearEnd = org.General.FiscalYearEnd;
                   item.General.Notes = org.General.Notes;
                   item.Contacts = org.Contacts;
                                                

                 
                   db.SubmitChanges();

               }
           }
           catch (Exception ex)
           {
               Log.EnsureInitialized();
               Log.Error(typeof(OrganizationRepository), "----------------------------------------------", ex);
               result = false;
           }


           return result; 
       }
       
       
       public Organization GetOrganizationOfCurrentProposal(int ProposalID)
       {


           Organization organization1 = null;
           BOTADataContext db = new BOTADataContext(connectString);
            {
                /*   from p in db.Purchases
where p.Customer.Address.State == "WA"
where p.PurchaseItems.Sum (pi => pi.SaleAmount) > 1000
select p */
        
         /*
         IQueryable<Organization> newSalesPeople = db.CreateQuery<Organization>(
         "SELECT VALUE sp " +
         "FROM AdventureWorks.AdventureWorksDB.SalesPeople AS sp " +
         "WHERE sp.HireDate > @date", new System.Data.Objects.ObjectParameter("@date", hireDate));
        

                foreach (SalesPerson p in newSalesPeople)
                {
                    Console.WriteLine("{0}\t{1}", p.FirstName, p.LastName);
                }

         */
               
                organization1 = (from projects in db.Projects
                                 join organization in db.Organizations //.Include("General").Include("LegalStatus")
                                 on projects.Organization.OrgID equals organization.OrgID 
                                 where projects.ProjectID == ProposalID
                                 select organization).First();
                         
                     
                         //Load All of Oganization 
                     //    organization1.Addresses.Load();
                     //    organization1.Contacts.Load();
                        // organization1.LegalStatus.LegalStatusList.LegStatListID 
                       //  organization1.LegalStatusReferences.Load(); 
                      //   organization1.OrganizationStaffs.Load();
                     //    organization1.OtherFunders.Load();         //for many results.
                       
                       //  organization1.GeneralReferences.Load();      //if we have one result.    


            }

           
           return organization1;

       

     
       }
    }
}
