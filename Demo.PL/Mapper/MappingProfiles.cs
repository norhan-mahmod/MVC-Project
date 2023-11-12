using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;

namespace Demo.PL.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
