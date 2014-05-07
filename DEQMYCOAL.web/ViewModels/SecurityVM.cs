using DEQMYCOAL.web.Models;
using ePermitBLL;
using ePermitDAL.DO.dbo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEQMYCOAL.web.ViewModels
{
    public class SecurityVM
    {
        public SecurityVM()
        {
            Registration = new RegistrationVM();
        }

        public SecurityVM(UserProfileBO profile)
        {
            Registration = new RegistrationVM() { Registration = profile.Registration };
            myPermitRegistrations = profile.PermitRegistration;


            myCoalUser user = myCoalUser.GetInstance();

            // owners and permit coordinators can see a user's permit registrations
            if (user.IsInRole(CoalRoles.Owner) || user.IsInRole(CoalRoles.PermitCoordinator))
            {
                AllPermits = PermitBLL.GetPermits();
            }

            // owners can see user's system roles
            if (user.IsInRole(CoalRoles.Owner))
            {
                IsOwner = profile.Roles.Contains(CoalRoles.Owner);
                IsManagement = profile.Roles.Contains(CoalRoles.Management);
                IsReviewStaff = profile.Roles.Contains(CoalRoles.ReviewStaff);
                IsPermitCoordinator = profile.Roles.Contains(CoalRoles.PermitCoordinator);
                IsReadOnly = profile.Roles.Contains(CoalRoles.Reader);
            }
        }

        public RegistrationVM Registration { get; set; }

        public IEnumerable<PermitRegistrationBO> myPermitRegistrations { get; set; }

        public IEnumerable<PermitDO> AllPermits { get; set; }

        #region Role Properties

        /// <summary>
        /// If the user is an owner
        /// </summary>
        [Display(Name="Owner")]
        public bool IsOwner { get; set; }

        /// <summary>
        /// If the user is management
        /// </summary>
        [Display(Name = "Management")]
        public bool IsManagement { get; set; }

        /// <summary>
        /// if the user is review staff
        /// </summary>
        [Display(Name = "Review Staff")]
        public bool IsReviewStaff { get; set; }

        // if the user is a permit coordinator
        [Display(Name = "Permit Coordinator")]
        public bool IsPermitCoordinator { get; set; }

        /// <summary>
        /// if the user is readonly
        /// </summary>
        [Display(Name = "Read Only")]
        public bool IsReadOnly { get; set; }


        /// <summary>
        /// Gets a space separated list of all the roles
        /// </summary>
        public string Roles
        {
            get
            {
                List<string> roles = new List<string>();

                if (IsOwner)
                    roles.Add(CoalRoles.Owner);
                if (IsManagement)
                    roles.Add(CoalRoles.Management);
                if (IsReviewStaff)
                    roles.Add(CoalRoles.ReviewStaff);
                if (IsPermitCoordinator)
                    roles.Add(CoalRoles.PermitCoordinator);
                if (IsReadOnly)
                    roles.Add(CoalRoles.Reader);

                return string.Join(" ", roles);
            }
        }

        #endregion

        /// <summary>
        /// Gets the registration statuses available for the dropdown
        /// </summary>
        public IEnumerable<SelectListItem> RegistrationStatuses
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();

                foreach (RegistrationStatusDO obj in RegistrationBLL.GetRegistrationStatuses())
                {
                    string status = obj.RegistrationStatusID;
                    SelectListItem item = new SelectListItem() { Text = status, Value = status, Selected = Registration.Registration.RegistrationStatusID == status };
                    items.Add(item);
                }

                return items;
            }
        }
        

    }
}