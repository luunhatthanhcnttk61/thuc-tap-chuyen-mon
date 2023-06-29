using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace aspnet_mvc_real_estate
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index" }
                );

            routes.MapRoute(
                name: "Show Contact",
                url: "lien-he",
                defaults: new { controller = "Home", action = "Contact" }
                );

            routes.MapRoute(
                name: "Show About",
                url: "gioi-thieu",
                defaults: new { controller = "Home", action = "About" }
                );

            routes.MapRoute(
                name: "Terms of Use",
                url: "chinh-sach-su-dung",
                defaults: new { controller = "Home", action = "PolicyUse" }
                );


            // Admin route config 

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login" }
                );

            routes.MapRoute(
                name: "ForgotPassword",
                url: "quen-mat-khau",
                defaults: new { controller = "User", action = "ForgotPassword" }
                );

            routes.MapRoute(
                name: "VeryfiForgotPassword",
                url: "quen-mat-khau/nhap-ma-xac-minh",
                defaults: new { controller = "User", action = "VeryfiForgotPassword" }
                );

            routes.MapRoute(
               name: "GetPasswordSuccess",
               url: "quen-mat-khau/thanh-cong",
               defaults: new { controller = "User", action = "GetPasswordSuccess" }
               );

            routes.MapRoute(
                name: "Admin page",
                url: "bang-dieu-khien",
                defaults: new { controller = "Admin", action = "Index" }
                );

            routes.MapRoute(
                name: "Collection manager",
                url: "bang-dieu-khien/du-an",
                defaults: new { controller = "Collection", action = "Index" }
                );
            routes.MapRoute(
                name: "Collection create",
                url: "bang-dieu-khien/du-an/tao-du-an",
                defaults: new { controller = "Collection", action = "Create" }
                );
            routes.MapRoute(
                name: "Collection edit",
                url: "bang-dieu-khien/du-an/chinh-sua/{id}",
                defaults: new { controller = "Collection", action = "Edit", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Collection details",
                url: "bang-dieu-khien/du-an/chi-tiet/{id}",
                defaults: new { controller = "Collection", action = "Details", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Collection galery",
                url: "bang-dieu-khien/du-an/thu-vien/{id}",
                defaults: new { controller = "Collection", action = "Galery", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Collection galery add",
                url: "bang-dieu-khien/du-an/thu-vien/{id}/them-anh",
                defaults: new { controller = "Collection", action = "AddImages", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Product manager",
                url: "bang-dieu-khien/san-pham",
                defaults: new { controller = "Product", action = "Index" }
                );
            routes.MapRoute(
                name: "Product create",
                url: "bang-dieu-khien/san-pham/tao-san-pham",
                defaults: new { controller = "Product", action = "Create" }
                );
            routes.MapRoute(
                name: "Product edit",
                url: "bang-dieu-khien/san-pham/chinh-sua/{id}",
                defaults: new { controller = "Product", action = "Edit", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Product details",
                url: "bang-dieu-khien/san-pham/chi-tiet/{id}",
                defaults: new { controller = "Collection", action = "Details", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Product galery",
                url: "bang-dieu-khien/san-pham/thu-vien/{id}",
                defaults: new { controller = "Product", action = "Galery", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Product galery add",
                url: "bang-dieu-khien/san-pham/thu-vien/{id}/them-anh",
                defaults: new { controller = "Product", action = "AddImages", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "News manager",
                url: "bang-dieu-khien/bai-viet",
                defaults: new { controller = "New", action = "Index" }
                );
            routes.MapRoute(
                name: "News create",
                url: "bang-dieu-khien/bai-viet/them-bai-viet",
                defaults: new { controller = "New", action = "Create" }
                );
            routes.MapRoute(
                name: "News edit",
                url: "bang-dieu-khien/bai-viet/chinh-sua/{id}",
                defaults: new { controller = "New", action = "Edit", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "News details",
                url: "bang-dieu-khien/bai-viet/chi-tiet/{id}",
                defaults: new { controller = "New", action = "Details", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Users manager",
                url: "bang-dieu-khien/nguoi-dung",
                defaults: new { controller = "User", action = "Index" }
                );
            routes.MapRoute(
                name: "User info",
                url: "bang-dieu-khien/thong-tin-nguoi-dung/{id}",
                defaults: new { controller = "User", action = "UserDetails", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Users create",
                url: "bang-dieu-khien/nguoi-dung/tao-nguoi-dung",
                defaults: new { controller = "Account", action = "Register" }
                );

            // News config 
            routes.MapRoute(
                name: "News Category",
                url: "tin-tuc",
                defaults: new { controller = "New", action = "Category" }
                );

            routes.MapRoute(
                name: "News Post",
                url: "tin-tuc/{slug}_{id}",
                defaults: new { controller = "New", action = "Post", slug = (string)null, id = @"\d{1,4}" }
                );

            // Product config

            routes.MapRoute(
                name: "Product Category",
                url: "bat-dong-san",
                defaults: new { controller = "Product", action = "Category" }
                );

            routes.MapRoute(
                name: "Search Result",
                url: "tim-kiem",
                defaults: new { controller = "Home", action = "Search" }
                );

            routes.MapRoute(
                name: "Product View",
                url: "{slug}_{id}",
                defaults: new { controller = "Product", action = "ProductItem", slug = (string)null, id = @"\d{1,4}" }
                );

            // Default config

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
