using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{

    public class UserSearchModel
    {
        public ApplicationUser User { set; get; }
        public string Role { set; get; }
    }

}