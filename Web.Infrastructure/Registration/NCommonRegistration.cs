namespace Boilerplate.Web.Registration {
    using Autofac;
    using Bootstrap.Autofac;
    using NCommon.Configuration;
    using NCommon.ContainerAdapter.Autofac;
    using NCommon.Data.EntityFramework;
    using Boilerplate.Contexts;

    public class NCommonRegistration : IAutofacRegistration {
        // -------------------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------------------
        public ContainerBuilder Builder { get; set; }
        public DataContext DataContext { get; set; }

        // -------------------------------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------------------------------
        public void Register() {
            this.Register(Builder);
        }
        public void Register(ContainerBuilder containerBuilder) {
            NCommon.Configure
                .Using(new AutofacContainerAdapter(containerBuilder))
                .ConfigureState<DefaultStateConfiguration>()
                .ConfigureData<EFConfiguration>(config => {
                    config.WithContext(() => {
                        return (this.DataContext ?? new DataContext("BoilerplateDatabase"));
                    });
                })
                .ConfigureUnitOfWork<DefaultUnitOfWorkConfiguration>();
        }
        public static NCommonRegistration WithContainerBuilder(ContainerBuilder builder) {
            return new NCommonRegistration {
                Builder = builder
            };
        }
        public NCommonRegistration WithDataContext(DataContext context) {
            this.DataContext = context;
            return this;
        }
    }
}