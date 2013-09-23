namespace Boilerplate.Common.Tests.Extensions {
    using Machine.Specifications;
    using System.Linq;
    using System.Collections.Generic;

    public class EnumerableExtensions_specification {
        [Subject("EnumerableExtensions specification")]
        public class when_using_the_foreach_extension {
            static int[] _array = new[] { 1, 2, 3, 4, 5, 6, 7 }, _result = new int[0];
            static int[] _expected = new[] { 1, 8, 27, 64, 125, 216, 343 };

            Because of = () =>
                _array.ForEach(x => {
                    _result = Enumerable.Union(_result, new[] { x * x * x })
                        .ToArray();
                });

            It should_invoke_the_action_for_every_element = () => {
                _result.ShouldContain(_expected);
            };
        }
    }
}
