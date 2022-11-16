using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppCitas.UnitTests.Test
{
    public class AccountControllerTests
    {
        private string apiRoute = "api/account";
        private readonly HttpClient _client;
        private HttpResponseMessage? httpResponse;
        private string requestUri = String.Empty;
        private string registerObject = String.Empty;
        private string loginObjetct = String.Empty;
        private HttpContent? httpContent;

        public AccountControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("BadRequest", "lisa", "KnownAs", "Gender", "2000-01-01", "City", "Country", "Password")]
        public async Task Register_ShouldBadRequest(string statusCode, string username, string knownAs, string gender, DateTime dateOfBirth, string city, string country, string password)
        {
            // Arrange
            requestUri = $"{apiRoute}/register";
            var registerDto = new RegisterDto
            {
                Username = username,
                KnownAs = knownAs,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                City = city,
                Country = country,
                Password = password
            };

            registerObject = GetRegisterObject(registerDto);
            httpContent = GetHttpContent(registerObject);

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "arturo", "Arturo", "male", "2000-01-01", "Aguascalientes", "Mexico", "Pa$$w0rd")]
        public async Task Register_ShouldOk(string statusCode, string username, string knownAs, string gender, DateTime dateOfBirth, string city, string country, string password)
        {
            // Arrange
            requestUri = $"{apiRoute}/register";
            var registerDto = new RegisterDto
            {
                Username = username + Guid.NewGuid().ToString(),
                KnownAs = knownAs,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                City = city,
                Country = country,
                Password = password
            };

            registerObject = GetRegisterObject(registerDto);
            httpContent = GetHttpContent(registerObject);

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("Unauthorized", "lisa", "password")]
        public async Task Login_ShouldUnauthorized(string statusCode, string username, string password)
        {
            // Arrange
            requestUri = $"{apiRoute}/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            loginObjetct = GetRegisterObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd")]
        public async Task Login_ShouldOK(string statusCode, string username, string password)
        {
            // Arrange
            requestUri = $"{apiRoute}/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };
            loginObjetct = GetRegisterObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods

        private static string GetRegisterObject(RegisterDto registerDto)
        {
            var entityObject = new JObject()
            {
                { nameof(registerDto.Username), registerDto.Username },
                { nameof(registerDto.KnownAs), registerDto.KnownAs },
                { nameof(registerDto.Gender), registerDto.Gender },
                { nameof(registerDto.DateOfBirth), registerDto.DateOfBirth },
                { nameof(registerDto.City), registerDto.City },
                { nameof(registerDto.Country), registerDto.Country },
                { nameof(registerDto.Password), registerDto.Password }
            };

            return entityObject.ToString();
        }

        private static string GetRegisterObject(LoginDto loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
            return entityObject.ToString();
        }

        private StringContent GetHttpContent(string objectToEncode)
        {
            return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
