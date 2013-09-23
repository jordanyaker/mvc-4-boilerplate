using Bootstrap;
using Bootstrap.Background;
using Bootstrap.Extensions.Containers;

namespace Boilerplate.Common.Tests.Background {
    using System.Collections.Generic;
    using Machine.Fakes;
    using Machine.Specifications;
    using Boilerplate.Background;

    public class BackgroundProcessesExtension_specification : WithFakes {
        static IRegistrationHelper _registrationHelper;
        static IBootstrapperContainerExtension _containerExtension;
        static IBackgroundProcess _processA, _processB, _processC;

        Establish context = () => {
            Bootstrapper.ClearExtensions();

            _processA = An<IBackgroundProcess>();
            _processB = An<IBackgroundProcess>();
            _processC = An<IBackgroundProcess>();

            _containerExtension = An<IBootstrapperContainerExtension>();
            _containerExtension
                .WhenToldTo(x => x.ResolveAll<IBackgroundProcess>())
                .Return(new List<IBackgroundProcess>() {
                    _processA,
                    _processB,
                    _processC
                });

            _registrationHelper = An<IRegistrationHelper>();
            _registrationHelper
                .WhenToldTo(x => x.GetInstancesOfTypesImplementing<IBackgroundProcess>())
                .Return(new List<IBackgroundProcess> {
                    _processA,
                    _processB,
                    _processC
                });
        };

        [Subject("BackgroundProcessesExtension specification")]
        public class when_starting_the_bootstrapper_with_the_background_process_extension {
            [Subject("BackgroundProcessesExtension specification, when starting the bootstrapper with the background process extension")]
            public class and_a_container_extension_has_been_declared {
                Establish context = () =>
                    Bootstrapper
                        .With.Extension(_containerExtension)
                        .And.Extension(new BackgroundProcessesExtension(_registrationHelper));

                Because of = () =>
                    Bootstrapper.Start();

                It should_start_all_of_the_background_processes = () => {
                    new[] { _processA, _processB, _processC }
                        .ForEach(x => x
                            .WasToldTo(y => y.Start()));
                };
            }

            [Subject("BackgroundProcessesExtension specification, when starting the bootstrapper with the background process extension")]
            public class and_no_container_extension_has_been_declared {
                Because of = () =>
                    new BackgroundProcessesExtension(_registrationHelper)
                        .Run();

                It should_start_all_of_the_background_processes = () =>
                    new[] { _processA, _processB, _processC }
                        .ForEach(x => x
                            .WasToldTo(y => y.Start()));
            }
        }

        [Subject("BackgroundProcessesExtension specification")]
        public class when_resetting_the_bootstrapper_with_the_background_process_extension {
            [Subject("BackgroundProcessesExtension specification, when resetting the bootstrapper with the background process extension")]
            public class and_a_container_extension_has_been_declared {
                Establish context = () =>
                    Bootstrapper
                         .With.Extension(_containerExtension)
                         .And.Extension(new BackgroundProcessesExtension(_registrationHelper));

                Because of = () =>
                    Bootstrapper.Reset();

                It should_start_all_of_the_background_processes = () => {
                    new[] { _processA, _processB, _processC }
                        .ForEach(x => x
                            .WasToldTo(y => y.Stop()));
                };
            }

            [Subject("BackgroundProcessesExtension specification, when resetting the bootstrapper with the background process extension")]
            public class and_no_container_extension_has_been_declared {
                Because of = () =>
                    new BackgroundProcessesExtension(_registrationHelper)
                        .Reset();

                It should_start_all_of_the_background_processes = () =>
                    new[] { _processA, _processB, _processC }
                        .ForEach(x => x
                            .WasToldTo(y => y.Stop()));
            }
        }
    }
}
