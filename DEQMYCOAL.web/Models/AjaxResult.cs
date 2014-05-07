using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEQMYCOAL.web.Models
{
    /// <summary>
    /// Used to return data to the client after an ajax request
    /// </summary>
    public class AjaxResult
    {
        AjaxStatus _status;

        public AjaxResult(AjaxStatus status, string message)
        {
            Message = message;
            _status = status;
            Data = new Dictionary<string, object>();
        }

        /// <summary>
        /// The message returned
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The status of the request
        /// </summary>
        public string Status { get { return _status.ToString(); } }

        /// <summary>
        /// Additional data to return with the result
        /// </summary>
        public Dictionary<string, object> Data { get; set; }

        // remember to keep in sync the javascript version of this enum 
        public enum AjaxStatus
        {
            OK,
            ERROR,
            UNEXPECTED
        }
    }
}