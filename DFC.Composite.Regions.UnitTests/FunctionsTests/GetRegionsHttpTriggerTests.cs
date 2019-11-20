using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace DFC.Composite.Regions.Tests.FunctionsTests
{
    [TestFixture]
    public class GetRegionsHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.GetList")]
        public async Task GetRegionsHttpTrigger_ReturnsStatusCodeOk_WhenRegionsExist()
        {
            // arrange
            const string path = ValidPathValue + "_GetList";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var responseModels = new List<Regions.Models.Region>(){
                new Regions.Models.Region(),
                new Regions.Models.Region()
            };

            _regionService.GetListAsync(Arg.Any<string>()).Returns(Task.FromResult(responseModels).Result);

            _httpResponseMessageHelper.Ok(Arg.Any<string>()).Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.GetList")]
        public async Task GetRegionsHttpTrigger_ReturnsStatusCodeNoContent_WhenNoRegionsExist()
        {
            // arrange
            const string path = ValidPathValue + "_GetList";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.NoContent;

            _httpResponseMessageHelper.NoContent().Returns(x => new HttpResponseMessage(expectedHttpStatusCode));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.GetList")]
        public async Task GetRegionsHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsNull()
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

        #region function runner method

        private async Task<HttpResponseMessage> RunFunctionAsync(string path)
        {
            var response = await DFC.Composite.Regions.Functions.GetRegionsHttpTrigger.Run(
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
