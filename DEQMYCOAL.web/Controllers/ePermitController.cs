using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEQMYCOAL.web.Controllers
{
    public class ePermitController : Controller
    {
        //
        // GET: /ePermit/

        public ActionResult GetePermitTab()
        {
            return PartialView();
        }

    }
}
