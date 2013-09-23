namespace Boilerplate.Background {
    public abstract class BackgroundProcess : IBackgroundProcess {
        // -------------------------------------------------------------------------------------
        // Properties
        // -------------------------------------------------------------------------------------
        public bool IsRunning { get; private set; }

        // -------------------------------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------------------------------
        public void Start() {
            if (!IsRunning) {
                IsRunning = true;
                OnStart();
            }
        }
        public void Stop() {
            if (IsRunning) {
                IsRunning = false;
                OnStop();
            }
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
    }
}
