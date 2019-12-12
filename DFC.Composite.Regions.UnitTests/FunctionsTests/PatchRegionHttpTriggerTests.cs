using DFC.Composite.Regions.Models;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Tests.FunctionsTests
{
    [TestFixture]
    public class PatchRegionHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeOk_WhenRegionExists()
        {
            // arrange
            const string path = ValidPathValue + "_Patch";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = Guid.NewGuid(),
                Path = "a-path",
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValueWithPlaceHolder,
            };
            var regionPatchModel = new JsonPatchDocument<Region>();

            regionPatchModel.Add(x => x.HealthCheckRequired, true);

            _httpRequestHelper.GetResourceFromRequest<JsonPatchDocument<Region>>(_request).Returns(Task.FromResult(regionPatchModel).Result);
            _regionService.GetAsync(Arg.Any<string>(), Arg.Any<PageRegions>()).Returns(Task.FromResult(responseModel).Result);
            _regionService.ReplaceAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.Ok(Arg.Any<string>()).Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenMissingBody()
        {
            // arrange
            const string path = ValidPathValue + "_Patch";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = Guid.NewGuid(),
                Path = "a-path",
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValueWithPlaceHolder,
            };

            _httpRequestHelper.GetResourceFromRequest<JsonPatchDocument<Region>>(_request).Returns(Task.FromResult(default(JsonPatchDocument<Region>)).Result);
            _regionService.GetAsync(Arg.Any<string>(), Arg.Any<PageRegions>()).Returns(Task.FromResult(responseModel).Result);
            _regionService.ReplaceAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

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
            const string path = ValidPathNoContentValue;
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.NoContent;
            var regionPatchModel = new JsonPatchDocument<Region>();

            _httpRequestHelper.GetResourceFromRequest<JsonPatchDocument<Region>>(_request).Returns(Task.FromResult(regionPatchModel).Result);
            _regionService.GetAsync(Arg.Any<string>(), Arg.Any<PageRegions>()).Returns(Task.FromResult<Regions.Models.Region>(null).Result);

            _httpResponseMessageHelper.NoContent().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Patch")]
        public async Task PatchRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsNull()
        {
            // arrange
            const string path = NullPathValue;
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

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
            const string path = ValidPathValue + "_Patch";
            const PageRegions pageRegion = PageRegions.None;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

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
            const string path = ValidPathValue + "_Patch";
            const int pageRegion = -1;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        #region function runner method

        private async Task<HttpResponseMessage> RunFunctionAsync(string path, int pageRegion)
        {
            var response = await DFC.Composite.Regions.Functions.PatchRegionHttpTrigger.Run(
                                                                                                _request,
                                                                                                _log,
                                                                                                path,
                                                                                                pageRegion,
                                                                                                _loggerHelper,
                                                                                                _httpRequestHelper,
                                                                                                _httpResponseMessageHelper,
                                                                                                _jsonHelper,
                                                                                                _regionService
                                                                                            ).ConfigureAwait(false);

            return response;
        }

        #endregion

    }
}
