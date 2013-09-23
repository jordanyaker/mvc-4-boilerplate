namespace Boilerplate.Background {
    public interface IBackgroundProcess {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}
