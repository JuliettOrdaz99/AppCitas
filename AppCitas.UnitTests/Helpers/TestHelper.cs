using AppCitas.Service;
using System;
using System.Net.Http;

namespace AppCitas.UnitTests.Helpers
{
    public sealed class TestHelper
    {
        private static readonly Lazy<TestHelper> _lazyInstance =
            new Lazy<TestHelper>(() => new TestHelper());

        public static TestHelper Instance
        {
            get
            {
                return _lazyInstance.Value;
            }
        }

        public HttpClient Client { get; set; }

        private TestHelper()
        {
            // Place for instance initialization code
            Client = new APIWebApplicationFactory<Startup>().CreateDefaultClient();
        }
    }
}
