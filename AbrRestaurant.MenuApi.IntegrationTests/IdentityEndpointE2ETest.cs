using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity;
using AbrRestaurant.MenuApi.Contracts.V1.Resources.Identity.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
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
        public async Task ClientCanSignUpNewApplicationUserWithCorrectInput()
        {
            var requestBody = new UserSignUpRequest() 
            { 
                Email = "developer@gmail.com", 
                Username = "developer",
                Password = "123!@#qweQWE"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var endpointUri = IdentityResourceRoutesV1.IdentityResource.SignUp;

            var request = await _httpClient.PostAsync(endpointUri, content);
            Assert.NotNull(request);
        }
    }
}
