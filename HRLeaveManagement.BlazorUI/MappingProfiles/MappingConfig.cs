using AutoMapper;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveTypes;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<LeaveTypeDto, LeaveTypeViewModel>().ReverseMap();
            CreateMap<CreateLeaveTypeCommand, LeaveTypeViewModel>().ReverseMap();
            CreateMap<UpdateLeaveTypeCommand, LeaveTypeViewModel>().ReverseMap();
        }
    }
}
