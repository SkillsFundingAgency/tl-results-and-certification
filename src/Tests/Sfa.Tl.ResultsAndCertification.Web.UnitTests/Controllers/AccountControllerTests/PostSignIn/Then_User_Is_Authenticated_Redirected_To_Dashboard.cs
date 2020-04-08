using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.PostSignIn
{
    public class Then_User_Is_Authenticated_Redirected_To_Dashboard : When_PostSignIn_Is_Called
    {
        public override void Given()
        {
            //var httpContext = new ClaimsIdentityBuilder<AccountController>(Controller)
            //    .Add(CustomClaimTypes.HasAccessToService, "true")
            //    .Build()
            //    .HttpContext;

           
        }

        [Fact]
        public void Then_Redirected_ToDashboard()
        {
            Result.Should().NotBeNull();
        }
    }
}
