using System;

/// <summary>
/// A base utility class to provide a standard pattern for disposable objects.
/// </summary>
public abstract class Disposable : IDisposable {
    private bool disposed;

    /// <summary>
    /// The default destructor.
    /// </summary>
    ~Disposable() {
        Dispose(false);
    }

    /// <summary>
    /// Implementation of the <see cref="IDisposable.Dispose"/> method.
    /// </summary>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// An overridable handler for implementing the managed disposal mechanism.
    /// </summary>
    protected virtual void OnDisposing() {
    }

    /// <summary>
    /// The internal implementation of the <see cref="IDisposable.Dispose"/> method.
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose(bool disposing) {
        if (!disposed && disposing) {
            OnDisposing();
        }

        disposed = true;
    }
}
