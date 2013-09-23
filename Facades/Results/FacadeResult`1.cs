namespace Boilerplate.Facades {
    using System;

    public class FacadeResult<T> : FacadeResult {
        // -------------------------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------------------------
        public FacadeResult(T data) {
            Type = FacadeResultTypes.Success;
            Data = data;
        }
        public FacadeResult(FacadeError error)
            : base(error) {
        }

        // -------------------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------------------
        public T Data { get; set; }
    }
}
