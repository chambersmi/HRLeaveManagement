using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.GetLeaveTypeDetails;

public class GetLeaveTypeDetailsQueryHandler : IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetLeaveTypeDetailsQueryHandler> _logger;

    public GetLeaveTypeDetailsQueryHandler(
        ILeaveTypeRepository leaveTypeRepository, 
        IMapper mapper,
        IAppLogger<GetLeaveTypeDetailsQueryHandler> logger)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
        _logger = logger;

    }

    public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        // Query the database
        var leaveType = await _leaveTypeRepository.GetByIdAsync(request.Id);

        // Verify that record exists
        if (leaveType == null)
        {
            throw new NotFoundException(nameof(LeaveType), request.Id);
        }

        // Convert data object to DTO object
        var data = _mapper.Map<LeaveTypeDetailsDto>(leaveType);

        // Return data with logger tester
        _logger.LogInformation("Leave types were successfully retrieved.");
        return data;
    }
}
