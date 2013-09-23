namespace Boilerplate.Web.Infrastructure.Testing.Extensions {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Machine.Specifications;
    using Moq.Mvc;
    using System.Web.Routing;

    public class UrlHelperExtensions_specification {
        static UrlHelper _helper;
        static RequestContext _context;

        Establish context =()=> {
            _context = new RequestContext();
            _helper = new UrlHelper(_context);
        };

        [Subject("UrlHelperExtensions specification")]
        public class when_getting_a_friendly_string {
            It should_replace_all_of_the_spaces_with_dashes = () => {
                _helper.ToFriendly("This is a test")
                    .ShouldEqual("this-is-a-test");
            };

            It should_replace_ampersands_with_the_word_and = () => {
                _helper.ToFriendly("This & that")
                    .ShouldEqual("this-and-that");
            };

            It should_remove_single_quotes = () => {
                _helper.ToFriendly("The original's value")
                    .ShouldEqual("the-originals-value");
            };

            It should_replace_all_other_characters_with_dashes = () => {
                _helper.ToFriendly("This is 100% accurate")
                    .ShouldEqual("this-is-100-accurate");
            };
        }
    }
}
