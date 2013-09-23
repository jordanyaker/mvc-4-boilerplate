namespace Boilerplate.Common.Tests.Extensions {
    using Machine.Specifications;
    using System;
    using System.Runtime.Caching;
    using Boilerplate.Caching;

    public class CachingExtensions_specification {
        [Subject("CachingExtensions specification")]
        public class when_building_a_key {
            It should_return_the_class_name_of_the_type_plus_the_key = () =>
                ":test".BuildFullKey<Int16>()
                    .ShouldEqual("Int16:test");

            public class and_no_key_is_supplied {
                It should_just_use_the_class_name_of_the_type = () =>
                    ((object)null).BuildFullKey<Int16>()
                        .ShouldEqual("Int16");
            }
        }
    }
}
