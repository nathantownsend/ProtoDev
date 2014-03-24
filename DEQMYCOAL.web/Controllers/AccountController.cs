using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DEQMYCOAL.web.Models;
using DEQMYCOAL.web.ViewModels;
using ePermitBLL;
using ePermitDAL.DAL.dbo;
using ePermitDAL.DO.dbo;

namespace DEQMYCOAL.web.Controllers
{
    public class AccountController : Controller
    {
        


        public ActionResult Register()
        {
            // don't allow users to register twice
            if (RegistrationBLL.IsUserRegistered(myCoalUser.UserToken))
            {
                //myCoalUser.GetInstance().
                return RedirectToAction("RegistrationReceived");
            }

            ViewBag.Message = "Register";
            RegistrationDO reg = new RegistrationDO() { CountryCode = "1" };
            RegistrationVM model = new RegistrationVM() { Registration = reg };
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegistrationVM model)
        {
            model.Registration.StateID = model.States.Where(s => s.Selected).FirstOrDefault().Value;
            model.Registration.AccessRoleID = model.AccessRoles.Where(s => s.Selected).First().Value;
            model.Registration.UserToken = myCoalUser.UserToken;
            

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // strip out any non-numeric characters
                Regex re = new Regex(@"[^\d]");
                model.Registration.Phone = re.Replace(model.Registration.Phone, "");

                // request registration
                RegistrationBLL.Save(model.Registration);

                // show confirmation screen
                return RedirectToAction("RegistrationReceived", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        #region Registration Information Views

        public ActionResult RegistrationReceived()
        {
            return View();
        }

        public ActionResult Denied()
        {
            return View();
        }

        public ActionResult Inactive()
        {
            return View();
        }

        public ActionResult Pending()
        {
            return View();
        }

        public ActionResult Unknown()
        {
            return View();
        }

        #endregion


        #region Security / User Management

        public ActionResult Manage()
        {
            return View();
        }


        [ChildActionOnly]
        [Authorize(Roles="owner")]
        public ActionResult UserTable()
        {
            return PartialView();
        }

        public ActionResult MyInfo()
        {
            return PartialView();
        }

        public ActionResult MyNotifications()
        {
            return PartialView();
        }

        [ChildActionOnly]
        [Authorize(Roles = "owner")]
        public ActionResult myPermitsOwner()
        {
            return PartialView();
        }


        [ChildActionOnly]
        [Authorize(Roles = "permitcoordinator")]
        public ActionResult myPermitsCoordinator()
        {
            return PartialView();
        }

        [Authorize(Roles="permitcoordinator")]
        public ActionResult myPermitsCoordinatorPermitPanel()
        {
            return PartialView();
        }

        public ActionResult myPermitsUser()
        {
            return PartialView();
        }


        [Authorize(Roles="owner")]
        public ActionResult Permissions()
        {
            return PartialView();
        }

        #endregion


    }
}

