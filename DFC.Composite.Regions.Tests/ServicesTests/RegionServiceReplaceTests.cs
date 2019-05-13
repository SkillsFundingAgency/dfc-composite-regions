using System;
using System.Net;
using System.Threading.Tasks;
using DFC.Composite.Regions.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Tests.ServicesTests
{
    [TestFixture]
    public class RegionServiceReplaceTests : RegionServiceTestsBase
    {

        [Test]
        [Category("Service.Replace")]
        public async Task ReplaceAsyncTest()
        {
            // arrange
            const string path = ValidPathValue + "Replace";
            const PageRegions pageRegion = PageRegions.Body;
            var regionModel = new Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion
            };

            var resourceResponse = MockResourceResponse(HttpStatusCode.OK);

            _documentDbProvider.UpdateRegionAsync(Arg.Any<Region>()).Returns(Task.FromResult(resourceResponse).Result);

            // act
            var result = await _regionService.ReplaceAsync(regionModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Models.Region>(result);
        }

    }
}
