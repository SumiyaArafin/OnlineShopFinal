using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? JoiningDate { get; set; }
        public decimal? Salary { get; set; }
        public string EmployeeType { get; set; }
        public string Email { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public string ContactNo { get; set; }
        public string EmergencyContactNo { get; set; }
        public string EmployeeStatus { get; set; }
        public bool? IsActive { get; set; }
        public int? DesignationId { get; set; }

        public Designation Designation { get; set; }
    }
}
