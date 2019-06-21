using NUnit.Framework;

namespace DFC.Composite.Regions.Tests
{
    [SetUpFixture]
    public abstract class UnitTestsBase
    {
        protected const string ValidPathValue = "unit_tests";
        protected const string ValidPathNoContentValue = "unit_tests/XXXX";
        protected const string NullPathValue = null;
        protected const string InvalidPathValue = "unit tests";
        protected const string ValidHtmlFragment = "<H1>Service Unavailable</H1>";
        protected const string MalformedHtmlFragment = "<H1>Service <B>Malformed";
        protected const string ValidEndpointValue = "https://nationalcareersservice.direct.gov.uk/regions/unittests/";
        protected const string ValidEndpointValueWithPlaceHolder = "https://nationalcareersservice.direct.gov.uk/regions/unittests/{0}/contents";
        protected const string ValidEndpointNoContentValue = "https://nationalcareersservice.direct.gov.uk/regions/unittests/XXXX";
        protected const string InvalidEndpointValue = "https//nationalcareersservice.direct.gov.uk/";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }
    }
}
