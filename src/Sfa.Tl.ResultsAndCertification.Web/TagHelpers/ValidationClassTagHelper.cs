using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sfa.Tl.ResultsAndCertification.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = ValidationClassName)]
    [HtmlTargetElement("div", Attributes = ValidationForAttributeName + "," + ValidationClassName)]
    [HtmlTargetElement("input", Attributes = ValidationForAttributeName + "," + ValidationClassName)]
    [HtmlTargetElement("select", Attributes = ValidationForAttributeName + "," + ValidationClassName)]
    [HtmlTargetElement("fieldset", Attributes = ValidationForAttributeName + "," + ValidationClassName)]
    public class ValidationClassTagHelper : TagHelper
    {
        public const string ValidationClassName = "sfa-validation-class";
        public const string ValidationForAttributeName = "sfa-validation-for";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(ValidationClassName)]
        public string ValidationErrorClass { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.ViewData.ModelState.IsValid)
                return;

            if (For != null)
            {
                ViewContext.ViewData.ModelState.TryGetValue(For.Name, out var entry);

                if (entry == null || !entry.Errors.Any()) return;
            }
            
            var tagBuilder = new TagBuilder(context.TagName);
            tagBuilder.AddCssClass(ValidationErrorClass);
            output.MergeAttributes(tagBuilder);
        }
    }
}
