using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.Composite.Regions.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Tests.ServicesTests
{
    [TestFixture]
    public class RegionServiceGetListTests : RegionServiceTestsBase
    {

        [Test]
        [Category("Service.GetList")]
        public async Task GetListAsyncTest_ReturnsSuccess_WhenRegionsExist()
        {
            // arrange
            const string path = ValidPathValue + "_GetList";
            var regionModels = new List<Region>() {
                new Region() {
                    Path = path,
                    PageRegion = PageRegions.Body
                },
                new Region()
                {
                    Path = path,
                    PageRegion = PageRegions.Breadcrumb
                }
            };

            _documentDbProvider.GetRegionsForPathAsync(Arg.Any<string>()).Returns(Task.FromResult(regionModels).Result);

            // act
            var results = await _regionService.GetListAsync(path);

            // assert
            results.Should().NotBeNull();
            results.Count().Should().BeGreaterOrEqualTo(2);
            results[0].Path.Should().Be(regionModels[0].Path);
            results[0].PageRegion.Should().Be(regionModels[0].PageRegion);
            results[1].Path.Should().Be(regionModels[1].Path);
            results[1].PageRegion.Should().Be(regionModels[1].PageRegion);
        }

    }
}
