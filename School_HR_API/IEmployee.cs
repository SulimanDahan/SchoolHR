using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_HR_API
{
    public interface IEmployee
    {
        int Id { get; set; }

        string Name { get; set; }

        string Job { get; set; }

        decimal Salary { get; set; }
    }
}
