using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BOTACORE.CORE
{
   public class AccessLevelConvert
    {
       public static int FromEnumToInt(AccessLevel accessLevel)
       {
           switch (accessLevel)
           {
               case AccessLevel.Full:
                   return 2; //A single instance will be created for each HttpContext.  Caches the instances in the HttpContext.Items collection if it exists, otherwise uses ThreadLocal storage.
               case AccessLevel.Read:
                   return 1; //A new instance will be created for each request.
               case AccessLevel.None:
                   return 0; //A single instance will be shared across all requests
               default:
                   throw new NotImplementedException("{0} not implemented ");
           } 
       }

       public static AccessLevel FromIntToEnum(int accessLevel)
       {
           
           switch (accessLevel)
           {
               case 2:
                   return AccessLevel.Full; //A single instance will be created for each HttpContext.  Caches the instances in the HttpContext.Items collection if it exists, otherwise uses ThreadLocal storage.
               case 1: 
                   return AccessLevel.Read; //A new instance will be created for each request.
               case 0:
                   return AccessLevel.None; //A single instance will be shared across all requests
               default:
                   throw new NotImplementedException("{0} not implemented ");
           }  
       }
    }
}
