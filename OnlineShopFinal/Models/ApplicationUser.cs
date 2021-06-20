using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string TownOrCity { get; set; }
        public string StateOrCountry { get; set; }
        public string PostCodeOrZip { get; set; }
        public string Address { get; set; }
    }
}
