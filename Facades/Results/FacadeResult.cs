namespace Boilerplate.Facades {
    public class FacadeResult {
        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        public FacadeResult() {
            Type = FacadeResultTypes.Success;
        }
        public FacadeResult(FacadeError error) {
            Type = FacadeResultTypes.Error;
            Error = error;
        }

        // -------------------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------------------
        public FacadeResultTypes Type { get; protected set; }
        public FacadeError Error { get; protected set; }
    }
}
