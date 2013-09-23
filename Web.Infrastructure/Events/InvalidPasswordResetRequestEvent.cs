namespace Boilerplate.Events {
    public class InvalidPasswordResetRequestEvent {
        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        public InvalidPasswordResetRequestEvent(string email) {
            this.Email = email;
        }

        // -------------------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------------------
        public string Email { get; private set; }
    }
}
