using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DEQMYCOAL.web.Models
{
    public class myCoalPrincipal: IPrincipal
    {
        IIdentity _identity;
        myCoalUser _profile;

        public myCoalPrincipal(myCoalUser profile, IIdentity federatedIdentity)
        {
            _identity = federatedIdentity;
            _profile = profile;
        }

        public IIdentity Identity
        {
            get 
            {
                return _identity;
            }
        }

        public bool IsInRole(string role)
        {
            // remove commas
            if (role.Contains(','))
                role = role.Replace(",", "");

            // if role contains multiple roles check each
            if (role.Contains(' '))
            {
                string[] roles = role.Split(' ');

                foreach (string r in roles)
                {
                    if (_profile.Roles.Contains(r))
                        return true;
                }
            }

            // is the role specified contained in the roles provided
            return _profile.Roles.Contains(role);
        }

        public string FullName { get { return _profile.FullName; } }
    }
}