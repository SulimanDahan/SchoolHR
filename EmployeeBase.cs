using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_HR_API
{
    public class EmployeeBase : IEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public virtual decimal Salary { get; set; }
    }
}
