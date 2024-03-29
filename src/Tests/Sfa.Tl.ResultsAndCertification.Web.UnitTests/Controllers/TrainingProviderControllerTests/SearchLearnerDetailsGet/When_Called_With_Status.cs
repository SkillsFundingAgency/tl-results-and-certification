﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerDetailsGet
{
    public class When_Called_With_Status
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { true, true, true, true },
                    new object[] { false, true, true, false },
                    new object[] { true, false, true, false },
                    new object[] { true, true, false, false },
                    new object[] { false, false, false, false }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(bool isMathsAdded, bool isEnglishAdded, bool isIpAdded, bool expectedStatus)
        {
            var viewModel = new SearchLearnerDetailsViewModel { IsMathsAdded = isMathsAdded, IsEnglishAdded = isEnglishAdded, IsIndustryPlacementAdded = isIpAdded };
            viewModel.IsStatusCompleted.Should().Be(expectedStatus);
        }
    }
}
