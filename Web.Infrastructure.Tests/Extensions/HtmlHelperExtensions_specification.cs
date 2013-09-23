namespace Boilerplate.Web.Infrastructure.Testing.Extensions {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Machine.Specifications;
    using Machine.Fakes;

    public class HtmlHelperExtensions_specification : WithFakes {
        static HtmlHelper _helper;
        static ViewContext _context;

        Establish context = () => {
            _context = new ViewContext();
            _helper = new HtmlHelper(_context, An<IViewDataContainer>());
        };

        [Subject("HtmlHelperExtensions specification")]
        public class when_getting_a_numeric_string {
            It should_use_an_M_to_designate_values_in_the_millions = () => {
                _helper.ToNumeric(1345000).ToHtmlString()
                    .ShouldEqual("1.3M");
            };

            It should_use_an_K_to_designate_values_in_the_thousands = () => {
                _helper.ToNumeric(1345).ToHtmlString()
                    .ShouldEqual("1.3K");
            };

            It should_not_use_any_designations_for_values_below_a_thousand = () => {
                _helper.ToNumeric(123).ToHtmlString()
                    .ShouldEqual("123");
            };
        }
    }
}
