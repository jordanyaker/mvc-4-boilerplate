namespace Boilerplate.Bootstrapper {
    using Cassette;
    using Cassette.Scripts;
    using Cassette.Stylesheets;

    public class CassetteBundlesConfiguration : IConfiguration<BundleCollection> {
        public void Configure(BundleCollection bundles) {
            //---------------------------------------------------------------------
            // CSS Bundles
            //---------------------------------------------------------------------
            bundles.Add<StylesheetBundle>("css",
                new[] { 
                    "~/Content/Stylesheets/jquery.ui.core.css",
                    "~/Content/Stylesheets/jquery.ui.resizable.css",
                    "~/Content/Stylesheets/jquery.ui.selectable.css",
                    "~/Content/Stylesheets/jquery.ui.accordion.css",
                    "~/Content/Stylesheets/jquery.ui.autocomplete.css",
                    "~/Content/Stylesheets/jquery.ui.button.css",
                    "~/Content/Stylesheets/jquery.ui.dialog.css",
                    "~/Content/Stylesheets/jquery.ui.slider.css",
                    "~/Content/Stylesheets/jquery.ui.tabs.css",
                    "~/Content/Stylesheets/jquery.ui.datepicker.css",
                    "~/Content/Stylesheets/jquery.ui.progressbar.css",
                    "~/Content/Stylesheets/jquery.ui.theme.css",
                    "~/Content/Stylesheets/Site.css",
                },
                config => config.EmbedImages());

            bundles.Add<ScriptBundle>("js",
                new[] { 
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/jquery.unobtrusive-ajax.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                });
        }
    }
}
