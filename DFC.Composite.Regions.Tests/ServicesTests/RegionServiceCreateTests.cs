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
    public class RegionServiceCreateTests : RegionServiceTestsBase
    {

        [Test]
        [Category("Service.Create")]
        public async Task CreateAsyncTest()
        {
            // arrange
            const string path = ValidPathValue + "Create";
            const PageRegions pageRegion = PageRegions.Body;
            var regionModel = new Region()
            {
                Path = path,
                PageRegion = pageRegion
            };
            var resourceResponse = MockResourceResponse(HttpStatusCode.Created);

            _documentDbProvider.CreateRegionAsync(Arg.Any<Region>()).Returns(Task.FromResult(resourceResponse).Result);

            // act
            var result = await _regionService.CreateAsync(regionModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Models.Region>(result);
        }

    }
}
