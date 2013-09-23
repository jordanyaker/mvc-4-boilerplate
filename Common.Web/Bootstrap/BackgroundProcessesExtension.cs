namespace Bootstrap.Background {
    using Bootstrap.Extensions;
    using Bootstrap.Extensions.Containers;
    using Boilerplate.Background;
    using System.Collections.Generic;
    using System.Linq;

    public class BackgroundProcessesExtension : IBootstrapperExtension {
        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        public BackgroundProcessesExtension(IRegistrationHelper registrationHelper) {
            this._registrationHelper = registrationHelper;
        }

        // -------------------------------------------------------------------------------------
        // Fields
        // -------------------------------------------------------------------------------------
        IRegistrationHelper _registrationHelper;

        // -------------------------------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------------------------------
        private List<IBackgroundProcess> GetTasks() {
            List<IBackgroundProcess> tasks;

            if (Bootstrapper.ContainerExtension != null)
                tasks = Bootstrapper.ContainerExtension
                    .ResolveAll<IBackgroundProcess>()
                    .OrderBy(t => t.GetType().Name)
                    .ToList();
            else
                tasks = _registrationHelper
                    .GetInstancesOfTypesImplementing<IBackgroundProcess>()
                    .OrderBy(t => t.GetType().Name)
                    .ToList();

            return tasks;
        }
        public void Run() {
            GetTasks()
                .ForEach(x => x.Start());
        }
        public void Reset() {
            var tasks = GetTasks();
            tasks.Reverse();

            tasks
                .ForEach(x => x.Stop());
        }
    }
}
