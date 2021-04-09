using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOTACORE.CORE.Domain
{
   public class ViewStaffProject
    {
        public int? sspid = 0;
        public int? projid = 0;
        public int? roleId = 0;
        public bool? Active = false;
        public string RoleName;
        public string FirstName;
        public string LastName;
    }

   //№	Grant #	Organization name	Grant type	Area	Round
   public class ViewStaffMyProject
   {
       public int? sspid = 0;
       public int? projid = 0;
       public string projectLabel; 
       public string OrgName;
       public string GrantType;
       public string Area;
       public string Round;
       public string Status;
       public GrantType grantType=null;
       public ProgramArea programArea=null;
       public string FirstName;
       public string LastName;
   }

    public class StaffGrantHolder
    {
        public int? sspid = 0;
        public IEnumerable<GrantType> grantType;
        public IEnumerable<ProgramArea> ProgramArea;
        public string FirstName;
        public string LastName;
    }

    public class StaffGrantTypeGrouped
    {
        public int? sspid = 0;
        public Dictionary<int, int> grantTypeCount;
        public string FirstName;
        public string LastName;
    }

    //...
    //Public class ViewStaffProjReport

}
