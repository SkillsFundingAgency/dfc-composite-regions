using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Common.Standard.Logging;
using DFC.Composite.Regions.Models;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.IntegrationTests.FunctionsTests
{
    [TestFixture]
    public class DeleteRegionHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.Delete")]
        public async Task DeleteRegionHttpTrigger_ReturnsStatusCodeOk_WhenRegionExists()
        {
            // arrange
            const string path = ValidPathValue + "Delete";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var regionModel = new Region()
            {
                Path = path,
                PageRegion = pageRegion
            };
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            _ = await regionService.CreateAsync(regionModel);

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Delete")]
        public async Task DeleteRegionHttpTrigger_ReturnsStatusCodeNoContent_WhenRegionDoesNotExist()
        {
            // arrange
            const string path = ValidPathNoContentValue;
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.NoContent;

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Delete")]
        public async Task DeleteRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsInvalid()
        {
            // arrange
            const string path = null;
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Delete")]
        public async Task DeleteRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenBadPathUrl()
        {
            // arrange
            const string path = InvalidPathValue;
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Delete")]
        public async Task DeleteRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsNone()
        {
            // arrange
            const string path = ValidPathValue;
            const PageRegions pageRegion = PageRegions.None;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Delete")]
        public async Task DeleteRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsInvalid()
        {
            // arrange
            const string path = ValidPathValue;
            const int pageRegion = -1;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            // act
            var result = await RunFunctionAsync(path, pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        #region function runner method

        private async Task<HttpResponseMessage> RunFunctionAsync(string path, int pageRegion)
        {
            var request = serviceProvider.GetService<DefaultHttpRequest>();
            var log = serviceProvider.GetService<ILogger>();
            var loggerHelper = serviceProvider.GetService<ILoggerHelper>();
            var httpRequestHelper = serviceProvider.GetService<IHttpRequestHelper>();
            var httpResponseMessageHelper = serviceProvider.GetService<IHttpResponseMessageHelper>();
            var jsonHelper = serviceProvider.GetService<IJsonHelper>();
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            var response = await DFC.Composite.Regions.Functions.DeleteRegionHttpTrigger.Run(
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
