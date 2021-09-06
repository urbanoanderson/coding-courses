using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress, opt =>
                {
                    opt.MapFrom(x => string.Join(' ', x.Address, x.Country));
                });

            this.CreateMap<Employee, EmployeeDto>();

            this.CreateMap<CompanyForCreationDto, Company>();

            this.CreateMap<CompanyForUpdateDto, Company>();

            this.CreateMap<EmployeeForCreationDto, Employee>();

            this.CreateMap<EmployeeForUpdateDto, Employee>();
        }
    }
}
