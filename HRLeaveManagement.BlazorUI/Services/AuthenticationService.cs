using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Providers;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;
using HRLeaveManagement.BlazorUI.ViewModels.Identity;
using Microsoft.AspNetCore.Components.Authorization;

namespace HRLeaveManagement.BlazorUI.Services
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(
            IClient client, 
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider) : base(client, localStorage)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        //Crashes
        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            try
            {
                AuthRequest authenticationRequest = new AuthRequest() { Email = email, Password = password };
                var authenticationResponse = await _client.LoginAsync(authenticationRequest);
                Console.WriteLine("AUTHENTICATE ASYNC TOKEN: " + authenticationResponse.Token);

                if (!string.IsNullOrWhiteSpace(authenticationResponse.Token))
                {
                    await _localStorage.SetItemAsync("token", authenticationResponse.Token);

                    // Set claims in Blazor and login state
                    // Crashes inside the .LoggedIn() method
                    await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task Logout()
        {
            // Remove Claims in Blazor and Invalidate Login State
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
        {
            RegistrationRequest registrationRequest = new RegistrationRequest() { FirstName = firstName, LastName = lastName, Email = email, UserName = userName, Password = password };
            var response = await _client.RegisterAsync(registrationRequest);

            if (!string.IsNullOrEmpty(response.UserId))
            {
                return true;
            }
            return false;
        }
    }
}