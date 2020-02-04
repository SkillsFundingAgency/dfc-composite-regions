using DFC.Common.Standard.Logging;
using DFC.Composite.Regions.Extensions;
using DFC.Composite.Regions.Models;
using DFC.Composite.Regions.Services;
using DFC.Functions.DI.Standard.Attributes;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Functions
{
    public static class PatchRegionHttpTrigger
    {
        [FunctionName("Patch")]
        [ProducesResponseType(typeof(Models.Region), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Region found", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Region does not exist", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Request was malformed", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.UnprocessableEntity, Description = "Region validation error(s)", ShowSchema = false)]
        [Display(Name = "Patch", Description = "Ability to update specific values of an existing Region for a Path. <br>" +
                                             "<br><b>Validation Rules:</b> <br>" +
                                             "<br><b>Path:</b> Is mandatory <br>" +
                                             "<br><b>PageRegion:</b> Is mandatory <br>" +
                                             "<br><b>RegionEndpoint:</b> Is mandatory <br>"
            )]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "paths/{path}/regions/{pageRegion}")]
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
            var correlationId = httpRequestHelper.GetOrCreateDssCorrelationId(req);

            if (string.IsNullOrEmpty(path))
            {
                loggerHelper.LogInformationMessage(log, correlationId, $"Missing value in request for '{nameof(path)}'");
                return httpResponseMessageHelper.BadRequest();
            }

            if (pageRegion == 0 || !Enum.IsDefined(typeof(PageRegions), pageRegion))
            {
                loggerHelper.LogInformationMessage(log, correlationId, $"Missing/invalid value in request for '{nameof(pageRegion)}'");
                return httpResponseMessageHelper.BadRequest();
            }

            PageRegions pageRegionValue = (PageRegions)pageRegion;

            JsonPatchDocument<Region> regionPatch;
            try
            {
                regionPatch = await httpRequestHelper.GetResourceFromRequest<JsonPatchDocument<Region>>(req);

                if (regionPatch == null)
                {
                    loggerHelper.LogException(log, correlationId, "Request body is empty", null);
                    return httpResponseMessageHelper.BadRequest();
                }
            }
            catch (Exception ex)
            {
                loggerHelper.LogException(log, correlationId, ex);
                return httpResponseMessageHelper.BadRequest();
            }

            loggerHelper.LogInformationMessage(log, correlationId, $"Attempting to get Region {pageRegionValue} for Path {path}");

            var currentRegion = await regionService.GetAsync(path, pageRegionValue);

            if (currentRegion == null)
            {
                loggerHelper.LogInformationMessage(log, correlationId, $"Region does not exist for {pageRegionValue} for Path {path}");
                return httpResponseMessageHelper.NoContent();
            }

            try
            {
                loggerHelper.LogInformationMessage(log, correlationId, $"Attempting to apply patch to {path} region {pageRegionValue}");
                regionPatch?.ApplyTo(currentRegion);
                var validationResults = currentRegion.Validate(new ValidationContext(currentRegion));

                if (validationResults.Any())
                {
                    loggerHelper.LogInformationMessage(log, correlationId, "Validation Failed");
                    return httpResponseMessageHelper.UnprocessableEntity(validationResults.ToList());
                }
            }
            catch (Exception ex)
            {
                loggerHelper.LogException(log, correlationId, ex);
                return httpResponseMessageHelper.BadRequest();
            }

            try
            {
                loggerHelper.LogInformationMessage(log, correlationId, $"Attempting to update path {path}");
                var patchedRegion = await regionService.ReplaceAsync(currentRegion);
                loggerHelper.LogMethodExit(log);
                return httpResponseMessageHelper.Ok(jsonHelper.SerializeObjectAndRenameIdProperty(patchedRegion, "id", nameof(Models.Region.DocumentId)));
            }
            catch (Exception ex)
            {
                loggerHelper.LogException(log, correlationId, ex);
                return httpResponseMessageHelper.UnprocessableEntity(new JsonException(ex.Message, ex));
            }
        }
    }
}
