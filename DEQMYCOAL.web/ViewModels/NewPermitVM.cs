using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DEQMYCOAL.web.ViewModels
{
    /// <summary>
    /// This class is used on the form for creating new permits
    /// </summary>
    public class NewPermitVM
    {
        /// <summary>
        /// The site name is required to create a new permit
        /// </summary>
        [Required]
        [Display(Name="Site Name")]
        [MaxLength(75)]
        public string SiteName { get; set; }
    }
}