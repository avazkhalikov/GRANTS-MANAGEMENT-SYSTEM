﻿using System;
using BOTACORE.CORE.Domain;
namespace BOTACORE.CORE.Services
{
   public interface IUserSession
    {
        SSPStaff CurrentUser { get; set; }
        bool LoggedIn { get; set; }
        int ProjectID { get; set; }
        int OrgID { get; set; }
        string ProjectLabel{ get; set; }
        string OrganizationName { get; set; }
        
    }
}
