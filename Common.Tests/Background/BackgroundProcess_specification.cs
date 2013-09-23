using Boilerplate.Background;
namespace Boilerplate.Common.Tests.Background {
    using Machine.Specifications;
    using Moq;
    using Moq.Protected;

    public class BackgroundProcess_specification {
        static Mock<BackgroundProcess> _backgroundTask;

        Establish context = () =>
            _backgroundTask = new Mock<BackgroundProcess> { CallBase = true };

        Machine.Specifications.It should_not_be_running_by_default = () =>
            _backgroundTask.Object.IsRunning.ShouldBeFalse();

        [Subject("BackgroundProcess specification")]
        public class when_starting_the_task {
            Because of = () =>
                _backgroundTask.Object.Start();

            Machine.Specifications.It should_run_the_internal_startup_routine = () =>
                _backgroundTask.Protected().Verify("OnStart", Times.Once());

            Machine.Specifications.It should_mark_the_task_as_running = () =>
                _backgroundTask.Object.IsRunning.ShouldBeTrue();
        }

        [Subject("BackgroundProcess specification")]
        public class when_stopping_a_task {
            Because of = () =>
                _backgroundTask.Object.Stop();

            [Subject("BackgroundProcess specification, when stopping a task")]
            public class and_the_task_is_running {
                Establish context = () =>
                    _backgroundTask.Object.Start();

                Machine.Specifications.It should_run_the_internal_shutdown_routine = () =>
                    _backgroundTask.Protected().Verify("OnStop", Times.Once());

                Machine.Specifications.It should_mark_the_task_as_no_longer_running = () =>
                    _backgroundTask.Object.IsRunning.ShouldBeFalse();
            }
        }
    }
}
