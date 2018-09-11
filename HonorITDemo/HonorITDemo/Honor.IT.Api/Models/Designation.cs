using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Honor.IT.Api.Models
{
    //[Table("Designation")]
    public class Designation
    {
        public Guid Id { get; set; }

        public string DesignationName { get; set; }
    }
}