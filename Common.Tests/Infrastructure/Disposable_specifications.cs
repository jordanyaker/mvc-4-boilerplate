namespace Boilerplate.Common.Tests.Infrastructure {
    using Machine.Fakes;
    using Machine.Specifications;
    using System;

    public class Disposable_specifications : WithFakes {
        class TestDisposable : Disposable {
            public bool WasDisposed { get; set; }
            protected override void OnDisposing() {
                WasDisposed = true;
            }
        }

        static TestDisposable _target;

        Establish context = () =>
            _target = new TestDisposable();

        [Subject("Disposable specifications")]
        public class when_disposing_of_the_objects {
            Because of = () => _target.Dispose();

            It should_invoke_the_on_disposing_method = () =>
                _target.WasDisposed.ShouldBeTrue();
        }

        [Subject("Disposable specifications")]
        public class when_deconstructing_the_object {
            static Exception _exception;

            Because of = () => {
                _exception = Catch.Exception(() => new TestDisposable());
            };

            It should_work_without_issue = () =>
                _exception.ShouldBeNull();
        }
    }
}
