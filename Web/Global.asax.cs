namespace Boilerplate.Web {
    using Autofac;
    using Autofac.Integration.Mvc;
    using AutofacContrib.CommonServiceLocator;
    using Bootstrap;
    using Bootstrap.Autofac;
    using Bootstrap.Background;
    using Bootstrap.Extensions.StartupTasks;
    using Microsoft.Practices.ServiceLocation;
    using StackExchange.Profiling;
    using System;
    using System.Web;
    using System.Web.Mvc;

    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            MvcHandler.DisableMvcResponseHeader = true;

            try {
                Bootstrapper
                    .With.Autofac()
                    .And.StartupTasks()
                    .And.BackgroundProcesses()
                    .Start();

                var container = (ILifetimeScope)Bootstrapper.Container;
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
                DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            } catch {
                // Configuration issues occurred during the startup.
            }
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e) {
            if (HttpContext.Current != null && HttpContext.Current.Response != null && Request.IsLocal == false) {
                HttpContext.Current.Response.Headers.Remove("X-Powered-By");
                HttpContext.Current.Response.Headers.Remove("Server");
            }
        }

#if DEBUG
        protected void Application_BeginRequest() {
            MiniProfiler.Start();
            MiniProfiler.StepStatic("Application.BeginRequest");
        }

        protected void Application_EndRequest() {
            MiniProfiler.Stop();
        }
#endif
    }
}