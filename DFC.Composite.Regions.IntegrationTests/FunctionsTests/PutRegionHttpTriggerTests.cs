using System;
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
    public class PutRegionHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeOk_ForUpdatedRegion()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            regionModel = await regionService.CreateAsync(regionModel);

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsNull()
        {
            // arrange
            const string path = NullPathValue;
            const PageRegions pageRegion = PageRegions.Body;
            const string endpoint = ValidEndpointValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = endpoint,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsInvalid ()
        {
            // arrange
            const string path = InvalidPathValue;
            const PageRegions pageRegion = PageRegions.Body;
            const string endpoint = ValidEndpointValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = endpoint,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

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
            const PageRegions pageRegion = PageRegions.Body;
            const string endpoint = null;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = endpoint,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

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
            const PageRegions pageRegion = PageRegions.Body;
            const string endpoint = InvalidEndpointValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = endpoint,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeUnprocessableEntity_WhenMissingBody()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.UnprocessableEntity;

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, null);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenDocumemntIdIsMissing()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathDoesNotMatchRoute()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = ValidPathValue + "_XXXX",
                PageRegion = pageRegion,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionDoesNotMatchRoute()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = PageRegions.SidebarRight,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsNone()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = PageRegions.None;
            const string endpoint = ValidEndpointValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = endpoint,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsInvalid()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = (PageRegions)(-1);
            const string endpoint = ValidEndpointValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = endpoint,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenBadPathUrlInBody()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = NullPathValue,
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenMalformedHtml()
        {
            // arrange
            const string path = ValidPathValue + "_Put";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var regionModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = MalformedHtmlFragment
            };

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        #region function runner method

        private async Task<HttpResponseMessage> RunFunctionAsync(string path, int pageRegion, Regions.Models.Region regionModel)
        {
            var request = serviceProvider.GetService<DefaultHttpRequest>();
            var log = serviceProvider.GetService<ILogger>();
            var loggerHelper = serviceProvider.GetService<ILoggerHelper>();
            var httpRequestHelper = serviceProvider.GetService<IHttpRequestHelper>();
            var httpResponseMessageHelper = serviceProvider.GetService<IHttpResponseMessageHelper>();
            var jsonHelper = serviceProvider.GetService<IJsonHelper>();
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            request.Body = MemoryStreamFromObject(regionModel);

            var response = await DFC.Composite.Regions.Functions.PutRegionHttpTrigger.Run(
                                                                                            request,
                                                                                            log,
                                                                                            path,
                                                                                            pageRegion,
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
