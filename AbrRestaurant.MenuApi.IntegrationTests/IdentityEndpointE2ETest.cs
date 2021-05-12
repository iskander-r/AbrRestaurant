using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Requests;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AbrRestaurant.MenuApi.IntegrationTests
{
    public class IdentityEndpointE2ETest
    {
        private readonly HttpClient _httpClient;
        public IdentityEndpointE2ETest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _httpClient = appFactory.CreateClient();
        }

        [Fact]
        public async Task ClientCanSignUpNewApplicationUserWithCorrectInputAndGetProfileInformation()
        {
            var id = Guid.NewGuid().ToString("n");
            var email = $"email_{id}@restaurant.kz";
            var username = $"user_{id}";
            var password = "123!@#qweQWE";

            var requestBody = new UserSignUpRequest() 
            { 
                Email = email, 
                Username = username,
                Password = password
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var signUpEndpointUri = IdentityResourceRoutesV1.IdentityResource.SignUp;

            var signUpResponse = await _httpClient.PostAsync(signUpEndpointUri, content);

            Assert.True(signUpResponse.StatusCode == HttpStatusCode.OK);

            var token = JsonConvert.DeserializeObject<AuthSuccessResponse>(
                await signUpResponse.Content.ReadAsStringAsync());

            var getProfileEndpointUri = IdentityResourceRoutesV1.IdentityResource.GetProfile;

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token.Token);

            var getProfileResponse = await _httpClient.GetAsync(getProfileEndpointUri);

            Assert.True(getProfileResponse.StatusCode == HttpStatusCode.OK);

            var profile = JsonConvert.DeserializeObject<GetProfileResponse>(
                await getProfileResponse.Content.ReadAsStringAsync());

            Assert.True(profile.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
            Assert.True(profile.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
        }


        [Fact]
        public async Task ClientCanChangePassword()
        {
            var id = Guid.NewGuid().ToString("n");
            var email = $"email_{id}@restaurant.kz";
            var username = $"user_{id}";
            var password = "123!@#qweQWE";

            var requestBody = new UserSignUpRequest()
            {
                Email = email,
                Username = username,
                Password = password
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var signUpEndpointUri = IdentityResourceRoutesV1.IdentityResource.SignUp;

            var signUpResponse = await _httpClient.PostAsync(signUpEndpointUri, content);

            Assert.True(signUpResponse.StatusCode == HttpStatusCode.OK);

            var token = JsonConvert.DeserializeObject<AuthSuccessResponse>(
                await signUpResponse.Content.ReadAsStringAsync());

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token);

            var changePasswordRequestBody = new UserChangePasswordRequest()
            {
                CurrentPassword = password,
                NewPassword = password + "_new"
            };
          
            content = new StringContent(
                JsonConvert.SerializeObject(changePasswordRequestBody), Encoding.UTF8, "application/json");
            var changePasswordRequestUri = IdentityResourceRoutesV1.IdentityResource.ChangePassword;

            var changePasswordResponse = await _httpClient.PostAsync(changePasswordRequestUri, content);

            Assert.True(changePasswordResponse.StatusCode == HttpStatusCode.OK);
        }
    }
}
