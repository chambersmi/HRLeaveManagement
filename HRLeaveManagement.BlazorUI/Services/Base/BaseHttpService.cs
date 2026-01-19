using Blazored.LocalStorage;
using HRLeaveManagement.BlazorUI.Services.Base;
using System.Net.Http.Headers;

namespace HRLeaveManagement.BlazorUI.Services.Base
{
    public class BaseHttpService
    {
        protected IClient _client;
        protected readonly ILocalStorageService _localStorage;

        public BaseHttpService(IClient client, ILocalStorageService localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }

        // Cleans up runtime errors
        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
        {
            if (ex.StatusCode == 400)
            {
                return new Response<Guid>()
                {
                    Message = "Invalid data was submitted",
                    ValidationErrors = ex.Response,
                    Success = false
                };
            }
            else if (ex.StatusCode == 404)
            {
                return new Response<Guid>()
                {
                    Message = "The record was not found",
                    ValidationErrors = ex.Response,
                    Success = false
                };
            }
            else
            {
                return new Response<Guid>()
                {
                    Message = "Something went wrong, please try again.",
                    ValidationErrors = ex.Response,
                    Success = false
                };
            }
        }

        protected async Task AddBearerToken()
        {
            if(_localStorage == null)
            {
                return;
            }

            // If key is present, send over bearer token and fetch value
            if (await _localStorage.ContainKeyAsync("token"))
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _localStorage.GetItemAsync<string>("token"));
            }
        }
    }
}
