using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.ViewModels.Identity;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages
{
    public partial class Login
    {
        public LoginViewModel Model { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public string Message { get; set; } = string.Empty;
        [Inject]
        private IAuthenticationService AuthenticationService { get; set; }

        public Login()
        {
            
        }

        protected override void OnInitialized()
        {
            Model = new LoginViewModel();
        }

        protected async Task HandleLogin()
        {
            if(await AuthenticationService.AuthenticateAsync(Model.Email, Model.Password))
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Message = "Username/password combination invalid.";
            }
                
        }
    }
}