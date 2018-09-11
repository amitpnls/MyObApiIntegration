using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Honor.IT.Api.Models
{
    //[Table("Department")]
    public class Department
    {
        public Guid Id { get; set; }

        public string DepartName { get; set; }
    }
}