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


            // allow any authenticated user to get to the register page 
            string path = Request.Path.ToString();
            if (path.ToLower().StartsWith("/account/register") || path.ToLower().StartsWith("/account/terms"))
                return;


            // get the current user
            myCoalUser user = myCoalUser.GetInstance();
            
            // if the user isn't registered yet direct them to the registration page
            if (!user.IsRegistered)
                Response.Redirect("~/account/register");

            

        }

    }
}