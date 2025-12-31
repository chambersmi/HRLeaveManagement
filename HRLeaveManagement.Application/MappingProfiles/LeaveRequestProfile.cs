using AutoMapper;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using HRLeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.MappingProfiles
{
    public class LeaveRequestProfile : Profile
    {
        public LeaveRequestProfile()
        {
            CreateMap<LeaveRequestListDto, LeaveRequest>().ReverseMap();
            CreateMap<LeaveRequestDetailDto, LeaveRequest>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestDetailDto>();
            CreateMap<CreateLeaveRequestCommand, LeaveRequest>();
            CreateMap<UpdateLeaveRequestCommand, LeaveRequest>();
        }
    }
}
