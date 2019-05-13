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
    public class RegionServicePatchTests : RegionServiceTestsBase
    {

        [Test]
        [Category("Service.Patch")]
        public async Task PatchAsyncTest()
        {
            // arrange
            const string path = ValidPathValue + "Patch";
            const PageRegions pageRegion = PageRegions.Body;
            var regionModel = new Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion
            };
            var regionPatchModel = new RegionPatch()
            {
                IsHealthy = !regionModel.IsHealthy
            };

            var resourceResponse = MockResourceResponse(HttpStatusCode.OK);

            _documentDbProvider.UpdateRegionAsync(Arg.Any<Region>()).Returns(Task.FromResult(resourceResponse).Result);

            // act
            var result = await _regionService.PatchAsync(regionModel, regionPatchModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Models.Region>(result);
        }

    }
}
