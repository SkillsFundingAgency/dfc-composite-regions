using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using DFC.Functions.DI.Standard;
using Microsoft.Extensions.DependencyInjection;
using DFC.Common.Standard.Logging;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using DFC.Swagger.Standard;
using DFC.Composite.Regions.Services;
using DFC.Composite.Regions.Cosmos.Provider;

[assembly: WebJobsStartup(typeof(DFC.Composite.Regions.Startup.WebJobsExtensionStartup), "Web Jobs Extension Startup")]

namespace DFC.Composite.Regions.Startup
{
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection();

            builder.Services.AddSingleton<ILoggerHelper, LoggerHelper>();
            builder.Services.AddSingleton<IHttpRequestHelper, HttpRequestHelper>();
            builder.Services.AddSingleton<IHttpResponseMessageHelper, HttpResponseMessageHelper>();
            builder.Services.AddSingleton<IJsonHelper, JsonHelper>();
            builder.Services.AddSingleton<IDocumentDBProvider, DocumentDBProvider>();

            builder.Services.AddScoped<IRegionService, RegionService>();
            builder.Services.AddScoped<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
        }
    }
}
