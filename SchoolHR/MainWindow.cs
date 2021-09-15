using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using School_HR_API;

namespace SchoolHR
{
    public enum EmployeeType
    {
        Teacher,
        HeadOfDepartment,
        HeadMaster
    }

    #region Form class
    public partial class AddEmpWin : Form
    {
        #region Variables area
        List<IEmployee> employees;   // List of type employee interface to store all profiles
        int ProfileIndex = -1;       // Record the last index of added profiles
        #endregion 

        public AddEmpWin()
        {
            InitializeComponent();
        }

        #region User defined methods
        // Insert data form the list to the fields
        private void GetProfile()
        {
            if (ProfileIndex > -1)
            {
                EmpIDTxt.Text = employees[ProfileIndex].Id.ToString();
                EmpNameTxt.Text = employees[ProfileIndex].Name;
                if (employees[ProfileIndex].Job == "Teacher")
                  EmpJobCombo.SelectedItem = EmployeeType.Teacher;
                if (employees[ProfileIndex].Job == "Head of department")
                    EmpJobCombo.SelectedItem = EmployeeType.HeadOfDepartment;
                if (employees[ProfileIndex].Job == "Head master")
                    EmpJobCombo.SelectedItem = EmployeeType.HeadMaster;
                EmpSalaryTxt.Text = employees[ProfileIndex].Salary.ToString();
            }
            // If the list is empty
            else
            {
                EmpIDTxt.Clear();
                EmpNameTxt.Clear();
                EmpJobCombo.SelectedItem = EmployeeType.Teacher;
                EmpSalaryTxt.Clear();
            }

        }
        
        // Insert the total number of employees and total salaries into the form
        private void UpdateTotal()
        {
            NumOfEmpLbl.Text = employees.Count.ToString();

            // Calculate the total salaries using lambda expression
            TotalSalShowLbl.Text = employees.Sum(emps => emps.Salary).ToString();
        }
        #endregion

        #region form events
        // Form load event
        private void AddEmpWin_Load(object sender, EventArgs e)
        {
            employees = new List<IEmployee>();

            // Insert the employees enum as a data source for the employee type combobox
            EmpJobCombo.DataSource = Enum.GetValues(typeof(EmployeeType));
        }

        // Add profile button click event
        private void AddEmpBtn_Click(object sender, EventArgs e)
        {
            // Convert selected item from job combobox to job's type enum and store it in a variable
            EmployeeType EmpTypeTemp = (EmployeeType)EmpJobCombo.SelectedItem;

            int id = int.Parse(EmpIDTxt.Text);
            decimal salary = decimal.Parse(EmpSalaryTxt.Text);

            employees.Add(EmployeeFactory.GetEmployeeInstane(id, EmpNameTxt.Text, EmpTypeTemp, salary));

            ProfileIndex = employees.Count - 1;

            UpdateTotal();
        }

        // Clear profile button click event
        private void ClearEmpBtn_Click(object sender, EventArgs e)
        {
            if (ProfileIndex > -1)
            {
                employees.RemoveAt(ProfileIndex);

                // If the deleted profile was the last one, decrease the profile by one
                if (ProfileIndex == employees.Count)
                    ProfileIndex--;

                GetProfile();
                UpdateTotal();
            }
        }

        // Id field key press event
        private void EmpIDTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Force the field to accept numbers and backspace signals only
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == Convert.ToChar(Keys.Back))
                e.Handled = false;
            else
                e.Handled = true;
        }

        // Employee salary key press event
        private void EmpSalaryTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == '.')
                e.Handled = false;
            else
                e.Handled = true;
        }

        // First profile button click event
        private void FirstProfileB_Click(object sender, EventArgs e)
        {
            if (ProfileIndex > 0)
                ProfileIndex = 0; GetProfile();
        }

        // Previous profile button click event
        private void PreviousProfileB_Click(object sender, EventArgs e)
        {
            if (ProfileIndex > 0)
                --ProfileIndex; GetProfile();
        }

        // Next profile button click event
        private void NextProfileB_Click(object sender, EventArgs e)
        {
            if (ProfileIndex < employees.Count - 1)
                ++ProfileIndex; GetProfile();
        }

        // Last profile button click event
        private void LastProfileB_Click(object sender, EventArgs e)
        {
            if (ProfileIndex < employees.Count - 1)
                ProfileIndex = employees.Count - 1; GetProfile();
        }
        #endregion
    }
    #endregion

    #region Employee classes
    public class Teacher : EmployeeBase
    {
        public override decimal Salary => base.Salary + (base.Salary * 0.02m);
    }

    public class HeadOfDepartment : EmployeeBase
    {
        public override decimal Salary { get => base.Salary + (base.Salary * 0.03m); }
    }

    public class HeadMaster : EmployeeBase
    {
        public override decimal Salary { get => base.Salary + (base.Salary * 0.04m); }
    }
    #endregion

    public static class EmployeeFactory
    {
        public static IEmployee GetEmployeeInstane(int id, string name, EmployeeType employeeType, decimal salary)
        {
            IEmployee employee = null;

            switch (employeeType)
            {
                case EmployeeType.Teacher:
                    employee = new Teacher { Id = id, Name = name, Job = "Teacher", Salary = salary };
                    break;
                case EmployeeType.HeadOfDepartment:
                    employee = new HeadOfDepartment { Id = id, Name = name, Job = "Head of department", Salary = salary };
                    break;
                case EmployeeType.HeadMaster:
                    employee = new HeadMaster { Id = id, Name = name, Job = "Head master", Salary = salary };
                    break;
                default:
                    break;
            }
            return employee;
        }
    }
}
