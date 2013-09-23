namespace Boilerplate.Common.Tests.Extensions {
    using Machine.Specifications;

    public class ObjectExtensions_specification {
        [Subject("ObjectExtensions specification")]
        public class when_serializing_an_object {
            static object _input = new { test = "testing" };

            It should_return_a_valid_string = () =>
                _input.ToSerialized()
                    .ShouldNotBeEmpty();
        }
    }
}
