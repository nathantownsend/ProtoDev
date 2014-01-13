using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEQMYCOAL.web.Controllers
{
    public class BondEstimatesController : Controller
    {
        //
        // GET: /BondEstimates/

        public ActionResult GetBondEstimatesTab()
        {
            return PartialView();
        }

    }
}
