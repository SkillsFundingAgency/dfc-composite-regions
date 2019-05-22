using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Common.Standard.Logging;
using DFC.Composite.Regions.Models;
using DFC.Composite.Regions.Services;
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
        [FunctionName("GetById")]
        [ProducesResponseType(typeof(Models.Region), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Region found", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Region does not exist", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Request was malformed", ShowSchema = false)]
        [Display(Name = "Get", Description = "Ability to retrieve an individual Region for the given Path")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "paths/{path}/regions/{pageRegion}")]
            HttpRequest req,
            ILogger log,
            string path,
            int pageRegion,
            [Inject] ILoggerHelper loggerHelper,
            [Inject] IHttpRequestHelper httpRequestHelper,
            [Inject] IHttpResponseMessageHelper httpResponseMessageHelper,
            [Inject] IJsonHelper jsonHelper,
            [Inject] IRegionService regionService
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
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Missing value in request for '{nameof(path)}'");
                return httpResponseMessageHelper.BadRequest();
            }

            if (pageRegion == 0 || !Enum.IsDefined(typeof(PageRegions), pageRegion))
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Missing/invalid value in request for '{nameof(pageRegion)}'");
                return httpResponseMessageHelper.BadRequest();
            }

            PageRegions pageRegionValue = (PageRegions)pageRegion;

            loggerHelper.LogInformationMessage(log, correlationGuid, $"Attempting to get Region {pageRegionValue} for Path {path}");

            var regionModel = await regionService.GetAsync(path, pageRegionValue);

            if (regionModel == null)
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Region does not exist for {pageRegionValue} for Path {path}");
                return httpResponseMessageHelper.NoContent();
            }

            loggerHelper.LogMethodExit(log);

            return regionModel != null
                ? httpResponseMessageHelper.Ok(jsonHelper.SerializeObjectAndRenameIdProperty(regionModel, "id", nameof(Models.Region.DocumentId)))
                : httpResponseMessageHelper.NoContent();
        }
    }
}
