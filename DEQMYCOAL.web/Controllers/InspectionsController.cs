using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEQMYCOAL.web.Controllers
{
    public class InspectionsController : Controller
    {
        //
        // GET: /Inspections/

        public ActionResult GetInspectionsTab()
        {
            return PartialView();
        }

    }
}
