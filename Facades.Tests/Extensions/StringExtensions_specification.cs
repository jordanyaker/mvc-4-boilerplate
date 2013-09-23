namespace Boilerplate.Facades.Testing.Extensions {
    using Boilerplate.Facades;
    using Machine.Specifications;

    public class StringExtensions_specification {
        [Subject("StringExtensions specification")]
        public class when_getting_an_error_from_an_error_message {
            static int _code = 1000;
            static string _propertyName = "Property";
            static string _description = "This is the message.";
            static string _errorMessage;
            static FacadeError _error;

            Establish context = () =>
                _errorMessage = string.Format("{0};{1};{2}", _code, _propertyName, _description);

            Because of = () =>
                _error = _errorMessage.GetError();

            It should_return_the_correct_error_code = () =>
                _error.Code
                    .ShouldEqual(_code);

            It should_return_the_correct_property_name = () =>
                _error.Property
                    .ShouldEqual(_propertyName);

            It should_return_the_correct_description = () =>
                _error.Description
                    .ShouldEqual(_description);

            [Subject("StringExtensions specification, when getting an error from an error message")]
            public class and_the_error_string_is_invalid {
                Establish context = () =>
                    _errorMessage = "";

                It should_return_an_error_code_of_negative_one = () =>
                    _error.Code
                        .ShouldEqual(0);

                It should_return_an_unknown_property_name = () =>
                    _error.Property
                        .ShouldEqual("Unknown");

                It should_return_an_unknown_property_description = () =>
                    _error.Description
                        .ShouldEqual("An unknown error has occurred.");
            }
        }
    }
}
