using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOTACORE.CORE.Domain
{
   [Serializable]  //in case we use StateServer.
    public class Account   //Persistent Class
    {
        private int _AccountID;
       
        private string _FirstName;

        private string _LastName;

        private string _Email;

        private string _Username;

        private string _Password;

        private System.Nullable<System.DateTime> _CreateDate;

        //Roles  -----------------------------------
        private List<Role> _roles = new List<Role>();
        public List<Role> Roles
        {
            get { return _roles; }
        }

        public void AddRole(Role role)
        {
            _roles.Add(role);
        }

        public bool HasRole(string Name)
        {
            foreach (Role r in _roles)
            {
                if (r.RoleName == Name)
                    return true;
            }
            return false;
        }
       //--------------------------------
              
       
       public int AccountID
        {
            get
            {
                return this._AccountID;
            }
            set
            {
                if ((this._AccountID != value))
                {
                    this._AccountID = value;
                 
                }
            }
        }


      


        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                if ((this._FirstName != value))
                {
                 
                    this._FirstName = value;
                    
                }
            }
        }

        
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                if ((this._LastName != value))
                {
                 
                    this._LastName = value;
          
                }
            }
        }


        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                if ((this._Email != value))
                {
                    this._Email = value;
                    
                }
            }
        }


        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                if ((this._Username != value))
                {
                    
                    this._Username = value;
                    
                }
            }
        }

       
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                if ((this._Password != value))
                {
                    
                    this._Password = value;
                    
                }
            }
        }


        public System.Nullable<System.DateTime> CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                if ((this._CreateDate != value))
                {
            
                    this._CreateDate = value;
                    
                }
            }
        }


      
    }


   

}
