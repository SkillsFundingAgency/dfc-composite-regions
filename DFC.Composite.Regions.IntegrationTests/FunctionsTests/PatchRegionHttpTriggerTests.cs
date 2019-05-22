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
    public class PatchRegionHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeOk_WhenRegionExists()
        {
            // arrange
            const string path = ValidPathValue + "Patch";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var regionModel = new Region()
            {
                Path = path,
                PageRegion = pageRegion
            };
            var regionPatchModel = new RegionPatch()
            {
                IsHealthy = !regionModel.IsHealthy
            };
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            _ = await regionService.CreateAsync(regionModel);

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion, regionPatchModel);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
            var content = await result.Content.ReadAsStringAsync();
            var responseItem = JsonConvert.DeserializeObject<Region>(content);
            responseItem.Path.Should().Be(path);
            responseItem.PageRegion.Should().Be(pageRegion);
            responseItem.IsHealthy.Should().Be(regionPatchModel.IsHealthy);
        }

        [Test]
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeUnprocessableEntity_WhenMissingBody()
        {
            // arrange
            const string path = ValidPathValue + "Patch";
            const PageRegions pageRegion = PageRegions.Breadcrumb;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.UnprocessableEntity;
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
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeNoContent_WhenRegionDoesNotExist()
        {
            // arrange
            const string path = ValidPathValue + "Patch";
            const PageRegions pageRegion = PageRegions.SidebarLeft;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.NoContent;

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsInvalid()
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
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsNone()
        {
            // arrange
            const string path = ValidPathValue + "Patch";
            const PageRegions pageRegion = PageRegions.None;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsInvalid()
        {
            // arrange
            const string path = ValidPathValue + "Patch";
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

            var response = await Functions.PatchRegionHttpTrigger.Run(
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

        private async Task<HttpResponseMessage> RunFunctionAsync(string path, int pageRegion, RegionPatch regionPatchModel)
        {
            var request = serviceProvider.GetService<DefaultHttpRequest>();
            var log = serviceProvider.GetService<ILogger>();
            var loggerHelper = serviceProvider.GetService<ILoggerHelper>();
            var httpRequestHelper = serviceProvider.GetService<IHttpRequestHelper>();
            var httpResponseMessageHelper = serviceProvider.GetService<IHttpResponseMessageHelper>();
            var jsonHelper = serviceProvider.GetService<IJsonHelper>();
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            request.Body = MemoryStreamFromObject(regionPatchModel);

            var response = await Functions.PatchRegionHttpTrigger.Run(
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
