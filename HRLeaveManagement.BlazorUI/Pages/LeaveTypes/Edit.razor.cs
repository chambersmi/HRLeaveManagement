using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveTypes;
using HRLeaveManagement.Domain;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Edit
    {
        [Inject]
        ILeaveTypeService _client { get; set; }

        [Inject]
        NavigationManager _navManager { get; set; }

        [Parameter]
        public int id { get; set; }
        public string Message { get; private set; }

        LeaveTypeViewModel leaveType = new LeaveTypeViewModel();

        protected async override Task OnParametersSetAsync()
        {
            leaveType = await _client.GetLeaveTypeDetails(id);
        }

        async Task EditLeaveType()
        {
            var response = await _client.UpdateLeaveType(id, leaveType);
            if (response.Success)
            {
                _navManager.NavigateTo("/leavetypes/");
            }
            Message = response.Message;
        }
    }
}