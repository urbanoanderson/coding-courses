using Contracts;
using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Utility
{
    public class EmployeeLinks
    {
        private readonly LinkGenerator linkGenerator;

        private readonly IDataShaper<EmployeeDto> dataShaper;

        public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
        {
            this.linkGenerator = linkGenerator;
            this.dataShaper = dataShaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, string fields,
            Guid companyId, HttpContext httpContext)
        {
            var shapedEmployees = this.ShapeData(employeesDto, fields);

            if (this.ShouldGenerateLinks(httpContext))
                return this.ReturnLinkedEmployees(employeesDto, fields, companyId, httpContext, shapedEmployees);

            return this.ReturnShapedEmployees(shapedEmployees);
        }

        private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields)
        {
            return this.dataShaper.ShapeData(employeesDto, fields).Select(e => e.Entity).ToList();
        }
        
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }
        
        private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees)
        {
            return new LinkResponse() { ShapedEntities = shapedEmployees };
        }

        private LinkResponse ReturnLinkedEmployees(IEnumerable<EmployeeDto> employeesDto, string fields,
            Guid companyId, HttpContext httpContext, List<Entity> shapedEmployees)
        {
            var employeeDtoList = employeesDto.ToList();

            for (var index = 0; index < employeeDtoList.Count(); index++)
            {
                var employeeLinks = this.CreateLinksForEmployee(httpContext, companyId, employeeDtoList[index].Id, fields);
                shapedEmployees[index].Add("Links", employeeLinks);
            }

            var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
            var linkedEmployees = this.CreateLinksForEmployees(httpContext, employeeCollection);

            return new LinkResponse() { HasLinks = true, LinkedEntities = linkedEmployees };
        }

        private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId, Guid id, string fields = "")
        {
            return new List<Link>()
            {
                new Link(this.linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany", 
                    values: new { companyId, id, fields }), "self", "GET"),
                new Link(this.linkGenerator.GetUriByAction(httpContext, "DeleteEmployeeForCompany",
                    values: new { companyId, id }), "delete_employee", "DELETE"),
                new Link(this.linkGenerator.GetUriByAction(httpContext, "UpdateEmployeeForCompany",
                    values: new { companyId, id }), "update_employee", "PUT"),
                new Link(this.linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateEmployeeForCompany",
                    values: new { companyId, id }), "partially_update_employee", "PATCH"),
            };
        }
        
        private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext, LinkCollectionWrapper<Entity> employeesWrapper)
        {
            employeesWrapper.Links.Add(new Link(this.linkGenerator.GetUriByAction(httpContext, "GetEmployeesForCompany",
                    values: new { }), "self", "GET"));

            return employeesWrapper;
        }
    }
}
