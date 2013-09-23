namespace Boilerplate.Web.Infrastructure.Testing.Bootstrapper {
    using System.Web.Routing;
    using Machine.Specifications;
    using Boilerplate.Bootstrapper;

    public class RegisterRoutesTask_specification {
        static RouteCollection _routes;
        static RegisterRoutesTask _registerRoutesTask;

        Establish context = () => {
            _routes = new RouteCollection();
            _registerRoutesTask = new RegisterRoutesTask(_routes);
        };

        It should_allow_the_default_constructor_without_issues = () =>
            Catch.Exception(() => new RegisterRoutesTask())
                .ShouldBeNull();

        [Subject("RegisterRoutesTask specification")]
        public class when_running_the_task {
            Because of = () =>
                _registerRoutesTask.Run();

            It should_register_the_routes_for_the_project = () =>
                _routes.Count.ShouldBeGreaterThan(0);

            It should_route_existing_files = () =>
                _routes.RouteExistingFiles.ShouldBeTrue();

            //It should_register_the_localized_500_error_route = () => {
            //    var data = _routes.GetRouteDataFor("~/es-mx/500");
            //    data.Values["culture"].ShouldEqual("es-mx");
            //    data.Values["controller"].ShouldEqual("Errors");
            //    data.Values["action"].ShouldEqual("Error500");
            //};

            //It should_register_the_localized_404_error_route = () => {
            //    var data = _routes.GetRouteDataFor("~/es-mx/404");
            //    data.Values["culture"].ShouldEqual("es-mx");
            //    data.Values["controller"].ShouldEqual("Errors");
            //    data.Values["action"].ShouldEqual("Error404");
            //};

            //It should_register_the_localized_logout_route = () => {
            //    var data = _routes.GetRouteDataFor("~/es-mx/Logout");
            //    data.Values["culture"].ShouldEqual("es-mx");
            //    data.Values["controller"].ShouldEqual("Security");
            //    data.Values["action"].ShouldEqual("Logout");
            //};

            //It should_register_the_localized_login_route = () => {
            //    var data = _routes.GetRouteDataFor("~/es-mx/Login");
            //    data.Values["culture"].ShouldEqual("es-mx");
            //    data.Values["controller"].ShouldEqual("Security");
            //    data.Values["action"].ShouldEqual("Login");
            //};

            It should_register_the_500_error_route = () => {
                var data = _routes.GetRouteDataFor("~/500");
                data.Values["controller"].ShouldEqual("Errors");
                data.Values["action"].ShouldEqual("Error500");
            };

            It should_register_the_404_error_route = () => {
                var data = _routes.GetRouteDataFor("~/404");
                data.Values["controller"].ShouldEqual("Errors");
                data.Values["action"].ShouldEqual("Error404");
            };

            It should_register_the_logout_route = () => {
                var data = _routes.GetRouteDataFor("~/Logout");
                data.Values["controller"].ShouldEqual("Security");
                data.Values["action"].ShouldEqual("Logout");
            };

            It should_register_the_login_route = () => {
                var data = _routes.GetRouteDataFor("~/Login");
                data.Values["controller"].ShouldEqual("Security");
                data.Values["action"].ShouldEqual("Login");
            };

            It should_register_the_default_landing_page_route = () => {
                var data = _routes.GetRouteDataFor("~/");
                data.Values["controller"].ShouldEqual("Home");
                data.Values["action"].ShouldEqual("Index");
            };

            //It should_register_the_localized_routes = () => {
            //    var data = _routes.GetRouteDataFor("~/en-us/Home/Index");
            //    data.Values["culture"].ShouldEqual("en-us");
            //    data.Values["controller"].ShouldEqual("Home");
            //    data.Values["action"].ShouldEqual("Index");
            //};
        }

        [Subject("RegisterRoutesTask specification")]
        public class when_resetting_the_task {
            It should_work_without_issues = () =>
                Catch.Exception(() => _registerRoutesTask.Reset())
                    .ShouldBeNull();
        }
    }
}
