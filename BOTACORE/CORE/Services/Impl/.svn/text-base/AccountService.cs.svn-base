using System;
using System.Collections.Generic;
using System.Text;
using BOTACORE.CORE.DataAccess;
using StructureMap;
using BOTACORE.CORE.Domain;
using BOTACORE.CORE.DataAccess.Impl;

namespace BOTACORE.CORE.Services.Impl
{
    /// <summary>
    /// @Author Avaz
    /// </summary>
   
    public class AccountService : IAccountService
    {
       private IAccountRepository _accountRepository;
       private IWebContext _webContext;
       private IUserSession _userSession;
       private IRedirector _redirector; 

       public AccountService()
       {
          
           //This way AccountService can use any accountRepository(does not depend on one Repository)!
           //It gets Repository from RepositoryFactory! So he has someone(factory) to ask for any Repository.
           //Currently our RepositoryFactory is static and directly depends on StructureMap IOC.
           
           _accountRepository = RepositoryFactory.AccountRepository();
           _webContext = ServiceFactory.WebContext();
           _userSession = ServiceFactory.UserSession();
           _redirector = ServiceFactory.Redirector(); 
       }


         
       public string Login(string Username, string Password)
       {

           Account account = _accountRepository.GetAccountByUsername(Username);

           if (account != null)
           {
               //password matches
               if (account.Password == Password)
               {
              //     _userSession.CurrentUser = account;  //Account changed to SSPSTAFF.
                   _userSession.LoggedIn = true;
                   _redirector.GoToDefault(_userSession.CurrentUser.SSPStaffID); 
                    
               }

               return "Password did not match"; 

           }

       
           return "Such account does not exist";
                    
       }

       public void Logout()
       {
           _userSession.LoggedIn = false;
           _userSession.CurrentUser = null;
       }
        

       public string GetAdminURL()
       {
           return null; // _accountRepository.getAdminUrl();
       }
    }
}
