using System;
using System.Threading.Tasks;
using DFC.Composite.Regions.Models;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Tests.ServicesTests
{
    [TestFixture]
    public class RegionServiceGetByIdTests : RegionServiceTestsBase
    {

        [Test]
        [Category("Service.GetById")]
        public async Task GetByIdAsyncTest()
        {
            // arrange
            const string path = ValidPathValue + "GetById";
            const PageRegions pageRegion = PageRegions.Body;
            var regionModel = new Region()
            {
                DocumentId = new Guid(),
                Path = path,
                PageRegion = pageRegion
            };

            _documentDbProvider.GetRegionByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult(regionModel).Result);

            // act
            var result = await _regionService.GetByIdAsync(regionModel.DocumentId.Value);

            // assert
            result.Should().NotBeNull();
            result.Path.Should().Be(path);
            result.PageRegion.Should().Be(pageRegion);
        }

        [Test]
        [Category("Service.Get")]
        public async Task GetByIdAsyncTest_BadId()
        {
            // arrange
            _documentDbProvider.GetRegionByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<Region>(null).Result);

            // act
            var result = await _regionService.GetByIdAsync(new Guid());

            // assert
            result.Should().BeNull();
        }
    }
}
