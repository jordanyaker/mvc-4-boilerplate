namespace Boilerplate.Web.Registration {
    using Autofac;
    using Autofac.Configuration;
    using Autofac.Integration.Mvc;
    using Bootstrap.Autofac;

    public class AutofacCommonRegistration : IAutofacRegistration {
        public void Register(ContainerBuilder containerBuilder) {
            containerBuilder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            containerBuilder.RegisterModule(new AutofacWebTypesModule());
        }
    }
}
