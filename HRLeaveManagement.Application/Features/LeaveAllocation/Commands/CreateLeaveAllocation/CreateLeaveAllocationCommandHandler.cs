using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;

        public CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationCommandValidator(_leaveAllocationRepository);
            var validationResult = await validator.ValidateAsync(request);

            if(validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid Leave Allocation Request.");
            }

            var leaveType = await _leaveAllocationRepository.GetByIdAsync(request.LeaveTypeId);

            // Get Employees

            // Get Period

            // Assign Allocations
            var leaveAllocation = _mapper.Map<Domain.LeaveAllocation>(request);
            await _leaveAllocationRepository.CreateAsync(leaveAllocation);
            
            return Unit.Value;

        }
    }
}
