namespace Boilerplate.Common.Tests.Events {
    using System;

    using Boilerplate.Events;

    using Machine.Specifications;
    using Machine.Fakes;

    class SampleEvent { };

    public class EventAggregator_specification {
        static EventAggregator _aggregator;

        Establish context = () => {
            _aggregator = new EventAggregator();
        };

        [Subject(typeof(EventAggregator))]
        public class when_notifying_observers_of_an_event {
            static int _eventInvocations = 0;
            static IDisposable _token;

            Establish context = () => {
                _token = _aggregator.GetEvent<SampleEvent>().Subscribe((e) => { _eventInvocations++; });
            };

            Because of = () =>
                _aggregator.Notify(new SampleEvent());

            It should_invoke_all_subscribers = () =>
                _eventInvocations.ShouldEqual(1);
        }

        [Subject(typeof(EventAggregator))]
        public class and_disposing_of_the_event_token {
            static int _eventInvocations = 0;
            static IDisposable _token;

            Establish context = () => {
                _token = _aggregator.GetEvent<SampleEvent>().Subscribe((e) => { _eventInvocations++; });
                _token.Dispose();
            };

            Because of = () =>
                _aggregator.Notify(new SampleEvent());
           
            It should_not_invoke_the_action_any_further =()=>
                _eventInvocations.ShouldEqual(0);
        }
    }
}
