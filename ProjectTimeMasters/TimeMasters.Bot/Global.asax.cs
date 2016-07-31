using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Hangfire;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;
using Microsoft.Owin;
using Owin;
using TimeMasters.PortableClassLibrary.Translator;

[assembly: OwinStartup(typeof(TimeMasters.Bot.WebApiApplication))]

namespace TimeMasters.Bot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Hangfire.GlobalConfiguration.Configuration
                .UseSqlServerStorage(
                    "Server=tcp:timemastershangfire.database.windows.net,1433;Initial Catalog=TimeMastersHangfire;Persist Security Info=False;User ID=timemaster;Password=Tm2016Ic_BOT;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            Translator.RenewAccessToken();
            RecurringJob.AddOrUpdate("TranslatorAccessToken", () => Translator.RenewAccessToken(), "*/9 * * * *");
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseHangfireDashboard();
        }
    }
}
