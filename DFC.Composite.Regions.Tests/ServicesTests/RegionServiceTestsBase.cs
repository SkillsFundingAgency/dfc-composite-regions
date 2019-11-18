using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using NSubstitute;
using NUnit.Framework;

namespace DFC.Composite.Regions.Tests.ServicesTests
{
    [TestFixture]
    public abstract class RegionServiceTestsBase : UnitTestsBase
    {
        protected Services.IRegionService _regionService;
        protected Cosmos.Provider.IDocumentDBProvider _documentDbProvider;

        protected static IServiceProvider serviceProvider;

        #region Tests initialisations and cleanup

        [SetUp]
        public void SetUp()
        {
            _documentDbProvider = Substitute.For<Cosmos.Provider.IDocumentDBProvider>();

            _regionService = new Services.RegionService(_documentDbProvider);
        }

        [TearDown]
        public void TearDown()
        {
            _regionService = null;
            _documentDbProvider = null;
        }

        #endregion

        #region Helper methods

        protected ResourceResponse<Document> MockResourceResponse(HttpStatusCode statusCode)
        {
            const string documentServiceResponseClass = "Microsoft.Azure.Documents.DocumentServiceResponse, Microsoft.Azure.DocumentDB.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
            const string dictionaryNameValueCollectionClass = "Microsoft.Azure.Documents.Collections.DictionaryNameValueCollection, Microsoft.Azure.DocumentDB.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

            var resourceResponse = new ResourceResponse<Document>(new Document());
            var documentServiceResponseType = Type.GetType(documentServiceResponseClass);

            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var headers = new NameValueCollection { { "x-ms-request-charge", "0" } };

            var headersDictionaryType = Type.GetType(dictionaryNameValueCollectionClass);

            var headersDictionaryInstance = Activator.CreateInstance(headersDictionaryType, headers);

            var arguments = new[] { Stream.Null, headersDictionaryInstance, statusCode, null };

            var documentServiceResponse = documentServiceResponseType.GetTypeInfo().GetConstructors(flags)[0].Invoke(arguments);

            var responseField = typeof(ResourceResponse<Document>).GetTypeInfo().GetField("response", flags);

            responseField?.SetValue(resourceResponse, documentServiceResponse);

            return resourceResponse;
        }

        #endregion

    }
}
