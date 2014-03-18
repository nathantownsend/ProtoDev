using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ePermitBLL;
using ePermitDAL.DO.dbo;
using ePermitDAL.DAL.dbo;
using DEQMYCOAL.web.Models;
using System.Security.Claims;

namespace DEQMYCOAL.web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }


        /// <summary>
        /// Occurs after the user has been authenticated 
        /// </summary>
        protected void Application_AuthorizeRequest()
        {

            // if token is null, then somehting went bad with ePass
            if (string.IsNullOrEmpty(myCoalUser.UserToken))
                throw new UnauthorizedAccessException("The system could not retrieve your ePass name identifier. Please contact the system admin.");

            

            // allow any authenticated user to get to any of the account setup informational pages
            string[] allowedPaths = new string[] { "/account/register", "/account/registrationreceived", "/account/denied", "/account/inactive", "/account/pending", "/account/unknown" };
            string path = Request.Path.ToString().ToLower();
            string[] parts = path.Split('/');

            if (parts.Length >= 3)
            {
                string start = string.Join("/", parts.Take(3));
                if (allowedPaths.Contains(start))
                    return;
            }


            /*
            bool allowed = false;
            foreach (string p in allowedPaths)
            {
                if (path.StartsWith(p))
                    allowed = true;
            }

            // if allowed stop processing the request
            if (allowed)
                return;
            */



            // get the current user
            myCoalUser user = myCoalUser.GetInstance();
            
            // if the user isn't registered yet direct them to the registration page
            if (!user.IsRegistered)
                Response.Redirect("~/account/register");

            switch (user.Status)
            {
                case myCoalUserStatus.Denied:
                    Response.Redirect("~/account/denied");
                    break;
                case myCoalUserStatus.Inactive:
                    Response.Redirect("~/account/inactive");
                    break;
                case myCoalUserStatus.Pending:
                    Response.Redirect("~/account/pending");
                    break;
                case myCoalUserStatus.Unknown:
                    Response.Redirect("~/account/unknown");
                    break;

            }

            

        }

    }
}