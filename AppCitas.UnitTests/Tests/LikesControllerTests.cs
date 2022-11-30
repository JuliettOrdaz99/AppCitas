using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AppCitas.UnitTests.Tests
{
    public class LikesControllerTests
    {
        private string apiRoute = "api/likes";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private string registeredObject;
        private HttpContent httpContent;
        public LikesControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("NotFound", "todd", "Pa$$w0rd", "carlos")]
        public async Task AddLike_NotFound(string statusCode, string username, string password, string userLiked)
        {
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


            requestUri = $"{apiRoute}/" + userLiked;

            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }


        [Theory]
        [InlineData("BadRequest", "todd", "Pa$$w0rd", "todd")]
        public async Task AddLike_BadRequest(string statusCode, string username, string password, string userLiked)
        {

            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


            requestUri = $"{apiRoute}/" + userLiked;

            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "rosa", "Pa$$w0rd", "tanner")]
        public async Task AddLike_OK(string statusCode, string username, string password, string userLiked)
        {
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


            requestUri = $"{apiRoute}/" + userLiked;

            httpResponse = await _client.PostAsync(requestUri, httpContent);
            _client.DefaultRequestHeaders.Authorization = null;
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("BadRequest", "rosa", "Pa$$w0rd", "tanner")]
        public async Task AddLike_BadRequest2(string statusCode, string username, string password, string userLiked)
        {
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


            requestUri = $"{apiRoute}/" + userLiked;
            httpResponse = await _client.PostAsync(requestUri, httpContent);
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            _client.DefaultRequestHeaders.Authorization = null;
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "tanner", "Pa$$w0rd")]
        public async Task GetUserLikes_OK(string statusCode, string username, string password)
        {
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);



            requestUri = $"{apiRoute}" + "?predicate=likedBy";

            httpResponse = await _client.GetAsync(requestUri);
            _client.DefaultRequestHeaders.Authorization = null;
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