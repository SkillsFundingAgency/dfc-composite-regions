using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DFC.Composite.Regions.Models
{
    public static class Constants
    {
        public enum PageRegions
        {
            None = 0,
            Head,
            Breadcrumb,
            BodyTop,
            Body,
            SidebarRight,
            SidebarLeft,
            BodyFooter
        }

    }
}
