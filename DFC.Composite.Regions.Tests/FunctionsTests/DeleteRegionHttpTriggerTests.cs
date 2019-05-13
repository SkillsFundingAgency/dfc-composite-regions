using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Tests.FunctionsTests
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
            var responseModel = new Regions.Models.Region()
            {
                DocumentId = new Guid()
            };

            _regionService.GetAsync(Arg.Any<string>(), Arg.Any<PageRegions>()).Returns(Task.FromResult(responseModel).Result);
            _regionService.DeleteAsync(Arg.Any<Guid>()).Returns(Task.FromResult(true).Result);

            _httpResponseMessageHelper.Ok().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

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

            _regionService.GetAsync(Arg.Any<string>(), Arg.Any<PageRegions>()).Returns(Task.FromResult<Regions.Models.Region>(null).Result);

            _httpResponseMessageHelper.NoContent().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

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

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

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

            _httpResponseMessageHelper.BadRequest().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

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
            const string path = ValidPathValue + "Delete";
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
        [Category("HttpTrigger.Delete")]
        public async Task DeleteRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsInvalid()
        {
            // arrange
            const string path = ValidPathValue + "Delete";
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
            var response = await DFC.Composite.Regions.Functions.DeleteRegionHttpTrigger.Run(
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
