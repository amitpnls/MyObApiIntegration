using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Honor.IT.Api.Models
{
    interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();

        Employee Get(Guid Id);

        void Remove(Guid Id);

        bool Update(Guid Id, Employee employee);

        Employee Add(Employee employee);
    }
}