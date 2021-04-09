using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOTACORE.CORE.Domain
{
    public class BaseEntity
    {

        private bool _Inflated;
        public bool Inflated
        {
            get
            {
                return this._Inflated;
            }
            set
            {

                this._Inflated = value;

            }
        }

        //Permissions -------------------------------------------------
       
        private Dictionary<Role, AccessLevel> _permissions =
                            new Dictionary<Role, AccessLevel>();

        //Role - String, AccessLevel - Enum

        public Dictionary<Role, AccessLevel> Permissions
        {
            get { return _permissions; }
        }


        public void AddPermission(Role _role, AccessLevel _accessLevel)
        {
            _permissions.Add(_role, _accessLevel);
        }

        public bool HasPermission(Account acct, AccessLevel _accessLevel)
        {
            foreach (Role r in acct.Roles)
            {
                foreach (KeyValuePair<Role, AccessLevel> permission in Permissions)
                {
                    if (permission.Key.RoleName == r.RoleName && permission.Value == _accessLevel)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        
        //-----------------------------------------------------------------
    }
}
