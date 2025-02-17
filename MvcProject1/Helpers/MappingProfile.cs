using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MvcProject1.DAL.Models;
using MvcProject1.PL.ViewModels;

namespace MvcProject1.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>()
                .ReverseMap();
            CreateMap<DepartmentViewModel, Department>()
                .ReverseMap();
            CreateMap<UserViewModel, AppUser>()
                .ReverseMap();
            CreateMap<RoleViewModel, IdentityRole>()
                .ReverseMap();
        }
    }
}
