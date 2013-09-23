namespace Boilerplate.Bootstrapper {
    using System.Web.Mvc;
    using Bootstrap.Extensions.StartupTasks;

    public class RegisterViewEnginesTask : IStartupTask {
        public RegisterViewEnginesTask(ViewEngineCollection engines) {
            _engines = engines;
        }
        public RegisterViewEnginesTask()
            : this(ViewEngines.Engines) {
        }

        private readonly ViewEngineCollection _engines;

        public void Run() {
            _engines.Clear();
            _engines.Add(new RazorViewEngine());
        }
        public void Reset() {
        }
    }
}