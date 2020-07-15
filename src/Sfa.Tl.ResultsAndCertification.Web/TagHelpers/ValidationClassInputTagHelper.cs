using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sfa.Tl.ResultsAndCertification.Web.TagHelpers
{
    // eg: <input id="day" asp-for="Day" sfa-input-validation-for="Day">
    [HtmlTargetElement("input", Attributes = ValidationForAttributeName)]
    public class ValidationClassInputTagHelper : TagHelper
    {
        public const string ValidationClassName = "govuk-input--error";
        public const string ValidationForAttributeName = "sfa-input-validation-for";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out var entry);

            if (entry == null || !entry.Errors.Any()) return;

            var tagBuilder = new TagBuilder(context.TagName);
            tagBuilder.AddCssClass(ValidationClassName);
            output.MergeAttributes(tagBuilder);
        }
    }
}
