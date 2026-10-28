using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests
{
    public class ChangeUlnMappingTests
    {
        [Fact]
        public async Task Review_is_mapped_to_api_request_with_audit_details_and_current_user()
        {
            var accessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.GivenName, "Admin"), new Claim(ClaimTypes.Surname, "User")
                    }))
                }
            };
            var configuration = new MapperConfiguration(c => c.AddProfile<AdminDashboardMapper>());
            var mapper = configuration.CreateMapper(t => Activator.CreateInstance(t, accessor));
            var client = Substitute.For<IResultsAndCertificationInternalApiClient>();
            client.ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnRequest>()).Returns(true);
            var loader = new AdminDashboardLoader(client, mapper, new ResultsAndCertificationConfiguration());
            var model = new ReviewChangeUlnViewModel
            {
                RegistrationPathwayId = 3, Uln = 9999999990, UlnTo = "9999999991",
                ContactName = "Contact", ChangeReason = "Correction", ZendeskId = "123",
                Day = "1", Month = "2", Year = "2026"
            };

            Assert.True(await loader.ProcessChangeUlnAsync(model));

            await client.Received(1).ProcessChangeUlnAsync(Arg.Is<ReviewChangeUlnRequest>(r =>
                r.RegistrationPathwayId == 3 && r.ChangeUlnDetails.UlnFrom == 9999999990
                && r.ChangeUlnDetails.UlnTo == 9999999991 && r.ContactName == "Contact"
                && r.ChangeReason == "Correction" && r.ZendeskId == "123"
                && r.RequestDate == new DateTime(2026, 2, 1) && r.CreatedBy == "Admin User"));
        }
    }
}
