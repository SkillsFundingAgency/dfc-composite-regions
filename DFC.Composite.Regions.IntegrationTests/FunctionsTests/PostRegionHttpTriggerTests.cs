using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Common.Standard.Logging;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.IntegrationTests.FunctionsTests
{
    [TestFixture]
    public class PostRegionHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeOk_ForNewRegion()
        {
            // arrange
            const string path = ValidPathValue + "Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var regionModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsInvalid()
        {
            // arrange
            const string path = InvalidPathValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenMissingEndpointUrl()
        {
            // arrange
            const string path = ValidPathValue;
            const string endpoint = null;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.Body,
                RegionEndpoint= endpoint,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenBadEndpointUrl()
        {
            // arrange
            const string path = ValidPathValue;
            const string endpoint = InvalidEndpointValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.Body,
                RegionEndpoint= endpoint,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeUnprocessableEntity_WhenMissingBody()
        {
            // arrange
            const string path = ValidPathValue + "Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.UnprocessableEntity;

            // act
            var result = await RunFunctionAsync(path, null);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathDoesNotMatchRoute()
        {
            // arrange
            const string path = ValidPathValue + "Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = path + "XXXX",
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsNone()
        {
            // arrange
            const string path = ValidPathValue + "Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.None,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsInvalid()
        {
            // arrange
            const string path = ValidPathValue + "Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = (PageRegions)(-1),
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenBadPathUrlInBody()
        {
            // arrange
            const string path = ValidPathValue + "Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = InvalidPathValue,
                PageRegion = PageRegions.Breadcrumb,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenMalformedHtml()
        {
            // arrange
            const string path = ValidPathValue + "Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = MalformedHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        #region function runner method

        private async Task<HttpResponseMessage> RunFunctionAsync(string path, Regions.Models.Region regionModel)
        {
            var request = serviceProvider.GetService<DefaultHttpRequest>();
            var log = serviceProvider.GetService<ILogger>();
            var loggerHelper = serviceProvider.GetService<ILoggerHelper>();
            var httpRequestHelper = serviceProvider.GetService<IHttpRequestHelper>();
            var httpResponseMessageHelper = serviceProvider.GetService<IHttpResponseMessageHelper>();
            var jsonHelper = serviceProvider.GetService<IJsonHelper>();
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            request.Body = MemoryStreamFromObject(regionModel);

            var response = await DFC.Composite.Regions.Functions.PostRegionHttpTrigger.Run(
                                                                                                request,
                                                                                                log,
                                                                                                path,
                                                                                                loggerHelper,
                                                                                                httpRequestHelper,
                                                                                                httpResponseMessageHelper,
                                                                                                jsonHelper,
                                                                                                regionService
                                                                                            ).ConfigureAwait(false);

            return response;
        }

        #endregion

    }
}
