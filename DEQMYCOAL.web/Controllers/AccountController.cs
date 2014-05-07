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

        public ActionResult Logout()
        {
            myCoalUser.RemoveCookie();
            return Redirect("https://dev.adfs.mt.gov/adfs/ls/?wa=wsignout1.0&wreply=https://dev.adfs.mt.gov/stsclient35/logon.aspx");
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
            UserProfileBO profile = RegistrationBLL.GetUserProfile(myCoalUser.UserToken);

            SecurityVM model = new SecurityVM(profile);
            

            return View(model);
        }

        [Authorize(Roles="owner")]
        public ActionResult SecurityPanel(string userToken)
        {
            UserProfileBO profile = RegistrationBLL.GetUserProfile(userToken);
            SecurityVM model = new SecurityVM(profile);
            return PartialView(model);
        }



        [ChildActionOnly]
        [Authorize(Roles="owner")]
        public ActionResult UserTable()
        {
            RegistrationDO[] model = RegistrationBLL.GetRegistrations();
            return PartialView(model);
        }

        public ActionResult MyInfo(int RegistrationId)
        {
            RegistrationDO reg = RegistrationBLL.GetRegistration(RegistrationId);
            RegistrationVM model = new RegistrationVM() { Registration = reg };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult MyInfo(RegistrationVM model)
        {
            model.Registration.StateID = model.States.Where(s => s.Selected).FirstOrDefault().Value;
            model.Registration.UserToken = myCoalUser.UserToken;

            if (!ModelState.IsValid)
                return PartialView("MyInfo", model);

            try
            {

                // the system owner can update anybody's registration, but anyone else can only update their own
                // therefore only lookup a user by id when the owner is performing the task to prevent a user from maliciously 
                // chaning their RegistrationId before posting the form thereby updating someone else's data
                RegistrationDO data;
                if (myCoalUser.GetInstance().IsInRole("owner"))
                    data = RegistrationBLL.GetRegistration(model.Registration.RegistrationID);
                else
                    data = RegistrationBLL.GetRegistrationByUserToken(myCoalUser.UserToken);

                // restrict the update to the visible elements on the form
                data.Address1 = model.Registration.Address1;
                data.City = model.Registration.City;
                data.CompanyName = model.Registration.CompanyName;
                data.CountryCode = model.Registration.CountryCode;
                data.Email = model.Registration.Email;
                data.FirstName = model.Registration.FirstName;
                data.LastName = model.Registration.LastName;
                data.Phone = model.Registration.Phone;
                data.PhoneExtension = model.Registration.PhoneExtension;
                data.RegistrationDescription = model.Registration.RegistrationDescription;
                data.StateID = model.Registration.StateID;
                data.Title = model.Registration.Title;
                data.Zipcode = model.Registration.Zipcode;

                RegistrationBLL.Save(data);

                // refresh the cookie values 
                myCoalUser.ResetProfileCookie();

                // return ok to let the javascript clien tknow the update went well
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "User profile information was saved");
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }

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
        public ActionResult myPermitsCoordinatorPermitPanel(int PermitKey)
        {
            PermitAccessBO model = PermitRegistrationBLL.GetPermitAccess(PermitKey);
            return PartialView(model);
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

        [Authorize(Roles = "owner")]
        [HttpPost]
        public ActionResult Permissions(SecurityVM model) 
        {
            try
            {
                RegistrationBLL.Save(model.Registration.Registration.RegistrationID, model.Registration.Registration.RegistrationStatusID, model.Roles);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The user's permissions were set");
                return Json(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(model);
            }
        }


        #endregion


    }
}

