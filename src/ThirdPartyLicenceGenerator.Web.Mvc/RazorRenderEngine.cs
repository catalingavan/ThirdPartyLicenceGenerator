using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ThirdPartyLicenceGenerator.Web.Mvc.Controllers;

namespace ThirdPartyLicenceGenerator.Web.Mvc
{
    public static class RazorRenderEngine
    {
        private static T CreateController<T>(RouteData routeData = null) where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper = null;
            if (HttpContext.Current == null)
            {
                HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));
            }

            if (HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller", controller.GetType().Name
                    .ToLower()
                    .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

        public static string RenderView(object model, string viewName)
        {
            Controller controller = CreateController<HomeController>();

            ViewEngineResult result = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);

            if (result.View == null)
                throw new Exception($"View Page {viewName} was not found");

            controller.ViewData.Model = model;
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (System.Web.UI.HtmlTextWriter output = new System.Web.UI.HtmlTextWriter(sw))
                {
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, result.View, controller.ViewData, controller.TempData, output);
                    result.View.Render(viewContext, output);
                }
            }

            return sb.ToString();
        }
    }
}