using Bootstrap;
using Bootstrap.Background;
using Bootstrap.Extensions.Containers;

namespace Boilerplate.Common.Testing.Bootstrap {
    using Machine.Fakes;
    using Machine.Specifications;

    public class BootstrapperBackgroundProcessesHelper_specification : WithFakes {
        static IBootstrapperContainerExtension _containerExtension;

        Establish context = () => {
            _containerExtension = An<IBootstrapperContainerExtension>();
            Bootstrapper.ClearExtensions();
        };

        [Subject("BootstrapperBackgroundProcessesHelper specification")]
        public class when_the_bootstrapper_is_loaded_with_the_background_processes_helper {
            Because of = () =>
                Bootstrapper
                    .With.Extension(_containerExtension)
                    .And.BackgroundProcesses();

            It should_add_the_background_process_extension_to_bootstrapper = () =>
                Bootstrapper.GetExtensions()[1]
                    .ShouldBeOfType<BackgroundProcessesExtension>();
        }
    }
}
