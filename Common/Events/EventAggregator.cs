namespace Boilerplate.Events {
    using System;
    using System.Collections.Concurrent;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public interface IEventAggregator {
        IObservable<T> GetEvent<T>();
        void Notify<T>(T value);
    }

    public class EventAggregator : IEventAggregator {
        // -------------------------------------------------------------------------------------
        // Fields
        // -------------------------------------------------------------------------------------
        readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        // -------------------------------------------------------------------------------------
        // Methods
        // -------------------------------------------------------------------------------------
        public IObservable<T> GetEvent<T>() {
            var subject = (ISubject<T>)_subjects.GetOrAdd(typeof(T), t => new Subject<T>());

            return subject.AsObservable();
        }
        public void Notify<T>(T value) {
            object subject;

            if (_subjects.TryGetValue(typeof(T), out subject)) {
                ((ISubject<T>)subject).OnNext(value);
            }
        }
    }
}
