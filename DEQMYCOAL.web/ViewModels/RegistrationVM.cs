using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ePermitDAL.DAL.dbo;
using ePermitDAL.DO.dbo;

namespace DEQMYCOAL.web.ViewModels
{
    public class RegistrationVM
    {
        /// <summary>
        /// The registration data object
        /// </summary>
        public RegistrationDO Registration { get; set; }


        /// <summary>
        /// Dropdown list items with the current registration value selected
        /// </summary>
        public IEnumerable<SelectListItem> States
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();

                // add the default value
                items.Add(new SelectListItem() { Text = "", Value = "", Selected = String.IsNullOrEmpty(Registration.StateID) });

                foreach (StateDO state in State.GetAll())
                    items.Add(new SelectListItem() { Text = state.Description, Value = state.StateID, Selected = (state.StateID == Registration.StateID) });

                return items;
            }
        }


        /// <summary>
        /// Dropdown list items with the current registration value selected
        /// </summary>
        public IEnumerable<SelectListItem> AccessRoles
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();

                // add the default value
                items.Add(new SelectListItem() { Text = "", Value = "", Selected = String.IsNullOrEmpty(Registration.AccessRoleID) });

                foreach (AccessRoleDO role in AccessRole.GetAll())
                    items.Add(new SelectListItem() { Text = role.AccessRoleID, Value = role.AccessRoleID, Selected = (role.AccessRoleID == Registration.AccessRoleID) });

                return items;
            }
        }


    }
}