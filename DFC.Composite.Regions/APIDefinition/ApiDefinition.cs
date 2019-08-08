using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using DFC.Functions.DI.Standard.Attributes;
using DFC.Swagger.Standard;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DFC.Composite.Regions.APIDefinition
{
    public static class ApiDefinition
    {
        public const string ApiDefinitionName = "API-Definition";
        public const string ApiDefRoute = "regions/" + ApiDefinitionName;
        public const string ApiDescription = "To support the Digital First Careers Composite UI Region definitions.";

        public const string ApiVersion = "1.0.0";

        [FunctionName(ApiDefinitionName)]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiDefRoute)]
            HttpRequest req,
            [Inject] ISwaggerDocumentGenerator swaggerDocumentGenerator
        )
        {
            string ApiSuffix = Environment.GetEnvironmentVariable("ApiSuffix"); 
            string ApiTitle = "Regions " + ApiSuffix;
            var swagger = swaggerDocumentGenerator.GenerateSwaggerDocument(req, ApiTitle, ApiDescription, ApiDefinitionName, ApiVersion, Assembly.GetExecutingAssembly());

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(swagger)
            };
        }
    }
}