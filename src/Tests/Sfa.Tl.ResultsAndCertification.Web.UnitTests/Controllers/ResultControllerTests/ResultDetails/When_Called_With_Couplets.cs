using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_Called_With_Couplets : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;

        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel
            {
                Firstname = "John",
                Lastname = "Smith",
                Uln = 5647382910,
                DateofBirth = DateTime.Today,
                ProviderName = "Barnsley College",
                ProviderUkprn = 100656,
                TlevelTitle = "Design, Surveying and Planning for Construction",

                // Core
                CoreComponentDisplayName = "Design, Surveying and Planning (123456)",

                // Specialisms
                SpecialismComponents = new List<SpecialismComponentViewModel>
                {
                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Heating Engineering (10202101)",
                        LarId = "10202101",
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "10202101|10202105") },
                    },
                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Ventilation (10202105)",
                        LarId = "10202105",
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(1, "10202101|10202105") },
                    },
                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Specialism3 (ZT2158963)",
                        LarId = "ZT2158963",
                        TlSpecialismCombinations = new List<KeyValuePair<int,string>>(),
                    }
                }
            };

            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockResult);
        }

        [Fact]
        public void Then_Returns_Expected_RenderSpecialismComponents()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultDetailsViewModel));

            var model = viewResult.Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            // Three Specialisms components --> Two of them will be shown as couplets.
            model.SpecialismComponents.Should().HaveCount(3);
            model.SpecialismComponents[0].IsCouplet.Should().BeTrue();
            model.SpecialismComponents[1].IsCouplet.Should().BeTrue();
            model.SpecialismComponents[2].IsCouplet.Should().BeFalse();

            model.RenderSpecialismComponents.Should().HaveCount(2);
            model.RenderSpecialismComponents[0].SpecialismComponentDisplayName.Should().Be("Heating Engineering (10202101) and Ventilation (10202105)");
            model.RenderSpecialismComponents[1].SpecialismComponentDisplayName.Should().Be("Specialism3 (ZT2158963)");
        }
    }
}
