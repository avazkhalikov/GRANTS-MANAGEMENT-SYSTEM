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

    public class AccountTestRepository : IAccountRepository
    {
        // private IConfiguration _configuration;
        private Account account;
        private Role role; 

        public AccountTestRepository()
        {
            //  conn = new Connection();
            //  _configuration = ObjectFactory.GetInstance<IConfiguration>();

            account = new Account();
            
            //Later we can add more like SqlFactory.Oracle(); Which will still have the same sqlcmd.GetUserByName(Username) methods.
            //How? StrucuteMapBootStrapper will switch the ISqlCommands to another Implementer!
        }

        //  http://www.simplyvinay.com/Post/51/MPBlog-Implementation.-Part-5.aspx

        ////Now DO DB UPDATE/DELETE/INSERT HERE!

        public Account GetAccountByUsername(string Username)
        {
            //TODO: Data Comes from DB!

                        account.AccountID = 1;
                        account.Username = "akhalikov2";
                        account.FirstName = "Avaz3";
                        account.LastName = "Khalikov2";
                        account.Password = "123";

                        role = new Role();  
                        role.RoleName = "Grants Manager"; 
                        account.AddRole(role);

                        role = new Role(); 
                        role.RoleName = "Program Officer";
                        account.AddRole(role);

                       
                return account;
           


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
