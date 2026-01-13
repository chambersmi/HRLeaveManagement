using AutoMapper;
using HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveTypes;

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
