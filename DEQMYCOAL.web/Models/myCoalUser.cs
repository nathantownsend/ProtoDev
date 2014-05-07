using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using ePermitBLL;

namespace DEQMYCOAL.web.Models
{
    /// <summary>
    /// Statuses a user may be
    /// </summary>
    public enum myCoalUserStatus
    {
        Active,
        Denied,
        Inactive,
        Pending,
        Unknown
    }

    public class myCoalUser
    {
        const string COOKIE_NAME = "myCoalMontana";

        /// <summary>
        /// Creates a new instance of myCoalUser and tracks the values using a cookie
        /// </summary>
        /// <param name="UserIdentity"></param>
        /// <param name="Profile"></param>
        private myCoalUser(UserProfileBO Profile)
        {
            Roles = Profile.Roles.Split(' ');
            FirstName = Profile.Registration.FirstName;
            LastName = Profile.Registration.LastName;
            RegistrationId = Profile.Registration.RegistrationID;
            Email = Profile.Registration.Email;
            IsRegistered = true;
            Status = (myCoalUserStatus)Enum.Parse(typeof(myCoalUserStatus), Profile.Registration.RegistrationStatusID);

            AddProfileCookie();
        }


        /// <summary>
        /// Update user properties from a cookie
        /// </summary>
        /// <param name="userCookie"></param>
        private myCoalUser(HttpCookie userCookie)
        {
            IsRegistered = true;
            FirstName = userCookie.Values["FirstName"];
            LastName = userCookie.Values["LastName"];
            LastName = userCookie.Values["LastName"];
            Email = userCookie.Values["Email"];
            Roles = userCookie.Values["Roles"].Split(' ');
            RegistrationId = Convert.ToInt32(userCookie.Values["RegistrationId"]);
            Status = (myCoalUserStatus)Enum.Parse(typeof(myCoalUserStatus), userCookie.Values["RegistrationStatus"]);
            
        }


        /// <summary>
        /// Creates a new default instance of myCoalUser
        /// </summary>
        /// <param name="UserIdentity"></param>
        private myCoalUser()
        {
            Roles = new string[] { };
            FirstName = "";
            LastName = "";
            RegistrationId = 0;
            IsRegistered = false;
            Status = myCoalUserStatus.Unknown;
        }


        /// <summary>
        /// Loads a myCoalUser from cookie or returns a default instance if no cookie exists
        /// </summary>
        /// <returns></returns>
        public static myCoalUser GetInstance()
        {
            
            HttpCookie userCookie = HttpContext.Current.Request.Cookies[COOKIE_NAME];

            // if no cookie try getting the user from database
            if (userCookie == null)
            {

                // if the user is not registered return a default instance
                if (!RegistrationBLL.IsUserRegistered(myCoalUser.UserToken))
                    return new myCoalUser();

                // if the user was found then return the user
                UserProfileBO profile = RegistrationBLL.GetUserProfile(myCoalUser.UserToken);
                
                return new myCoalUser(profile);
            }

            // return a user from the values stored in the cookie
            return new myCoalUser(userCookie);
        }


        /// <summary>
        /// Gets the ePass token from the current request
        /// </summary>
        public static string UserToken
        {
            get
            {
                ClaimsIdentity id = (ClaimsIdentity)HttpContext.Current.User.Identity;
                Claim claim = id.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
                if (claim != null)
                    return claim.Value;
                else
                    return null;
            }
        }


        /// <summary>
        /// Logs the current user out of the system
        /// </summary>
        public static void RemoveCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIE_NAME];
            
            if (cookie != null)
            {
                cookie = new HttpCookie(COOKIE_NAME);
                cookie.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// A cookie storing the user profile values
        /// </summary>
        /// <returns></returns>
        void AddProfileCookie()
        {
            HttpCookie cookie = new HttpCookie(COOKIE_NAME);
            cookie.Values.Add("FirstName", FirstName);
            cookie.Values.Add("LastName", LastName);
            cookie.Values.Add("Email", Email);
            cookie.Values.Add("Roles", string.Join(" ", Roles));
            cookie.Values.Add("RegistrationId", RegistrationId.ToString());
            cookie.Values.Add("RegistrationStatus", Status.ToString());
            cookie.Values.Add("Token", UserToken);
            cookie.Expires = DateTime.Now.AddMinutes(5d);
            cookie.Secure = true;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Resets the values in the profile cookie
        /// </summary>
        public static void ResetProfileCookie()
        {
            HttpContext.Current.Request.Cookies.Remove(COOKIE_NAME);
            GetInstance();
        }

        /// <summary>
        /// If the user is registered with the system
        /// </summary>
        public bool IsRegistered { get; set; }
        
        /// <summary>
        /// If the user is in the role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }

        /// <summary>
        /// The registration id of the user
        /// </summary>
        public int RegistrationId { get; set; }

        /// <summary>
        /// The space delimited list of user roles
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The user's first and last name
        /// </summary>
        public string FullName { get { return string.Concat(FirstName, " ", LastName); } }

        public string Email { get; set; }

        /// <summary>
        /// The registration status of the user
        /// </summary>
        public myCoalUserStatus Status { get; set; }


    }
}