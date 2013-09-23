namespace Boilerplate.Events {
    public class PasswordResetRequestEvent {
        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        public PasswordResetRequestEvent(string name, string email, string url) {
            this.Name = name;
            this.Email = email;
            this.Url = url;
        }

        // -------------------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------------------
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
    }
}
