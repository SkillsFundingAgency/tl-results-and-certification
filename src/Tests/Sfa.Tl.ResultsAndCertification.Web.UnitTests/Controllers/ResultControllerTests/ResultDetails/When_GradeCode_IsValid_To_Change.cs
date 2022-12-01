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
    public class When_CoreGrade_IsValid_To_Change : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;

        public override void Given()
        {
            // Mock - ResultEndDate are passed, Core GradeCode = "PCG8" and Specialism "SCG5" Then IsResultChangeAllowed should set to True
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
                CoreComponentExams = new List<ComponentExamViewModel>
                {
                    new ComponentExamViewModel
                    {
                        AssessmentSeries = "Autumn 2022",
                        Grade = "Q - pending result",
                        GradeCode = "PCG8",
                        ResultEndDate = DateTime.Today.AddDays(-10), // DatePassed
                        ComponentType = ComponentType.Core
                    }
                },

                // Specialisms
                SpecialismComponents = new List<SpecialismComponentViewModel>
                {
                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Plumbing",
                        LarId = "S111",
                        SpecialismComponentExams = new List<ComponentExamViewModel>
                        {
                            new ComponentExamViewModel
                            {
                                AssessmentSeries = "Summer 2022",
                                Grade = "Q - pending result",
                                GradeCode = "SCG5",
                                ResultEndDate = DateTime.Today.AddDays(-10),
                                ComponentType = ComponentType.Specialism
                            }
                        }
                    },

                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Heating",
                        LarId = "S222",
                        SpecialismComponentExams = new List<ComponentExamViewModel>
                        {
                            new ComponentExamViewModel
                            {
                                AssessmentSeries = "Summer 2022",
                                Grade = "Q - pending result",
                                GradeCode = "SCG5",
                                ResultEndDate = DateTime.Today.AddDays(-10),
                                ComponentType = ComponentType.Specialism
                            }
                        }
                    }
                }
            };

            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_IsResultChangeAllowed_Is_True()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultDetailsViewModel));

            var model = viewResult.Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            foreach (var exam in model.CoreComponentExams)
            {
                exam.IsResultChangeAllowed.Should().BeTrue();
            }

            foreach (var specialism in model.SpecialismComponents)
            {
                foreach (var exam in specialism.SpecialismComponentExams)
                {
                    exam.IsResultChangeAllowed.Should().BeTrue();
                }
            }
        }
    }
}
