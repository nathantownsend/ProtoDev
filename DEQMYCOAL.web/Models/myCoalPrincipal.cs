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
        myCoalUser _coalUser;

        public myCoalPrincipal(myCoalUser coalUser, IIdentity federatedIdentity)
        {
            _identity = federatedIdentity;
            _coalUser = coalUser;
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
                    if (_coalUser.Roles.Contains(r))
                        return true;
                }
            }

            // is the role specified contained in the roles provided
            return _coalUser.Roles.Contains(role);
        }

        public string FullName { get { return _coalUser.FullName; } }

        public myCoalUser CoalUser { get { return _coalUser; } }
    }
}