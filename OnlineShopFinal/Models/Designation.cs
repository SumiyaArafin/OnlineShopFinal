using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopFinal.Models
{
    public partial class Designation
    {
        public Designation()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Salary { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
