using System.Threading.Tasks;
using DFC.Composite.Regions.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Tests.ServicesTests
{
    [TestFixture]
    public class RegionServiceGetTests : RegionServiceTestsBase
    {

        [Test]
        [Category("Service.Get")]
        public async Task GetAsyncTest()
        {
            // arrange
            const string path = ValidPathValue + "Get";
            const PageRegions pageRegion = PageRegions.Body;
            var regionModel = new Region()
            {
                Path = path,
                PageRegion = pageRegion
            };

            _documentDbProvider.GetRegionForPathAsync(Arg.Any<string>(),Arg.Any<PageRegions>()).Returns(Task.FromResult(regionModel).Result);

            // act
            var result = await _regionService.GetAsync(path, pageRegion);

            // assert
            result.Should().NotBeNull();
            result.Path.Should().Be(path);
            result.PageRegion.Should().Be(pageRegion);
        }

    }
}
