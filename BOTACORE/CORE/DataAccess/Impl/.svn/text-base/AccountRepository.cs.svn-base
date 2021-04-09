using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap; 
using BOTACORE.CORE;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.DAL;
using System.Data;


namespace BOTACORE.CORE.DataAccess.Impl
{

 /// <summary>
 /// This is the class where we get data from Database and Map it into Objects.
 /// </summary>
 /// 
     
  public  class AccountRepository: IAccountRepository
    {
     // private IConfiguration _configuration;
      private Account account;
      private ISqlCommands sqlcmd;
      
      public AccountRepository()
      {
        //  conn = new Connection();
        //  _configuration = ObjectFactory.GetInstance<IConfiguration>();

          account = new Account();
          sqlcmd = SqlFactory.MSSQL();  

          //Later we can add more like SqlFactory.Oracle(); Which will still have the same sqlcmd.GetUserByName(Username) methods.
          //How? StrucuteMapBootStrapper will switch the ISqlCommands to another Implementer!
      }

    //  http://www.simplyvinay.com/Post/51/MPBlog-Implementation.-Part-5.aspx

      ////Now DO DB UPDATE/DELETE/INSERT HERE!

      public Account GetAccountByUsername(string Username)
      {
          //TODO: Data Comes from DB!

         bool error = false; 
         IDataReader dr =  sqlcmd.GetUserByName(Username); 
         
           try
           {
               using (dr)
               {
                   while (dr.Read())
                   {
                     account.AccountID = Convert.ToInt32(dr["AccountID"]); 
                     account.Username =  dr["Username"].ToString(); 
                     account.FirstName = dr["FirstName"].ToString(); 
                     account.LastName =  dr["LastName"].ToString();
                     account.Password = dr["Password"].ToString();
                     //TODO: For now User in Groups 1 and 2.
                     
                   } 
               }
           }
           catch {
               error = true;
           }
           finally
           {
               dr.Close();
           }

           if (error == true)
           {
               return null;
           }
           else
           {
               return account;
           }


          //TEST
          /*
          if (Username == "akhalikov")
          {
              account.AccountID = 1;
              account.Username = "akhalikov";
              account.FirstName = "Avaz";
              account.LastName = "Khalikov";
          }

          return account;  */
      }

     

   
    }
}
