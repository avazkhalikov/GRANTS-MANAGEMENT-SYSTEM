using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOTACORE.CORE.Domain
{
   public partial class Permission
   {
       

       private string _Name;
       /*
       private int _PermissionID;
       public int PermissionID
       {
           get
           {
               return this._PermissionID;
           }
           set
           {
               if ((this._PermissionID != value))
               {

                   this._PermissionID = value;

               }
           }
       }
       */

       public string Name
       {
           get
           {
               return this._Name;
           }
           set
           {
               if ((this._Name != value))
               {

                   this._Name = value;

               }
           }
       }

   }
}
