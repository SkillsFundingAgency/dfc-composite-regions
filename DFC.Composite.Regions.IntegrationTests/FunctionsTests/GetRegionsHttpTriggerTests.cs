using System.Collections.Generic;
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

namespace DFC.Composite.Regions.IntegrationTests.FunctionsTests
{
    [TestFixture]
    public class GetRegionsHttpTriggerTests : FunctionsTestsBase
    {

        [Test]
        [Category("HttpTrigger.GetList")]
        public async Task GetRegionsHttpTrigger_ReturnsStatusCodeOk_WhenRegionsExist()
        {
            // arrange
            const string path = ValidPathValue + "GetList";
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.OK;
            var regionModels = new List<Region>() {
                new Region() {
                    Path = path,
                    PageRegion = Constants.PageRegions.Body
                },
                new Region()
                {
                    Path = path,
                    PageRegion = Constants.PageRegions.Breadcrumb
                }
            };
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            regionModels.ForEach(async f => _ = await regionService.CreateAsync(f));

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
            var content = await result.Content.ReadAsStringAsync();
            var responseItems = JsonConvert.DeserializeObject<List<Region>>(content);
            responseItems.Count.Should().BeGreaterOrEqualTo(regionModels.Count);
            responseItems[0].Path.Should().Be(regionModels[0].Path);
            responseItems[0].PageRegion.Should().Be(regionModels[0].PageRegion);
            responseItems[1].Path.Should().Be(regionModels[1].Path);
            responseItems[1].PageRegion.Should().Be(regionModels[1].PageRegion);
        }

        [Test]
        [Category("HttpTrigger.GetList")]
        public async Task GetRegionsHttpTrigger_ReturnsStatusCodeNoContent_WhenNoRegionsExist()
        {
            // arrange
            const string path = ValidPathNoContentValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.NoContent;

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        [Test]
        [Category("HttpTrigger.GetList")]
        public async Task GetRegionsHttpTrigger_ReturnsStatusCodeBadRequest_WhenPathIsInvalid()
        {
            // arrange
            const string path = InvalidPathValue;
            const HttpStatusCode expectedHttpStatusCode = HttpStatusCode.BadRequest;

            // act
            var result = await RunFunctionAsync(path);

            // assert
            Assert.IsInstanceOf<HttpResponseMessage>(result);
            Assert.AreEqual(expectedHttpStatusCode, result.StatusCode);
        }

        #region function runner method

        private async Task<HttpResponseMessage> RunFunctionAsync(string path)
        {
            var request = serviceProvider.GetService<DefaultHttpRequest>();
            var log = serviceProvider.GetService<ILogger>();
            var loggerHelper = serviceProvider.GetService<ILoggerHelper>();
            var httpRequestHelper = serviceProvider.GetService<IHttpRequestHelper>();
            var httpResponseMessageHelper = serviceProvider.GetService<IHttpResponseMessageHelper>();
            var jsonHelper = serviceProvider.GetService<IJsonHelper>();
            var regionService = serviceProvider.GetService<Services.IRegionService>();

            var response = await DFC.Composite.Regions.Functions.GetRegionsHttpTrigger.Run(
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
