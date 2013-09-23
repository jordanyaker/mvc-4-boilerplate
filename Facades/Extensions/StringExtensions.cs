namespace Boilerplate.Facades {
    using StackExchange.Profiling;

    public static class StringExtensions {
        public static FacadeError GetError(this string errorMessage) {
#if DEBUG
            using (MiniProfiler.Current.Step("StringExtensions.GetError")) {
#endif
                string[] details = errorMessage.Split(';');

                int code = 0;
                string property = "Unknown", description = "An unknown error has occurred.";

                if (details.Length >= 3) {
                    int.TryParse(details[0], out code);
                    property = details[1];
                    description = details[2];
                }

                return new FacadeError(code, property, description); 
#if DEBUG
            }
#endif
        }
    }
}
