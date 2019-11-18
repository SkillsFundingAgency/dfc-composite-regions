using DFC.Common.Standard.Logging;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;

namespace DFC.Composite.Regions.Tests.FunctionsTests
{
    [TestFixture]
    public class FunctionsTestsBase : UnitTestsBase
    {
        protected ILogger _log;
        protected HttpRequest _request;
        protected ILoggerHelper _loggerHelper;
        protected IHttpRequestHelper _httpRequestHelper;
        protected IHttpResponseMessageHelper _httpResponseMessageHelper;
        protected IJsonHelper _jsonHelper;
        protected Services.IRegionService _regionService;

        #region Tests initialisations and cleanup

        [SetUp]
        public void Setup()
        {
            _log = Substitute.For<ILogger>();
            _request = new DefaultHttpRequest(new DefaultHttpContext());
            _loggerHelper = Substitute.For<ILoggerHelper>();
            _httpRequestHelper = Substitute.For<IHttpRequestHelper>();
            _httpResponseMessageHelper = Substitute.For<IHttpResponseMessageHelper>();
            _jsonHelper = Substitute.For<IJsonHelper>();

            _regionService = Substitute.For<Services.IRegionService>();
        }

        [TearDown]
        public void TearDown()
        {
            _log =null;
            _request = null;
            _loggerHelper = null;
            _httpRequestHelper = null;
            _httpResponseMessageHelper = null;
            _jsonHelper = null;

            _regionService = null;
        }

        #endregion
    }
}
