using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Tests.FunctionsTests
{
    [TestFixture]
    public class PostRegionHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeOk_ForNewRegion()
        {
            // arrange
            const string path = ValidPathValue + "_Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var responseModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _regionService.CreateAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.Ok(Arg.Any<string>()).Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeOk_ForNewRegion_WithPlaceHolder()
        {
            // arrange
            const string path = ValidPathValue + "_Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var responseModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValueWithPlaceHolder,
                OfflineHtml = ValidHtmlFragment
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _regionService.CreateAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.Ok(Arg.Any<string>()).Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsNull()
        {
            // arrange
            const string path = NullPathValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

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

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenBadPathUrl()
        {
            // arrange
            const string path = NullPathValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeUnprocessableEntity_WhenMissingBody()
        {
            // arrange
            const string path = ValidPathValue + "_Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.UnprocessableEntity;

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Throws(new JsonException());

            _regionService.CreateAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult<Regions.Models.Region>(null).Result);

            _httpResponseMessageHelper.UnprocessableEntity(Arg.Any<JsonException>()).Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathDoesNotMatchRoute()
        {
            // arrange
            const string path = ValidPathValue + "_Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var responseModel = new Regions.Models.Region()
            {
                Path = path + "XXXX",
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsNone()
        {
            // arrange
            const string path = ValidPathValue + "_Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var responseModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.None,
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsInvalid()
        {
            // arrange
            const string path = ValidPathValue + "_Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var responseModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = (PageRegions)(-1),
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenBadPathUrlInBody()
        {
            // arrange
            const string path = ValidPathValue + "_Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var responseModel = new Regions.Models.Region()
            {
                Path = NullPathValue,
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _regionService.CreateAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Post")]
        public async Task PostRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenMalformedHtml()
        {
            // arrange
            const string path = ValidPathValue + "_Post";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var responseModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = PageRegions.Body,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = MalformedHtmlFragment
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _regionService.CreateAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        #region function runner method

        private async Task<HttpResponseMessage> RunFunctionAsync(string path)
        {
            var response = await DFC.Composite.Regions.Functions.PostRegionHttpTrigger.Run(
                                                                                                _request,
                                                                                                _log,
                                                                                                path,
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
