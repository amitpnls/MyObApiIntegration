using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HonorITDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "QuotesList",
                url: "QuoteList",
                defaults: new { controller = "Home", action = "QuoteList" });
            routes.MapRoute(
                name: "PurchaseOrderList",
                url: "PurchaseOrderList",
                defaults: new { controller = "Home", action = "PurchaseOrderAR" });
            routes.MapRoute(
                name: "CreatePurchaseOrder",
                url: "CreatePurchaseOrder",
                defaults: new { controller = "Home", action = "CreatePurchaseOrder" });
        }
    }
}
