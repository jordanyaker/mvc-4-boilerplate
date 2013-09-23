namespace Boilerplate.Web.Infrastructure.UnitTesting.Bootstrapper {
    using System.Web.Mvc;
    using Machine.Specifications;
    using Boilerplate.Bootstrapper;

    public class RegisterViewEnginesTask_specification {
        static ViewEngineCollection _viewEngines;
        static RegisterViewEnginesTask _registerViewEnginesTask;

        Establish context = () => {
            _viewEngines = new ViewEngineCollection();
            _registerViewEnginesTask = new RegisterViewEnginesTask(_viewEngines);
        };

        It should_allow_the_default_constructor_without_issues = () =>
            Catch.Exception(() => new RegisterViewEnginesTask())
                .ShouldBeNull();

        [Subject("RegisterViewEnginesTask specification")]
        public class when_running_the_task {
            Because of = () =>
                _registerViewEnginesTask.Run();

            It should_register_the_engines_for_the_project = () =>
                _viewEngines.Count.ShouldEqual(1);

            It should_only_setup_the_Razor_view_engine = () =>
                _viewEngines.ShouldEachConformTo(x => x is RazorViewEngine);
        }

        [Subject("RegisterViewEnginesTask specification")]
        public class when_resetting_the_task {
            It should_work_without_issues = () =>
                Catch.Exception(() => _registerViewEnginesTask.Reset())
                    .ShouldBeNull();
        }
    }
}
