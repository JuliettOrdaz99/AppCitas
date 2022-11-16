using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AppCitas.UnitTests.Test
{
    public class BuggyControllerTests
    {
        private string apiRoute = "api/buggy";
        private readonly HttpClient _client;
        private HttpResponseMessage? httpResponse;
        private string requestUrl = String.Empty;
        private string loginObjetct = String.Empty;
        private HttpContent? httpContent;

        public BuggyControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd")]
        public async Task GetSecret_ShouldOK(string statusCode, string username, string password)
        {
            // Arrange
            requestUrl = "api/account/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            loginObjetct = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            var reponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            requestUrl = $"{apiRoute}/auth";

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("not-found", "NotFound")]
        [InlineData("server-error", "InternalServerError")]
        [InlineData("bad-request", "BadRequest")]
        public async Task GetEndpoints_ShouldValidate(string endpoint, string statusCode)
        {
            // Arrange
            requestUrl = $"{apiRoute}/{endpoint}";

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NotFound")]
        public async Task GetNotFound_ShouldNotFound(string statusCode)
        {
            // Arrange
            requestUrl = $"{apiRoute}/not-found";

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("InternalServerError")]
        public async Task GetServerError_ShouldNotInternalServerError(string statusCode)
        {
            // Arrange
            requestUrl = $"{apiRoute}/server-error";

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("BadRequest")]
        public async Task GetBadRequest_ShouldBadRequest(string statusCode)
        {
            // Arrange
            requestUrl = $"{apiRoute}/bad-request";

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods

        private static string GetLoginObject(LoginDto loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };

            return entityObject.ToString();
        }

        private static StringContent GetHttpContent(string objectToCode)
        {
            return new StringContent(objectToCode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
