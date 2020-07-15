using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sfa.Tl.ResultsAndCertification.Web.TagHelpers
{
    // eg: <div sfa-group-error-line-class="govuk-form-group--error">
    [HtmlTargetElement("div", Attributes = ValidationClassName)]
    public class ValidationClassVerticalLineHelper : TagHelper
    {
        public const string ValidationClassName = "sfa-group-error-line-class";

        [HtmlAttributeName(ValidationClassName)]
        public string ValidationErrorClass { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.ViewData.ModelState.IsValid)
                return;
            
            var tagBuilder = new TagBuilder(context.TagName);
            tagBuilder.AddCssClass(ValidationErrorClass);
            output.MergeAttributes(tagBuilder);
        }
    }
}
