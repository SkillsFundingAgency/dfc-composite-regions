using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DFC.Common.Standard.Logging;
using DFC.Composite.Regions.Services;
using DFC.Functions.DI.Standard.Attributes;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using DFC.Swagger.Standard.Annotations;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static DFC.Composite.Regions.Models.Constants;

namespace DFC.Composite.Regions.Functions
{
    public static class PutRegionHttpTrigger
    {
        [FunctionName("Put")]
        [ProducesResponseType(typeof(Models.Region), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "Region found", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "Region does not exist", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Request was malformed", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.UnprocessableEntity, Description = "Region validation error(s)", ShowSchema = false)]
        [Display(Name = "Put", Description = "Ability to overwrite an existing Region for a Path. <br>" +
                                             "<br><b>Validation Rules:</b> <br>" +
                                             "<br><b>Path:</b> Is mandatory <br>" +
                                             "<br><b>PageRegion:</b> Is mandatory <br>" +
                                             "<br><b>RegionEndpoint:</b> Is mandatory <br>"
            )]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "paths/{path}/regions/{pageRegion}")]
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

            var pathRegex = new Regex(@"^[A-Za-z0-9.,-_]*$");

            if (path.Length > 100 || !pathRegex.IsMatch(path))
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Invalid value in request for '{nameof(path)}'");
                return httpResponseMessageHelper.BadRequest();
            }

            if (pageRegion == 0 || !Enum.IsDefined(typeof(PageRegions), pageRegion))
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Missing/invalid value in request for '{nameof(pageRegion)}'");
                return httpResponseMessageHelper.BadRequest();
            }

            PageRegions pageRegionValue = (PageRegions)pageRegion;
            Models.Region regionRequest;

            try
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, "Attempt to get resource from body of the request");
                regionRequest = await httpRequestHelper.GetResourceFromRequest<Models.Region>(req);

                if (regionRequest == null)
                {
                    loggerHelper.LogInformationMessage(log, correlationGuid, "Missing body in req");
                    return httpResponseMessageHelper.UnprocessableEntity();
                }
            }
            catch (JsonException ex)
            {
                loggerHelper.LogError(log, correlationGuid, "Unable to retrieve body from req", ex);
                return httpResponseMessageHelper.UnprocessableEntity(ex);
            }

            if (!regionRequest.DocumentId.HasValue)
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Request value for '{nameof(regionRequest.DocumentId)}' is missing");
                return httpResponseMessageHelper.BadRequest();
            }

            if (path != regionRequest.Path)
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Request value for '{nameof(regionRequest.Path)}' does not match resource path value");
                return httpResponseMessageHelper.BadRequest();
            }

            if (string.IsNullOrEmpty(regionRequest.RegionEndpoint))
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Missing value in request for '{nameof(regionRequest.RegionEndpoint)}'");
                return httpResponseMessageHelper.BadRequest();
            }

            if (!Uri.IsWellFormedUriString(regionRequest.RegionEndpoint, UriKind.Absolute))
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Request value for '{nameof(regionRequest.RegionEndpoint)}' is not a valid absolute Uri");
                return httpResponseMessageHelper.BadRequest();
            }

            if (pageRegionValue != regionRequest.PageRegion)
            {
                loggerHelper.LogInformationMessage(log, correlationGuid, $"Request value for '{nameof(regionRequest.PageRegion)}' does not match resource path value");
                return httpResponseMessageHelper.BadRequest();
            }

            if (!string.IsNullOrEmpty(regionRequest.OfflineHtml))
            {
                var htmlDoc = new HtmlDocument();

                htmlDoc.LoadHtml(regionRequest.OfflineHtml);

                if (htmlDoc.ParseErrors.Any())
                {
                    loggerHelper.LogInformationMessage(log, correlationGuid, $"Request value for '{nameof(regionRequest.OfflineHtml)}' contains malformed HTML");
                    return httpResponseMessageHelper.BadRequest();
                }
            }

            loggerHelper.LogInformationMessage(log, correlationGuid, string.Format("Attempting to update region {0}", regionRequest.DocumentId));
            var replacedRegion = await regionService.ReplaceAsync(regionRequest);

            loggerHelper.LogMethodExit(log);

            return replacedRegion != null
                ? httpResponseMessageHelper.Ok(jsonHelper.SerializeObjectAndRenameIdProperty(replacedRegion, "id", nameof(Models.Region.DocumentId)))
                : httpResponseMessageHelper.NoContent();
        }
    }
}
