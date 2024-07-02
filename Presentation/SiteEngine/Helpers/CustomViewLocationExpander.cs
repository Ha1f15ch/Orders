using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace SiteEngine.Helpers
{
    public class CustomViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var controllerName = context.ActionContext.ActionDescriptor.DisplayName;

            if(controllerName.Contains("Performer"))
            {
                return new[]
                {
                    "/Views/PerformerBoard/{1}/{0}.cshtml",
                    "/Views/PerformerBoard/Layout.cshtml"
                };
            }

            if (controllerName.Contains("Customer"))
            {
                return new[]
                {
                    "/Views/CustomerBoard/{1}/{0}.cshtml",
                    "/Views/CustomerBoard/Layout.cshtml"
                };
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }
    }
}
