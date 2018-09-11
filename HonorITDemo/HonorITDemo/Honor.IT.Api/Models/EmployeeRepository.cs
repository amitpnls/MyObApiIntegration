using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Honor.IT.Api.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private HonorITDbContext db = new HonorITDbContext();

        //private List<Employee> employees = new List<Employee>();



        public EmployeeRepository()
        {
            //PopulateInitialData();
        }

        public IEnumerable<Employee> GetAll()
        {
            return db.Employees.ToList();
        }

        public Employee Get(Guid Id)
        {
            Employee employee = db.Employees.FirstOrDefault(x => x.Id == Id);
            if (employee == null)
            {
                return employee;
            }
            return employee;
        }

        public void Remove(Guid Id)
        {
            Employee employee = db.Employees.Find(Id);

            if (employee != null)
            {
                return;
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return;
        }

        public bool Update(Guid Id, Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("employee");
            }

            if (Id != employee.Id)
            {
                return true;
            }

            db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public Employee Add(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("employee");
            }

            db.Employees.Add(employee);
            db.SaveChanges();

            return employee;
        }

        private void PopulateInitialData()
        {
            Add(new Employee { FirstName = "Tejas", MiddleName = "M", LastName = "Dhimmar", DateofBirth = Convert.ToDateTime("1982-11-15"), DateofJoin = Convert.ToDateTime("2015-07-06"), DepartmentID = new Guid("e4be10c4-d7b6-40fe-8f93-b0c21b95f0da"), DesignationID = new Guid("67fed07f-adb6-48a3-9713-a5dea97195ed") });
            Add(new Employee { FirstName = "Pinki", MiddleName = "V", LastName = "Dhimmar", DateofBirth = Convert.ToDateTime("1989-11-24"), DateofJoin = Convert.ToDateTime("2015-12-12"), DepartmentID = new Guid("e4be10c4-d7b6-40fe-8f93-b0c21b95f0da"), DesignationID = new Guid("67fed07f-adb6-48a3-9713-a5dea97195ed") });
            Add(new Employee { FirstName = "Hiren", MiddleName = "", LastName = "Amin", DateofBirth = Convert.ToDateTime("1991-10-02"), DateofJoin = Convert.ToDateTime("2015-07-10"), DepartmentID = new Guid("e4be10c4-d7b6-40fe-8f93-b0c21b95f0da"), DesignationID = new Guid("67fed07f-adb6-48a3-9713-a5dea97195ed") });
        }
    }
}