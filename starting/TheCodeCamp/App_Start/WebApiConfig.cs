using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Versioning;
using Microsoft.Web.Http.Versioning.Conventions;
using Newtonsoft.Json.Serialization;
using TheCodeCamp.Controllers;

namespace TheCodeCamp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            AutofacConfig.Register();


            config.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new Microsoft.Web.Http.ApiVersion(1, 1);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;

                // avoid URL versioning if possible#
                // cfg.ApiVersionReader = new UrlSegmentApiVersionReader();

                //  var constraintResolver = new DefaultInlineConstraintResolver() {
                // ConstraintMap = {
                // ["apiVersion"] = typeof(ApiVersionRouteConstraint);
                // }

                // Then in Routes (at class level if possible to simplify)
                // [RoutePrefix("api/v{version:apiVersion}/camps")]

                // and Urls are then like  http://localhost:6600/api/v2.0/camps/ATL2018

                //cfg.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                cfg.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("X-Version"),
                    new QueryStringApiVersionReader("ver"));


                // Conventions-based
                //cfg.Conventions.Controller<TalksController>()
                //    .HasApiVersion(1, 0)
                //    .HasApiVersion(1, 1)
                //    .Action(m => m.Get(default(string), default(int), default(bool)))
                //    .MapToApiVersion(2, 0);
            });

            // change json case 
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
