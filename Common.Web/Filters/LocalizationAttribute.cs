namespace Boilerplate.Web.Filters {
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;

    public class LocalizationAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            object locale = "en-us";
            CultureInfo culture = CultureInfo.CreateSpecificCulture(locale.ToString());

            try {
                filterContext.RouteData.Values.TryGetValue("culture", out locale);
                culture = CultureInfo.CreateSpecificCulture((locale ?? "en-us").ToString());
            } catch (CultureNotFoundException) {
                // Swallow the exception in the event of a data error or unknown culture.
            }

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            base.OnActionExecuting(filterContext);
        }
    }
}
