namespace Boilerplate.Bootstrapper {
    using System.Web.Mvc;
    using Bootstrap.Extensions.StartupTasks;
    using Boilerplate.Web.Filters;

    public class RegisterGlobalFiltersTask : IStartupTask {
        public RegisterGlobalFiltersTask()
            : this(GlobalFilters.Filters) {
        }
        public RegisterGlobalFiltersTask(GlobalFilterCollection filters) {
            this._filters = filters;
        }

        private GlobalFilterCollection _filters;

        public void Run() {
            _filters.Add(new HandleErrorAttribute());
            _filters.Add(new LocalizationAttribute());
        }
        public void Reset() {
        }
    }
}