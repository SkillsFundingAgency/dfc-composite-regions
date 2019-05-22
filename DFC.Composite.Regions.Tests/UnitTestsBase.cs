using NUnit.Framework;

namespace DFC.Composite.Regions.Tests
{
    [SetUpFixture]
    public abstract class UnitTestsBase
    {
        protected const string ValidPathValue = "unittests";
        protected const string ValidPathNoContentValue = "unittests/XXXX";
        protected const string InvalidPathValue = null;
        protected const string ValidHtmlFragment = "<H1>Service Unavailable</H1>";
        protected const string MalformedHtmlFragment = "<H1>Service <B>Malformed";
        protected const string ValidEndpointValue = "https://nationalcareersservice.direct.gov.uk/regions/unittests/";
        protected const string ValidEndpointNoContentValue = "https://nationalcareersservice.direct.gov.uk/regions/unittests/XXXX";
        protected const string InvalidEndpointValue = "https//nationalcareersservice.direct.gov.uk/";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }
    }
}
