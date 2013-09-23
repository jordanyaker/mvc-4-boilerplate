namespace Bootstrap.Background {
    using Bootstrap.Extensions;
    using Bootstrap.Extensions.Containers;

    public static class BackgroundProcessesConvenienceExtensions {
        public static BootstrapperExtensions BackgroundProcesses(this BootstrapperExtensions extensions) {
            var extension = new BackgroundProcessesExtension(Bootstrapper.RegistrationHelper);
            return extensions.Extension(extension);
        }
    }
}
