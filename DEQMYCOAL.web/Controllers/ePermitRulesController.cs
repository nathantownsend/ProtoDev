using DEQMYCOAL.web.Models;
using ePermitBLL;
using ePermitDAL.DO.dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEQMYCOAL.web.Controllers
{
    public class ePermitRulesController : Controller
    {
        //
        // GET: /ePermitRules/

        public ActionResult Index()
        {
            List<RuleBO> rules = RuleBLL.GetRules();
            return View(rules);
        }



        [Authorize(Roles=CoalRoles.Owner)]
        public ActionResult EditSection(string Section)
        {
            RuleBO rule = RuleBLL.GetRulesBySection(Section);
            RuleSectionDO model = rule.RuleSection;
            return PartialView(model);
        }


        [Authorize(Roles = CoalRoles.Owner)]
        [HttpPost]
        public ActionResult EditSection(RuleSectionDO RuleSection)
        {
            try
            {
                RuleBLL.Save(RuleSection);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The Section was updated");
                result.Data.Add("Section", RuleSection);
                return Json(result);
            }
            catch (Exception ex)
            {
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.ERROR, ex.Message);
                return Json(result);
            }
        }


        [Authorize(Roles = CoalRoles.Owner)]
        public ActionResult EditSubSection(string SubSection)
        {
            RuleBO rule = RuleBLL.GetRulesBySubSection(SubSection);
            RuleSubSectionDO model = rule.RuleSubSection.Where(r => r.SubSection == SubSection).FirstOrDefault();
            return PartialView(model);
        }


        [Authorize(Roles = CoalRoles.Owner)]
        [HttpPost]
        public ActionResult EditSubSection(RuleSubSectionDO RuleSubSection)
        {
            try
            {
                RuleBLL.Save(RuleSubSection);
                AjaxResult result = new AjaxResult(AjaxResult.AjaxStatus.OK, "The SubSection was updated");
                result.Data.Add("SubSection", RuleSubSection);
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
