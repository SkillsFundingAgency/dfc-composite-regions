using System;
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
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint=ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _regionService.ReplaceAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.Ok(Arg.Any<string>()).Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

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
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsInvalid()
        {
            // arrange
            const string path = InvalidPathValue;
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
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenBadPathUrl()
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
        [Category("HttpTrigger.Put")]
        public async Task PutRegionHttpTrigger_ReturnsStatusCodeUnprocessableEntity_WhenMissingBody()
        {
            // arrange
            const string path = ValidPathValue + "_Put"; 
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.UnprocessableEntity;

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Throws(new JsonException());

            _regionService.ReplaceAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult<Regions.Models.Region>(null).Result);

            _httpResponseMessageHelper.UnprocessableEntity(Arg.Any<JsonException>()).Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

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
            var responseModel = new Regions.Models.Region()
            {
                Path = path,
                PageRegion = pageRegion,
                OfflineHtml = ValidHtmlFragment,
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

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
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path + "XXXX",
                PageRegion = pageRegion,
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

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
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = PageRegions.SidebarRight,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = ValidHtmlFragment
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

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
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = PageRegions.None,
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

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
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

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
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = NullPathValue,
                PageRegion = pageRegion,
                RegionEndpoint = ValidEndpointValue
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _regionService.ReplaceAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

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
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion,
                RegionEndpoint = ValidEndpointValue,
                OfflineHtml = MalformedHtmlFragment
            };

            _httpRequestHelper.GetResourceFromRequest<Regions.Models.Region>(_request).Returns(Task.FromResult(responseModel).Result);

            _regionService.ReplaceAsync(Arg.Any<Regions.Models.Region>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        #region function runner method

        private async Task<HttpResponseMessage> RunFunctionAsync(string path, int pageRegion)
        {
            var response = await DFC.Composite.Regions.Functions.PutRegionHttpTrigger.Run(
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
