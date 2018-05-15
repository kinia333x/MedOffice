using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedOffice.Models
{

    public class UserSearchModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Specialization { get; set; }
        public string Role { get; set; }
    }
}