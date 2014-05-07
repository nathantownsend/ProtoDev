using DEQMYCOAL.web.Models;
using DEQMYCOAL.web.ViewModels;
using ePermitBLL;
using ePermitDAL.DO.dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEQMYCOAL.web.Controllers
{
    public class ePermitController : Controller
    {

        public ActionResult Help(string page)
        {
            try
            {
                string[] parts = page.Split('/');
                string url = string.Format("~/{0}/{1}", parts[parts.Length - 2], parts[parts.Length - 1]);
                HelpBO model = TOCBLL.GetHelp(url);
                return View(model);
            }
            catch
            {
                return RedirectToAction("HelpNotFound");
            }
        }

        public ActionResult HelpNotFound()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles=CoalRoles.Owner)]
        [ValidateInput(false)]
        public ActionResult UpdateInstructions(string Html, int TocID)
        {
            try
            {
                bool ret = TOCBLL.SaveInstruction(TocID, Html);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The instructions have been updated");
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }
        }


        
        /// <summary>
        /// Gets the main screen for choosing an ePermit
        /// </summary>
        /// <returns></returns>
        public ActionResult GetePermitTab()
        {
            List<PermitRegistrationBO> registrations = PermitRegistrationBLL.GetPermitRegistrationsByRegistrationID(myCoalUser.GetInstance().RegistrationId);
            int[] keys = (from r in registrations select r.PermitRegistration.PermitKey).ToArray();
            PermitDO[] model = PermitBLL.GetPermits().Where(p => keys.Contains(p.PermitKey)).ToArray();
            return PartialView(model);
        }




        public ActionResult AddApplication()
        {
            return PartialView();
        }



        #region Create new permit

        public ActionResult NewPermit()
        {
            NewPermitVM model = new NewPermitVM();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult NewPermit(NewPermitVM model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            try
            {
                PermitDO newPermit = new PermitDO() { SiteName = model.SiteName };
                int permitKey = PermitBLL.Save(newPermit, (int)myCoalUser.GetInstance().RegistrationId);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The permit was created");
                result.Data.Add("PermitKey", permitKey);
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }

        }

        #endregion


        #region Modify Permit Registration

        /// <summary>
        /// Enables an owner to add a user to a permit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = CoalRoles.Owner)]
        public ActionResult NewPermitRegistration(FormCollection data)
        {
            try
            {
                int RegistrationID = Convert.ToInt32(data["RegistrationID"]);
                int PermitKey = Convert.ToInt32( data["PermitKey"]);
                bool Read = Convert.ToBoolean(data["Read"]);
                bool Edit = Convert.ToBoolean(data["Edit"]);
                bool Coordinator = Convert.ToBoolean(data["Coordinator"]);

                PermitRegistrationDO record = new PermitRegistrationDO() { Coordinator = Coordinator, Edit = Edit, PermitKey = PermitKey, RegistrationID = RegistrationID, Read = Read };
                record.PermitRegistrationID = PermitRegistrationBLL.Save(record);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The user's permission was added");
                result.Data.Add("PermitRegistration", record);
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }
            
        }


        /// <summary>
        /// Provides a form for updating a user's permission on a permit
        /// </summary>
        /// <param name="PermitRegistrationId"></param>
        /// <returns></returns>
        [Authorize(Roles = CoalRoles.Owner + " " + CoalRoles.PermitCoordinator)]
        public ActionResult EditPermitRegistration(int PermitRegistrationId)
        {
            PermitRegistrationBO model = PermitRegistrationBLL.GetPermitRegistration(PermitRegistrationId);

            return PartialView(model);
        }


        /// <summary>
        /// Updates a user's permission on a permit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = CoalRoles.Owner + " " + CoalRoles.PermitCoordinator)]
        public ActionResult EditPermitRegistration(PermitRegistrationBO model)
        {
            try
            {
                // permit coordinators don't see the coordinator role, therefore it must be preserved when the post.
                if (myCoalUser.GetInstance().IsInRole(CoalRoles.PermitCoordinator))
                {
                    PermitRegistrationBO original = PermitRegistrationBLL.GetPermitRegistration(model.PermitRegistration.PermitRegistrationID);
                    model.PermitRegistration.Coordinator = original.PermitRegistration.Coordinator;
                }


                PermitRegistrationBLL.Save(model.PermitRegistration);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The user permissions were saved");
                result.Data.Add("PermitRegistration", model.PermitRegistration);
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }
        }


        /// <summary>
        /// Removes a user's permission from a permit
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = CoalRoles.Owner + " " + CoalRoles.PermitCoordinator)]
        public ActionResult DeletePermitRegistration(int Id)
        {
            try
            {
                bool val = PermitRegistrationBLL.Delete(Id);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The user permissions were deleted");
                result.Data.Add("PermitRegistrationID", Id);
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }
        }
        
        
        #endregion


        /// <summary>
        /// Updates the permit access code for a permit
        /// </summary>
        /// <param name="PermitKey"></param>
        /// <param name="AccessCode"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles=CoalRoles.PermitCoordinator)]
        public ActionResult UpdatePermitAccessCode(string oldAccessCode, string newAccessCode)
        {
            try
            {
                PermitDO permit = PermitBLL.GetPermitByAccessCode(oldAccessCode);
                permit.AccessCode = newAccessCode;
                PermitBLL.Save(permit, myCoalUser.GetInstance().RegistrationId);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The permit access code was updated");
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }
        }


        /// <summary>
        /// Attempts to create a new permit registration without any permissions set
        /// </summary>
        /// <param name="PermitAccessCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RequestPermitAccess(string PermitAccessCode)
        {
            try
            {
                RegistrationDO registrationDO = RegistrationBLL.GetRegistrationByUserToken(myCoalUser.UserToken);
                PermitRegistrationBLL.RequestPermitAccess(registrationDO, PermitAccessCode);
                PermitDO permit = PermitBLL.GetPermitByAccessCode(PermitAccessCode);

                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "Your request has been sent.");
                result.Data.Add("SiteName", permit.SiteName);
                result.Data.Add("PermitId", permit.PermitID);
                return Json(result);
            }
            catch (System.NullReferenceException ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, "The permit access code you entered could not be found. Please contact the permit coordinator for the correct access code.");
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }
        }

        


    }
}
