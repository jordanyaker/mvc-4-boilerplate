namespace Boilerplate.Common.Tests.Extensions {
    using Machine.Specifications;
    using System.Collections.Generic;

    public class StringExtensions_specification {
        [Subject("StringExtensions specification")]
        public class when_getting_a_hashed_string {
            static string _output, _input = "input string";

            Because of = () =>
                _output = _input.GetHashed();

            It should_not_return_the_same_string = () =>
                _output.ShouldNotEqual(_input);
        }

        [Subject("StringExtensions specification")]
        public class when_formatting_a_string {
            static string _pattern = "This is the {0}st of {1} formatted strings.";
            static int _value1 = 1, _value2 = 2;
            static string _expected = "This is the 1st of 2 formatted strings.";

            It should_return_the_correct_result = () =>
                _pattern.FormatWith(_value1, _value2)
                    .ShouldEqual(_expected);
        }

        [Subject("StringExtensions specification")]
        public class when_encrypting_a_string {
            static string _input = "This is not encrypted.";
            static string _key;

            It should_not_return_the_same_string = () =>
                _input.Encrypt(out _key)
                    .ShouldNotEqual(_input);

            [Subject("StringExtensions specification, when encrypting a string")]
            public class and_then_decrypting_it {
                static string _encrypted;

                Establish context = () => {
                    _encrypted = _input.Encrypt(out _key);
                };

                It should_return_the_original_value = () =>
                    _encrypted.Decrypt(_key)
                        .ShouldEqual(_input);
            }
        }

        [Subject("StringExtensions specification")]
        public class when_deserializing_an_object {
            static Dictionary<string, int> _input, _output;
            static string _serialized;

            Establish context = () => {
                _input = new Dictionary<string, int> {
                    { "Test 1", 123456789 },
                    { "Test 2", 987654321 }
                };

                _serialized = _input.ToSerialized();
            };

            It should_return_the_correct_object = () => {
                _output = _serialized.ToDeserialized<Dictionary<string, int>>();

                _output.ShouldNotBeNull();
                _output["Test 1"].ShouldEqual(123456789);
                _output["Test 2"].ShouldEqual(987654321);
            };
        }

        [Subject("StringExtensions specification")]
        public class when_getting_an_enum_representation_of_a_string {
            static System.ConsoleKey _expected = System.ConsoleKey.BrowserSearch;

            It should_return_the_correct_value = () =>
                "BrowserSearch".ToEnum<System.ConsoleKey>(System.ConsoleKey.Clear)
                    .ShouldEqual(_expected);

            [Subject("StringExtensions specification, when getting an enum representation of a string")]
            public class and_the_input_cannot_be_parsed {
                It should_use_the_default_value = () =>
                    "dsfsdfhgdsh".ToEnum<System.ConsoleKey>(System.ConsoleKey.BrowserSearch)
                        .ShouldEqual(_expected);
            }

            [Subject("StringExtensions specification, when getting an enum representation of a string")]
            public class and_the_specified_type_is_not_an_enum {
                It should_throw_an_exception = () =>
                    Catch.Exception(() => "test".ToEnum<int>(0));
            }
        }
    }
}
