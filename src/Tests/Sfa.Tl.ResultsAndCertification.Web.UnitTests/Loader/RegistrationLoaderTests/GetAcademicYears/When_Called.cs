using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetAcademicYears
{
    public class When_Called : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new List<AcademicYear>
            {
                new AcademicYear
                {
                    Id = 1,
                    Name = "2020/21",
                    Year = 2020
                },
                new AcademicYear
                {
                    Id = 2,
                    Name = "2021/22",
                    Year = 2021
                },
                new AcademicYear
                {
                    Id = 3,
                    Name = "2022/23",
                    Year = 2022
                }
            };

            InternalApiClient.GetAcademicYearsAsync().Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Should().BeEquivalentTo(expectedApiResult);
        }
    }
}
