using AutoMapper;
using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveTypes;

namespace HRLeaveManagement.BlazorUI.Services
{
    public class LeaveTypeService : BaseHttpService, ILeaveTypeService
    {
        private readonly IMapper _mapper;

        public LeaveTypeService(IClient client, IMapper mapper, ILocalStorageService localStorageService) : base(client) // ???????
        {
            _mapper = mapper;
        }

        public async Task<Response<Guid>> CreateLeaveType(LeaveTypeViewModel leaveType)
        {
            try
            {
                await AddBearerToken();
                // Create leave type command
                var createLeaveTypeCommand = _mapper.Map<CreateLeaveTypeCommand>(leaveType);
                await _client.LeaveTypesPOSTAsync(createLeaveTypeCommand);
                return new Response<Guid>()
                {
                    Success = true
                };
            } 
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }            
        }

        public async Task<Response<Guid>> DeleteLeaveType(int id)
        {
            try
            {
                await AddBearerToken();
                await _client.LeaveTypesDELETEAsync(id);
                return new Response<Guid>()
                {
                    Success = true
                };
            } catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }

        public async Task<LeaveTypeViewModel> GetLeaveTypeDetails(int id)
        {
            await AddBearerToken();
            var leaveType = await _client.LeaveTypesGETAsync(id);
            return _mapper.Map<LeaveTypeViewModel>(leaveType);
        }

        public async Task<List<LeaveTypeViewModel>> GetLeaveTypes()
        {
            await AddBearerToken();
            var leaveTypes = await _client.LeaveTypesAllAsync();
            return _mapper.Map<List<LeaveTypeViewModel>>(leaveTypes);
        }

        public async Task<Response<Guid>> UpdateLeaveType(int id, LeaveTypeViewModel leaveType)
        {
            try
            {
                await AddBearerToken();
                var updateLeaveTypeCommand = _mapper.Map<UpdateLeaveTypeCommand>(leaveType);
                await _client.LeaveTypesPUTAsync(id.ToString(), updateLeaveTypeCommand);
                
                return new Response<Guid>()
                {
                    Success = true
                };
            } catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }
    }
}
