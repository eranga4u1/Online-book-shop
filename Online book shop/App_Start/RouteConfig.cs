using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Online_book_shop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            name: "AuthorCollection",
            url: "authors",
            defaults: new { controller = "AuthorProfile", action = "Collection", id = UrlParameter.Optional },
            new string[] { "Online_book_shop.Controllers" }
           );
            routes.MapRoute(
                 name: "AuthorProfile",
                 url: "authors/{id}",
                 defaults: new { controller = "AuthorProfile", action = "Index", id = UrlParameter.Optional },
                 new string[] { "Online_book_shop.Controllers" }
                );
            routes.MapRoute(
                  name: "BookCollection",
                  url: "books",
                  defaults: new { controller = "BookProfile", action = "Collection", id = UrlParameter.Optional },
                  new string[] { "Online_book_shop.Controllers" }
               );
            routes.MapRoute(
                   name: "BookProfile",
                   url: "book/{id}",
                   defaults: new { controller = "BookProfile", action = "Index", id = UrlParameter.Optional },
                   new string[] { "Online_book_shop.Controllers" }
                );
            routes.MapRoute(
                   name: "Category",
                   url: "Category/{id}",
                   defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional },
                   new string[] { "Online_book_shop.Controllers" }
                );
            routes.MapRoute(
              name: "PublisherCollection",
              url: "Publisher/collection",
              defaults: new { controller = "Publisher", action = "collection", id = UrlParameter.Optional },
              new string[] { "Online_book_shop.Controllers" }
           );
            routes.MapRoute(
                   name: "Publisher",
                   url: "Publisher/{id}",
                   defaults: new { controller = "Publisher", action = "Index", id = UrlParameter.Optional },
                   new string[] { "Online_book_shop.Controllers" }
                );
            routes.MapRoute(
                   name: "Page",
                   url: "Page/{urlPart}",
                   defaults: new { controller = "Page", action = "Index", id = UrlParameter.Optional },
                   new string[] { "Online_book_shop.Controllers" }
                );
            routes.MapRoute(
                  name: "Blog Post",
                  url: "Blog/{id}",
                  defaults: new { controller = "Blog", action = "Blog", id = UrlParameter.Optional },
                  new string[] { "Online_book_shop.Controllers" }
               );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
