using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sfa.Tl.ResultsAndCertification.Web.TagHelpers
{
    [HtmlTargetElement("span", Attributes = ValidationForAttributeName)]
    public class ValidationErrorTagHelper : TagHelper
    {
        public const string ValidationForAttributeName = "sfa-validation-for";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out var entry);

            if (entry == null || !entry.Errors.Any()) return;

            var errorBuilder = new TagBuilder("span");
            errorBuilder.AddCssClass("govuk-visually-hidden");
            errorBuilder.InnerHtml.Append("Error:");

            var tagBuilder = new TagBuilder(context.TagName);
            tagBuilder.Attributes.Add("id", For.Name.ToLowerInvariant() + "-error-message");
            tagBuilder.Attributes.Add("class", "govuk-error-message");

            output.Content.SetHtmlContent(errorBuilder);
            output.Content.Append(entry.Errors[0].ErrorMessage);
            output.MergeAttributes(tagBuilder);
        }
    }
}
