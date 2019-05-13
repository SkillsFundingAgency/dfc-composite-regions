using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Tests.FunctionsTests
{
    [TestFixture]
    public class GetRegionHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.Get")]
        public async Task GetRegionHttpTrigger_ReturnsStatusCodeOk_WhenRegionExists()
        {
            // arrange
            const string path = ValidPathValue + "Get";
            const PageRegions pageRegion = PageRegions.Body;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var responseModel = new Regions.Models.Region();

            _regionService.GetAsync(Arg.Any<string>(), Arg.Any<PageRegions>()).Returns(Task.FromResult(responseModel).Result);

            _httpResponseMessageHelper.Ok(Arg.Any<string>()).Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path, (int)pageRegion);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.Get")]
        public async Task GetRegionHttpTrigger_ReturnsStatusCodeNoContent_WhenRegionDoesNotExist()
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
        [Category("HttpTrigger.Get")]
        public async Task GetRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsInvalid()
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
        [Category("HttpTrigger.Get")]
        public async Task GetRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenBadPathUrl()
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
        [Category("HttpTrigger.Get")]
        public async Task GetRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsNone()
        {
            // arrange
            const string path = ValidPathValue + "Get";
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
        [Category("HttpTrigger.Get")]
        public async Task GetRegionHttpTrigger_ReturnsStatusCodeBadRequest_WhenPageRegionIsInvalid()
        {
            // arrange
            const string path = ValidPathValue + "Get";
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
            var response = await DFC.Composite.Regions.Functions.GetRegionHttpTrigger.Run(
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
