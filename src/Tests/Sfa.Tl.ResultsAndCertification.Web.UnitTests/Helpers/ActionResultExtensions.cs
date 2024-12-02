using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers
{
    public static class ActionResultExtensions
    {
        public static void ShouldBeRedirectHome(this IActionResult actionResult)
            => ShouldBeRedirectToRouteResult(actionResult, RouteConstants.Home);

        public static void ShouldBeRedirectPageNotFound(this IActionResult actionResult)
            => ShouldBeRedirectToRouteResult(actionResult, RouteConstants.PageNotFound);

        public static void ShouldBeRedirectToProblemWithService(this IActionResult actionResult)
           => ShouldBeRedirectToRouteResult(actionResult, RouteConstants.ProblemWithService);

        public static void ShouldBeRedirectToRouteResult(this IActionResult actionResult, string routeName, params (string key, object value)[] values)
        {
            actionResult.Should().NotBeNull().And.BeOfType<RedirectToRouteResult>();

            var redirectToRouteResult = actionResult as RedirectToRouteResult;
            redirectToRouteResult.RouteName.Should().Be(routeName);

            if (!values.IsNullOrEmpty())
            {
                redirectToRouteResult.RouteValues.Should().HaveCount(values.Length);

                foreach (var (key, value) in values)
                {
                    redirectToRouteResult.RouteValues.Should().ContainEquivalentOf(new KeyValuePair<string, object>(key, value));

                }
            }
        }

        public static void ShouldBeRedirectToActionResult(this IActionResult actionResult, string routeName, params (string key, object value)[] values)
        {
            actionResult.Should().NotBeNull().And.BeOfType<RedirectToActionResult>();

            var redirectToRouteResult = actionResult as RedirectToActionResult;
            redirectToRouteResult.ActionName.Should().Be(routeName);

            if (!values.IsNullOrEmpty())
            {
                redirectToRouteResult.RouteValues.Should().HaveCount(values.Length);

                foreach (var (key, value) in values)
                {
                    redirectToRouteResult.RouteValues.Should().ContainEquivalentOf(new KeyValuePair<string, object>(key, value));

                }
            }
        }

        public static TModel ShouldBeViewResult<TModel>(this IActionResult actionResult)
        {
            actionResult.Should().NotBeNull().And.BeOfType<ViewResult>();
            var viewResult = actionResult as ViewResult;

            viewResult.Model.Should().NotBeNull().And.BeOfType<TModel>();
            return (TModel)viewResult.Model;
        }
    }
}