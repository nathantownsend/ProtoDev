using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ePermitDAL.DO.dbo;

namespace DEQMYCOAL.web.ViewModels
{
    public class RegistrationVM
    {
        public RegistrationDO Registration { get; set; }

        public CompanyDO[] Companies { get; set; }
    }
}