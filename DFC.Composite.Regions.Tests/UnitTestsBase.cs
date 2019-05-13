using NUnit.Framework;

namespace DFC.Composite.Regions.Tests
{
    [SetUpFixture]
    public abstract class UnitTestsBase
    {
        protected const string ValidPathValue = "https://nationalcareersservice.direct.gov.uk/regions/unittests/";
        protected const string ValidPathNoContentValue = "https://nationalcareersservice.direct.gov.uk/regions/unittests/XXXX";
        protected const string InvalidPathValue = "https//nationalcareersservice.direct.gov.uk/";
        protected const string ValidHtmlFragment = "<H1>Service Unavailable</H1>";
        protected const string MalformedHtmlFragment = "<H1>Service <B>Malformed";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }
    }
}
