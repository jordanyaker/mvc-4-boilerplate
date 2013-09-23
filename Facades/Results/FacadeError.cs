namespace Boilerplate.Facades {
    public class FacadeError {
        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        public FacadeError(int code, string property, string description) {
            this.Code = code;
            this.Property = property;
            this.Description = description;
        }

        // -------------------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------------------
        public int Code { get; private set; }
        public string Property { get; private set; }
        public string Description { get; private set; }
    }
}
