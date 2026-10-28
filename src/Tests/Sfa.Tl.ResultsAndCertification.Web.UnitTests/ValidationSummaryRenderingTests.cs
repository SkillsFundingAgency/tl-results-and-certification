using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests
{
    public class ValidationSummaryRenderingTests
    {
        [Theory]
        [InlineData("ulnTo", "Enter a ULN with 10 digits")]
        [InlineData("ulnTo", "ULN is already registered")]
        [InlineData("UlnTo", "Enter a ULN with 10 digits")]
        [InlineData("UlnTo", "ULN is already registered")]
        public async Task Summary_displays_errors_for_bound_field_names_regardless_of_case(string key, string error)
        {
            var assembly = typeof(AdminDashboardController).Assembly;
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ApplicationName = assembly.GetName().Name,
                EnvironmentName = "Production"
            });
            builder.Services.AddControllersWithViews().AddApplicationPart(assembly);
            await using var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var httpContext = new DefaultHttpContext { RequestServices = services };
            var modelState = new ModelStateDictionary();
            // MVC retains the casing of the original bound key when an error is added.
            modelState.SetModelValue(key, "123", "123");
            modelState.AddModelError(nameof(AdminChangeUlnViewModel.UlnTo), error);
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), modelState);
            var viewData = new ViewDataDictionary<AdminChangeUlnViewModel>(
                services.GetRequiredService<IModelMetadataProvider>(), modelState)
            {
                Model = new AdminChangeUlnViewModel { UlnTo = "123" }
            };
            var view = services.GetRequiredService<IRazorViewEngine>()
                .GetView(null, "/Views/Shared/_ValidationSummary.cshtml", false);
            Assert.True(view.Success);
            using var writer = new StringWriter();
            var viewContext = new ViewContext(actionContext, view.View, viewData,
                new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>()), writer, new HtmlHelperOptions());

            await view.View.RenderAsync(viewContext);

            Assert.Contains(error, writer.ToString());
            Assert.Contains("href=\"#ulnto\"", writer.ToString());
        }
    }
}
