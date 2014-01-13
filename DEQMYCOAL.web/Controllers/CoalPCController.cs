using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEQMYCOAL.web.Controllers
{
    public class CoalPCController : Controller
    {
        //
        // GET: /CoalPC/

        public ActionResult GetCoalPCTab()
        {
            return PartialView();
        }

    }
}
