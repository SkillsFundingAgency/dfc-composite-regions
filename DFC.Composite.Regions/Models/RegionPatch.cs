using System;
using System.ComponentModel.DataAnnotations;
using DFC.Swagger.Standard.Annotations;

namespace DFC.Composite.Regions.Models
{
    public class RegionPatch
    {
        [Display(Description = "Indicator stating that the application endpoint is working as expected. ")]
        [Example(Description = "true")]
        public bool IsHealthy { get; set; }
    }
}
