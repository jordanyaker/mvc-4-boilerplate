namespace Boilerplate.Web.Infrastructure.Testing.Registration {
    using Autofac;
    using Machine.Specifications;
    using Boilerplate.Web.Registration;

    public class RegisterConfiguration_specification {
        static AutofacCommonRegistration _registerConfiguration;
        static ContainerBuilder _containerBuilder;

        Establish context = () => {
            _registerConfiguration = new AutofacCommonRegistration();

            _containerBuilder = new ContainerBuilder();
        };

        [Subject("RegistrationConfiguration specification")]
        public class when_loading_the_registration {
            [Subject("RegistrationConfiguration specification, when loading the registration")]
            public class and_there_is_no_autofac_configuration_section {
                It should_throw_an_exception = () =>
                    Catch.Exception(() => _registerConfiguration.Register(_containerBuilder))
                        .ShouldNotBeNull();
            }
        }
    }
}
