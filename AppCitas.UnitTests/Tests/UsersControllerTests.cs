using AppCitas.Service.DTOs;
using AppCitas.UnitTests.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xunit;

namespace AppCitas.UnitTests.Tests
{
    public class UsersControllerTests
    {
        private string apiRoute = "api/users";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private string registeredObject;
        private HttpContent httpContent;

        public UsersControllerTests()
        {
            _client = TestHelper.Instance.Client;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Theory]
        [InlineData("OK", "rosa", "Pa$$w0rd")]
        public async Task GetUsersNoPagination_OK(string statusCode, string username, string password)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.GetAsync(requestUri);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "rosa", "Pa$$w0rd", 1, 10)]
        public async Task GetUsersWithPagination_OK(string statusCode, string username, string password, int pageSize, int pageNumber)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            requestUri = $"{apiRoute}" + "?pageNumber=" + pageSize + "&pageSize" + pageNumber;

            // Act
            httpResponse = await _client.GetAsync(requestUri);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "todd", "Pa$$w0rd")]
        public async Task GetUserByUsername_OK(string statusCode, string username, string password)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            requestUri = $"{apiRoute}/" + username;

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NoContent", "lisa", "Pa$$w0rd", "IntroductionU", "LookingForU", "InterestsU", "CityU", "CountryU")]
        public async Task UpdateUser_NoContent(string statusCode, string username, string password, string introduction, string lookingFor, string interests, string city, string country)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            var memberUpdateDto = new MemberUpdateDto
            {
                Introduction = introduction,
                LookingFor = lookingFor,
                Interests = interests,
                City = city,
                Country = country
            };
            registeredObject = GetRegisterObject(memberUpdateDto);
            httpContent = GetHttpContent(registeredObject);

            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.PutAsync(requestUri, httpContent);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("Created", "wagner", "Pa$$w0rd", "../../../a.jpg")]
        public async Task AddPhoto_Created(string statusCode, string username, string password, string file)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent(file);
            form.Add(content, file);
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Console.Write(string.Format("args1: {0}", storageFolder));
            StorageFile sampleFile = await storageFolder.GetFileAsync(file);
            var stream = await sampleFile.OpenStreamForReadAsync();
            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = sampleFile.Name
            };
            form.Add(content);

            requestUri = $"{apiRoute}" + "/add-photo";

            // Act
            httpResponse = await _client.PostAsync(requestUri, form);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NoContent", "tanner", "Pa$$w0rd", "../../../b.jpg")]
        public async Task SetMainPhoto_OK(string statusCode, string username, string password, string file)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent(file);
            form.Add(content, file);
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Console.Write(string.Format("args1: {0}", storageFolder));
            StorageFile sampleFile = await storageFolder.GetFileAsync(file);
            var stream = await sampleFile.OpenStreamForReadAsync();
            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = sampleFile.Name
            };
            form.Add(content);

            requestUri = $"{apiRoute}" + "/add-photo";

            // Act
            var result = await _client.PostAsync(requestUri, form);
            var messageJson = await result.Content.ReadAsStringAsync();
            var message = messageJson.Split(',');
            var id = message[0].Split("\"")[2].Split(":")[1];



            requestUri = $"{apiRoute}" + "/set-main-photo/" + id;

            // Act
            httpResponse = await _client.PutAsync(requestUri, null);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", "../../../c.jpg")]
        public async Task DeletePhoto_OK(string statusCode, string username, string password, string file)
        {
            // Arrange
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent(file);
            form.Add(content, file);
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Console.Write(string.Format("args1: {0}", storageFolder));
            StorageFile sampleFile = await storageFolder.GetFileAsync(file);
            var stream = await sampleFile.OpenStreamForReadAsync();
            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = sampleFile.Name
            };
            form.Add(content);

            requestUri = $"{apiRoute}" + "/add-photo";

            // Act
            var result = await _client.PostAsync(requestUri, form);
            var messageJson = await result.Content.ReadAsStringAsync();
            var message = messageJson.Split(',');
            var id = message[0].Split("\"")[2].Split(":")[1];


            requestUri = $"{apiRoute}" + "/delete-photo/" + id;

            // Act
            httpResponse = await _client.DeleteAsync(requestUri);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NotFound", "caroline", "Pa$$w0rd", "20")]
        public async Task DeletePhoto_NotFound(string statusCode, string username, string password, string id)
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;
            var user = await LoginHelper.LoginUser(username, password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);



            requestUri = $"{apiRoute}" + "/delete-photo/" + id;

            // Act
            httpResponse = await _client.DeleteAsync(requestUri);
            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods
        private static string GetRegisterObject(MemberUpdateDto memberUpdateDto)
        {
            var entityObject = new JObject()
            {
                { nameof(memberUpdateDto.Introduction), memberUpdateDto.Introduction },
                { nameof(memberUpdateDto.LookingFor), memberUpdateDto.LookingFor },
                { nameof(memberUpdateDto.Interests), memberUpdateDto.Interests },
                { nameof(memberUpdateDto.City), memberUpdateDto.City },
                { nameof(memberUpdateDto.Country), memberUpdateDto.Country }
            };
            return entityObject.ToString();
        }
        private static string GetRegisterObject(string file)
        {
            var entityObject = new JObject()
            {
                { "File", file}
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