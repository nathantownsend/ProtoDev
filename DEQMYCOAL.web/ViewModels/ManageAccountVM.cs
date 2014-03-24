using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEQMYCOAL.web.Models;
using ePermitBLL;

namespace DEQMYCOAL.web.ViewModels
{
    public class ManageAccountVM
    {

        /// <summary>
        /// The current logged in user
        /// </summary>
        public myCoalUser CurrentUser { get { return myCoalUser.GetInstance(); } }


        /// <summary>
        /// The user details of the current logged in user
        /// </summary>
        public RegistrationVM UserDetails
        {
            get
            {
                RegistrationVM vm = new RegistrationVM()
                {
                    Registration = RegistrationBLL.GetRegistration((int)CurrentUser.RegistrationId)
                };

                return vm;
            }
        }

    }
}