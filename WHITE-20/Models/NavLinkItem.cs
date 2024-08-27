using Microsoft.AspNetCore.Components.Routing;

namespace WHITE_20.Models
{
    public class NavLinkItem
    {
        public string Href { get; set; }
        public string Title { get; set; }
        public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;
        public string iconClass { get; set; }
    }
}