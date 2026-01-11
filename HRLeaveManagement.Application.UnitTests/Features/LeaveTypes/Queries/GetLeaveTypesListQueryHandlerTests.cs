using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Queries
{
    public class GetLeaveTypeListQueryHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly Mock<IAppLogger<GetLeaveTypesQueryHandler>> _mockAppLogger;
        private IMapper _mapper;

        public GetLeaveTypeListQueryHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();
            _mockAppLogger = new Mock<IAppLogger<GetLeaveTypesQueryHandler>>();

            //var mapperConfig = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<LeaveTypeProfile>();
            //}, loggerFactory);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                // leave empty as last ditch effort to get automapper to work below (mapperConfig)
            });

            var mapperConfig = new MapperConfiguration(
                cfg => cfg.AddProfile<LeaveTypeProfile>(),
                loggerFactory);

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetLeaveTypeListTest()
        {
            var handler = new GetLeaveTypesQueryHandler(_mapper, _mockRepo.Object, _mockAppLogger.Object);

            var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);

            result.ShouldBeOfType<List<LeaveTypeDto>>();
            result.Count.ShouldBe(3);
        }
    }
}