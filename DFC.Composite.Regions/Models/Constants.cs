namespace DFC.Composite.Regions.Models
{
    public static class Constants
    {
        public const string PathRegEx = @"^[a-zA-Z0-9](\w|[.,\/\-])*[a-zA-Z0-9]$";

        public enum PageRegions
        {
            None = 0,
            Head,
            Breadcrumb,
            BodyTop,
            Body,
            SidebarRight,
            SidebarLeft,
            BodyFooter,
            HeroBanner
        }
    }
}
