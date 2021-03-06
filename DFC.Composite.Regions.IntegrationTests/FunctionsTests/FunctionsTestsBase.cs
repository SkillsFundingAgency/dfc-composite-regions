﻿using System;
using System.IO;
using DFC.Common.Standard.Logging;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DFC.Composite.Regions.IntegrationTests.FunctionsTests
{
    [TestFixture]
    public class FunctionsTestsBase
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

        protected static IServiceProvider serviceProvider;

        #region Tests initialisations and cleanup

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext());
            var logger = NullLoggerFactory.Instance.CreateLogger("Unit Test Null Logger");

            services.AddSingleton<DefaultHttpRequest>(defaultHttpRequest);
            services.AddSingleton<ILogger>(logger);
            services.AddSingleton<ILoggerHelper, LoggerHelper>();
            services.AddSingleton<IHttpRequestHelper, HttpRequestHelper>();
            services.AddSingleton<IHttpResponseMessageHelper, HttpResponseMessageHelper>();
            services.AddSingleton<IJsonHelper, JsonHelper>();

            services.AddSingleton<Services.IRegionService, Services.RegionService>();
            services.AddSingleton<Cosmos.Provider.IDocumentDBProvider, Cosmos.Provider.DocumentDBProvider>();

            var appRegistrationConfiguration = configuration.GetSection("Configurations:CosmosDbConnections:AppRegistration").Get<Models.CosmosDbConnection>();

            serviceProvider = services.BuildServiceProvider();

            // set the environment variables
            Environment.SetEnvironmentVariable(Regions.Models.EnvironmentVariableNames.CosmosConnectionString, appRegistrationConfiguration.ConnectionString);
            Environment.SetEnvironmentVariable(Regions.Models.EnvironmentVariableNames.CosmosDatabaseId, appRegistrationConfiguration.DatabaseId);
            Environment.SetEnvironmentVariable(Regions.Models.EnvironmentVariableNames.CosmosCollectionId, appRegistrationConfiguration.CollectionId);
            Environment.SetEnvironmentVariable(Regions.Models.EnvironmentVariableNames.CosmosPartitionKey, appRegistrationConfiguration.PartitionKey);
        }

        [TearDown]
        public void TearDown()
        {
            serviceProvider = null;
        }

        #endregion

        #region Helper methods

        protected MemoryStream MemoryStreamFromObject<T>(T model)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var json = JsonConvert.SerializeObject(model);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            return ms;
        }

        #endregion

    }
}
