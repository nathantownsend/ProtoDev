using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// The user's employment
        /// </summary>
        [Required]
        [Display(Name = "Company Type")]
        public string EmploymentType
        {
            get
            {
                _employmentType = Registration.RegistrationDescription;

                string[] items = new string[] { "Mining", "Consultant", "Federal", "State" };
                if (items.Contains(_employmentType))
                    return _employmentType;
                else
                    return "Other";
            }
            set
            {
                _employmentType = value;
            }
        }
        string _employmentType;


        /// <summary>
        /// Droppdown list items for the user's employement
        /// </summary>
        public IEnumerable<SelectListItem> EmploymentTypeItmes
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem() { Value = "Mining", Text = "A Mining Company" });
                items.Add(new SelectListItem() { Value = "Consultant", Text = "A Consultant" });
                items.Add(new SelectListItem() { Value = "Federal", Text = "Federal Government" });
                items.Add(new SelectListItem() { Value = "State", Text = "State Government" });
                items.Add(new SelectListItem() { Value = "Other", Text = "Other" });

                return items;
            }
        }

    }
}