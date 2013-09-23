namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class HtmlHelperExtensions {
        public static MvcHtmlString ToNumeric(this HtmlHelper htmlHelper, int value) {
            string result = "";

            if (value > 1000000) {
                result = Math.Round((decimal)value / (decimal)1000000, 1).ToString() + "M";
            } else if (value > 1000) {
                result = Math.Round((decimal)value / (decimal)1000, 1).ToString() + "K";
            } else if (value > -1000) {
                result = value.ToString("0");
            } else if (value > -1000000) {
                result = Math.Round((decimal)value / (decimal)1000, 1).ToString() + "K";
            } else {
                result = Math.Round((decimal)value / (decimal)1000000, 1).ToString() + "M";
            }

            return new MvcHtmlString(result);
        }
        public static MvcHtmlString ToNumeric(this HtmlHelper htmlHelper, double value) {
            return ToNumeric(htmlHelper, (int)Math.Truncate(value));
        }
        public static MvcHtmlString ValidationBanner(this HtmlHelper htmlHelper) {
            return ValidationBanner(htmlHelper, false /* excludePropertyErrors */ );
        }
        public static MvcHtmlString ValidationBanner(this HtmlHelper htmlHelper, bool excludePropertyErrors) {
            return ValidationBanner(htmlHelper, excludePropertyErrors, null /* htmlAttributes */);
        }
        public static MvcHtmlString ValidationBanner(this HtmlHelper htmlHelper, object htmlAttributes) {
            return ValidationBanner(htmlHelper, false /* excludePropertyErrors */, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static MvcHtmlString ValidationBanner(this HtmlHelper htmlHelper, bool excludePropertyErrors, object htmlAttributes) {
            return ValidationBanner(htmlHelper, excludePropertyErrors, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }
        public static MvcHtmlString ValidationBanner(this HtmlHelper htmlHelper, IDictionary<string, object> htmlAttributes) {
            return ValidationBanner(htmlHelper, false /* excludePropertyErrors */, htmlAttributes);
        }
        public static MvcHtmlString ValidationBanner(this HtmlHelper htmlHelper, bool excludePropertyErrors, IDictionary<string, object> htmlAttributes) {
            if (htmlHelper == null) {
                throw new ArgumentNullException("htmlHelper");
            }

            FormContext formContext = htmlHelper.ViewContext.ClientValidationEnabled ?
                htmlHelper.ViewContext.FormContext :
                null;
            if (formContext == null && htmlHelper.ViewData.ModelState.IsValid) {
                return null;
            }

            IEnumerable<ModelState> modelStates = null;
            if (excludePropertyErrors) {
                ModelState ms;
                htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, out ms);
                if (ms != null) {
                    modelStates = new ModelState[] { ms };
                }
            } else {
                modelStates = htmlHelper.ViewData.ModelState.Values;
            }

            string errorText = null;
            if (modelStates != null && modelStates.Any(x => x.Errors.Count > 0)) {
                errorText = modelStates.First(x => x.Errors.Count > 0)
                    .Errors.First().ErrorMessage;
            }

            TagBuilder errorLabelBuilder = new TagBuilder("strong");
            errorLabelBuilder.SetInnerText("ERROR: ");

            TagBuilder divBuilder = new TagBuilder("div");
            divBuilder.AddCssClass("alert alert-error");
            divBuilder.AddCssClass(htmlHelper.ViewData.ModelState.IsValid ? 
                HtmlHelper.ValidationSummaryValidCssClassName : 
                HtmlHelper.ValidationSummaryCssClassName);
            divBuilder.InnerHtml = errorLabelBuilder.ToString(TagRenderMode.Normal) +
                errorText;

            if (formContext != null) {
                if (htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled) {
                    if (!excludePropertyErrors) {
                        divBuilder.MergeAttribute("data-valmsg-banner", "true");
                    }
                } else {
                    divBuilder.GenerateId("validationSummary");
                    formContext.ValidationSummaryId = divBuilder.Attributes["id"];
                    formContext.ReplaceValidationSummary = !excludePropertyErrors;
                }
            }

            if (htmlAttributes != null) {
                divBuilder.MergeAttributes(htmlAttributes);
            }

            return MvcHtmlString.Create(divBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
