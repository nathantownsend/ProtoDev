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
using log4net;
using System.Threading;

namespace DEQMYCOAL.web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801



    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog logger = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            log4net.Config.XmlConfigurator.Configure(); 
        }


        /// <summary>
        /// Verify the user is able to view the requested resource
        /// </summary>
        protected void Application_AuthorizeRequest()
        {



            // if the path is a freely available path then allow anybody to see the page
            if (IsReqestToPublicResource())
                return;

            // get the user instance
            myCoalUser user = GetUserInfo();
            AttachMyCoalIdentity(user);

            // if not registered take them to register page
            RegistrationBasedRedirect(user);

            // an active registered user is free to go
            if (IsUserActive(user))
                return;

            // redirect the user to specific informational pages regarding their registration status
            UserStatusRedirects(user);

        }


        

        /// <summary>
        /// Attaches a custom identity
        /// </summary>
        /// <param name="user"></param>
        private void AttachMyCoalIdentity(myCoalUser user)
        {
            myCoalPrincipal newUser = new myCoalPrincipal(user, User.Identity);
            HttpContext.Current.User = newUser;
        }

        /// <summary>
        /// Gets the current my coal user
        /// </summary>
        /// <returns></returns>
        private myCoalUser GetUserInfo()
        {
            try
            {
                return myCoalUser.GetInstance();
            }
            catch (Exception ex)
            {
                logger.Error("An error was encountered while getting the myCoalUser Instance", ex);
                throw ex;
            }
        }


        /// <summary>
        /// sends unregistered users immediately to the registration page
        /// </summary>
        private void RegistrationBasedRedirect(myCoalUser user)
        {
            try
            {
                if (!user.IsRegistered)
                    Response.Redirect("~/account/register");
            }
            catch (ThreadAbortException ex)
            {
                // when a redirect occurs it aborts the current thread, which is expected
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while determining the user's registration status", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Is the current user Active
        /// </summary>
        /// <param name="user"></param>
        private bool IsUserActive(myCoalUser user)
        {
            try
            {
                return (user.Status == myCoalUserStatus.Active);
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while determinning the user.Status", ex);
                throw ex;
            }
        }


        // store a static instance of the commonly accessible paths
        private static string[] allowedPaths = new string[] { "/account/register", "/account/registrationreceived", "/account/denied", "/account/inactive", "/account/pending", "/account/unknown" };


        /// <summary>
        /// Determines if the request is being made to a common page
        /// </summary>
        /// <returns></returns>
        private bool IsReqestToPublicResource()
        {
            try
            {
                // anybody can see certain pages relating to the status of their registration
                string path = Request.Path.ToString().ToLower();

                // quickly eleminate most requests
                if (!path.Contains("/account/"))
                    return false;

                // not all account pages are accessible, ensure it is to one of the allowable pages
                foreach (string p in allowedPaths)
                {
                    if (path.Contains(p))
                        return true;
                }

                // if the path did not match any of the registration status pages then don't allow them to proceeed
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while determining if the request is for a public resource", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Redirect the request based on the user status
        /// </summary>
        /// <param name="user"></param>
        private void UserStatusRedirects(myCoalUser user)
        {
            try
            {

                // registered users that aren't active get to an information page specific to their status 
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

                throw new UnauthorizedAccessException("Your status could not be determined");

            }
            catch (ThreadAbortException ex)
            {
                // when a redirect occurs it aborts the current thread, which is expected
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred in UserStatusRedirects", ex);
                throw ex;
            }

        }

    }
}