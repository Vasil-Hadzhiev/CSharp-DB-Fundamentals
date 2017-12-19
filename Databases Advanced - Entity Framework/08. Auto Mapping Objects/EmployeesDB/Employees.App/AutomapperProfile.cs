namespace Employees.App
{
    using AutoMapper;
    using Employees.DtoModels;
    using Employees.Models;

    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();
            CreateMap<Employee, EmployeePersonalDto>();
            CreateMap<Employee, ManagerDto>()
                .ForMember(dest => dest.EmployeesCount,
                           opt => opt.MapFrom(src => src.Employees.Count))
                .ForMember(dest => dest.Employees,
                           opt => opt.MapFrom(src => src.Employees));
            CreateMap<Employee, EmployeeWithManagerDto>()
                .ForMember(dest => dest.ManagerName,
                           opt => opt.MapFrom(src => src.Manager.LastName));
        }
    }
}