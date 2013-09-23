namespace Boilerplate.Web.Infrastructure.Testing.Bootstrapper {
    using System.Linq;
    using System.Web.Mvc;
    using Machine.Specifications;
    using Boilerplate.Bootstrapper;
    using Boilerplate.Web.Filters;

    public class RegisterGlobalFiltersTask_specification {
        static RegisterGlobalFiltersTask _registerGlobalFiltersTask;
        static GlobalFilterCollection _filters;

        Establish context = () => {
            _filters = new GlobalFilterCollection();
            _registerGlobalFiltersTask = new RegisterGlobalFiltersTask(_filters);
        };

        It should_allow_the_default_constructor_without_issues = () =>
            Catch.Exception(() => new RegisterGlobalFiltersTask())
                .ShouldBeNull();

        [Subject("RegisterGlobalFiltersTask specification")]
        public class when_running_the_task {
            Because of = () =>
                _registerGlobalFiltersTask.Run();

            It should_register_the_error_handler = () =>
                _filters.Any(f => f.Instance is HandleErrorAttribute).ShouldBeTrue();

            It should_register_the_localization_handler = () =>
                _filters.Any(f => f.Instance is LocalizationAttribute).ShouldBeTrue();
        }

        [Subject("RegisterGlobalFiltersTask specification")]
        public class when_resetting_the_task {
            It should_work_without_issues = () =>
                Catch.Exception(() => _registerGlobalFiltersTask.Reset())
                    .ShouldBeNull();
        } 
    }
}
