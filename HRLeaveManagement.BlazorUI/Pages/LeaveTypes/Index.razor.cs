using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.LeaveTypes;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Index
    {
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ILeaveTypeService LeaveTypeService { get; set; }
        [Inject] public ILeaveAllocationService LeaveAllocationService { get; set; }

        public List<LeaveTypeViewModel> LeaveTypes { get; private set; } = new();
        public string Message { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            LeaveTypes = await LeaveTypeService.GetLeaveTypes();
        }

        protected void CreateLeaveType()
        {
            NavigationManager.NavigateTo("/leavetypes/create/");
        }


        protected void EditLeaveType(int id)
        {
            NavigationManager.NavigateTo($"/leavetypes/edit/{id}");
        }

        protected void DetailsLeaveType(int id)
        {
            NavigationManager.NavigateTo($"/leavetypes/details/{id}");
        }

        protected async Task DeleteLeaveType(int id)
        {
            var response = await LeaveTypeService.DeleteLeaveType(id);
            if (response.Success)
            {
                //toastService.ShowSuccess("Leave Type deleted Successfully");
                await OnInitializedAsync();
            }
            else
            {
                Message = response.Message;
            }
        }

    }
}