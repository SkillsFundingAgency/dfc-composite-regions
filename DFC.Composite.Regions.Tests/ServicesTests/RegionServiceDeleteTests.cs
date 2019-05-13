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
    public class RegionServiceDeleteTests : RegionServiceTestsBase
    {

        [Test]
        [Category("Service.Delete")]
        public async Task DeleteAsyncTest()
        {
            // arrange
            const string path = ValidPathValue + "Delete";
            const PageRegions pageRegion = PageRegions.Body;
            var regionModel = new Region()
            {
                DocumentId=new Guid(),
                Path = path,
                PageRegion = pageRegion
            };

            var resourceResponse = MockResourceResponse(HttpStatusCode.NoContent);

            _documentDbProvider.DeleteRegionAsync(Arg.Any<Guid>()).Returns(Task.FromResult(resourceResponse).Result);

            // act
            var result = await _regionService.DeleteAsync(regionModel.DocumentId.Value);

            // assert
            result.Should().BeTrue();
        }

    }
}
