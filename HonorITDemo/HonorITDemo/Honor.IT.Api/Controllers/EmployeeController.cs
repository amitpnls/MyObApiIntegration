using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using Honor.IT.Api.Models;
using System.Net.Http;
using System.Net;

namespace Honor.IT.Api.Controllers
{
    public class EmployeeController : ApiController
    {
        static IEmployeeRepository employeeRepository = new EmployeeRepository();

        [Route("employees")]
        [HttpGet]
        public IEnumerable<Employee> GetAll()
        {
            return employeeRepository.GetAll();
        }

        [Route("employees/{Id:guid}", Name = "EmployeeInfoById")]
        public Employee GetEmployee(Guid Id)
        {
            Employee employee = employeeRepository.Get(Id);

            if (employee == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No employee with ID = {0} exists", Id)),
                    ReasonPhrase = "The given employee id was not found."
                };

                throw new HttpResponseException(response);
            }
            return employee;
        }

        [Route("employees")]
        [HttpPost]
        public HttpResponseMessage PostEmployee(Employee employee)
        {
            employee = employeeRepository.Add(employee);
            var response = Request.CreateResponse<Employee>(HttpStatusCode.Created, employee);

            string uri = Url.Link("EmployeeInfoById", new { id = employee.Id });
            response.Headers.Location = new Uri(uri);

            return response;
        }

        [Route("employees")]
        [HttpPut]
        public void PutEmployee(Guid Id, Employee employee)
        {
            employee.Id = Id;
            //if (!employeeRepository.Update(employee))
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
            //}
        }

        [Route("employees")]
        [HttpDelete]
        public void DeleteEmployee(Guid Id)
        {
            Employee employee = employeeRepository.Get(Id);
            if (employee == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            employeeRepository.Remove(Id);
        }
    }
}