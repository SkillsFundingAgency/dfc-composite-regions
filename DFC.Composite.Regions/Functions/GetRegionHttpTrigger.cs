using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Common.Standard.Logging;
using DFC.Functions.DI.Standard.Attributes;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Functions
{
    public static class GetRegionHttpTrigger
    {
        [FunctionName("Get")]
        [ProducesResponseType(typeof(Models.Region), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Region found", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Region does not exist", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Request was malformed", ShowSchema = false)]
        [Display(Name = "Get", Description = "Ability to retrieve all Regions for the given Path")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "paths/{path}/regions")]
            HttpRequest req,
            ILogger log,
            string path,
            [Inject] ILoggerHelper loggerHelper,
            [Inject] IHttpRequestHelper httpRequestHelper,
            [Inject] IHttpResponseMessageHelper httpResponseMessageHelper,
            [Inject] IJsonHelper jsonHelper
        )
        {
            loggerHelper.LogMethodEnter(log);

            // validate the parameters are present
            var dssCorrelationId = httpRequestHelper.GetDssCorrelationId(req);
            if (string.IsNullOrEmpty(dssCorrelationId))
            {
                log.LogInformation($"Unable to locate '{nameof(dssCorrelationId)}' in request header");
            }

            if (!Guid.TryParse(dssCorrelationId, out var correlationGuid))
            {
                log.LogInformation($"Unable to parse '{nameof(dssCorrelationId)}' to a Guid");
                correlationGuid = Guid.NewGuid();
            }

            if (string.IsNullOrEmpty(path))
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Missing value in request for '{nameof(path)}");
                return httpResponseMessageHelper.BadRequest();
            }

            loggerHelper.LogInformationMessage(log, correlationGuid, $"Attempting to get Regions for Path {path}");





            //////////////////////////////////////
            // sample code - to delete vvvvvvvvvvv
            //////////////////////////////////////




            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            var regions = new List<Models.Region>()
            {
                new Models.Region()
                {
                    Path = path,
                    PageRegion =  (int)PageRegions.Head
                },
                new Models.Region()
                {
                    Path = path,
                    PageRegion =  (int)PageRegions.BodyFooter
                }
            };

            //////////////////////////////////////
            // sample code - to delete ^^^^^^^^^^^
            //////////////////////////////////////






            loggerHelper.LogMethodExit(log);

            return regions != null
                ? httpResponseMessageHelper.Ok(jsonHelper.SerializeObjectsAndRenameIdProperty(regions, "id", nameof(Models.Region.DocumentId)))
                : httpResponseMessageHelper.NoContent();
        }
    }
}
