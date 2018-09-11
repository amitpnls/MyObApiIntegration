using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Honor.IT.Api.Models
{
    //[Table("Employee")]
    public class Employee
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateofBirth { get; set; }

        public DateTime DateofJoin { get; set; }

        public Guid DepartmentID { get; set; }

        public Guid DesignationID { get; set; }
    }
}