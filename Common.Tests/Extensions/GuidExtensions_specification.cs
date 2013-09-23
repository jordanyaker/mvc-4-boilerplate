namespace Boilerplate.Common.Tests.Extensions {
    using Machine.Specifications;
    using System;

    public class GuidExtensions_specification {
        [Subject("GuidExtensions specification")]
        public class when_encoding_a_guid {
            static Guid _input = Guid.NewGuid();

            It should_return_an_encoded_version_of_the_guid = () =>
                _input.Encode()
                    .ShouldNotEqual(_input.ToString());
        }
    }
}
