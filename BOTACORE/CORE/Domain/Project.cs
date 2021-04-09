using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Resources;


namespace BOTACORE.CORE.Domain
{
    public partial class Project
    {
        public void Detach()
        {  //http://geekswithblogs.net/michelotti/archive/2007/12/25/117984.aspx
            this.PropertyChanged = null; this.PropertyChanging = null;
           
          //  this = default(EntitySet<Project>); 

          //  this.BankInfo = default(EntityRef<BankInfo>);

            //this._State = default(EntityRef<State>);

           // this.Detach();              
            //foreach (Address address in this.Addresses)
            // {
            //     address.Detach();
            // }
         }
    }
}