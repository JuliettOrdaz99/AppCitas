using AppCitas.Service.DTOs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppCitas.UnitTests.Helpers
{
    public class LoginHelper
    {
        private static string registeredObject;
        private static StringContent httpContent;

        public static async Task<UserDto> LoginUser(string username, string password)
        {
            string requestUri = $"api/account/login";
            HttpClient client = TestHelper.Instance.Client;
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };
            registeredObject = GetLoginObject(loginDto);
            httpContent = GetHttpContent(registeredObject);
            var result = await client.PostAsync(requestUri, httpContent);
            var userJson = await result.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDto>(userJson, new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true
            });
            return user;
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
        public  static StringContent GetHttpContent(string objectToEncode)
        {
            return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
