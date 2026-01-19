using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Identity;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages
{
    public partial class Register
    {
        private RegisterViewModel Model { get; set; }
        public string Message { get; set; } = string.Empty;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        private IAuthenticationService _authenticationService { get; set; }

        protected override void OnInitialized()
        {
            Model = new RegisterViewModel();
        }

        protected async Task HandleRegister()
        {
            var result = await _authenticationService.RegisterAsync(Model.FirstName, Model.LastName, Model.UserName, Model.Email, Model.Password);
            
            if(result)
            {
                NavigationManager.NavigateTo("/");
            }

            Message = "Something went wrong. Please try again.";
        }
    }
}