using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessIndustryPlacementDetails
{
    public class When_Called_With_IpStatus_Completed : TestSetup
    {
        private readonly bool _expectedApiResult = true;
        private IndustryPlacementDetails _industryPlacementDetails;

        public override void Given()
        {
            CreateMapper();

            ProviderUkprn = 987654321;

            ViewModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel
                {
                    ProfileId = 1,
                    RegistrationPathwayId = 1,
                    PathwayId = 7,
                    IndustryPlacementStatus = IndustryPlacementStatus.Completed
                },
                IpModelViewModel = new IpModelViewModel
                {
                    IpModelUsed = new IpModelUsedViewModel
                    {
                        IsIpModelUsed = true
                    },
                    IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel
                    {
                        IsMultiEmployerModelUsed = true
                    },
                    IpMultiEmployerOther = new IpMultiEmployerOtherViewModel
                    {
                        OtherIpPlacementModels = new List<IpLookupDataViewModel>
                        {
                            new IpLookupDataViewModel
                            {
                                Id = 1,
                                Name = "Test 1",
                                IsSelected = true
                            },
                            new IpLookupDataViewModel
                            {
                                Id = 2,
                                Name = "Test 2",
                                IsSelected = true
                            },
                            new IpLookupDataViewModel
                            {
                                Id = 3,
                                Name = "Test 3",
                                IsSelected = false
                            }
                        }
                    }
                },
                TempFlexibility = new IpTempFlexibilityViewModel
                {
                    IpTempFlexibilityUsed = new IpTempFlexibilityUsedViewModel
                    {
                        IsTempFlexibilityUsed = true
                    },
                    IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel
                    {
                        IsBlendedPlacementUsed = false
                    },
                    IpGrantedTempFlexibility = new IpGrantedTempFlexibilityViewModel
                    {
                        TemporaryFlexibilities = new List<IpLookupDataViewModel>
                        {
                            new IpLookupDataViewModel
                            {
                                Id = 4,
                                Name = "Temp Flex 1",
                                IsSelected = true
                            },
                            new IpLookupDataViewModel
                            {
                                Id = 5,
                                Name = "Temp Flex 2",
                                IsSelected = true
                            },
                            new IpLookupDataViewModel
                            {
                                Id = 6,
                                Name = "Temp Flex 3",
                                IsSelected = false
                            }
                        }
                    }
                }
            };

            _industryPlacementDetails = new IndustryPlacementDetails
            {
                IndustryPlacementStatus = IndustryPlacementStatus.Completed.ToString(),
                HoursSpentOnPlacement = null,
                SpecialConsiderationReasons = new List<int?>(),
                IndustryPlacementModelsUsed = true,
                MultipleEmployerModelsUsed = true,
                OtherIndustryPlacementModels = new List<int?> { 1, 2 },
                IndustryPlacementModels = new List<int?>(),
                TemporaryFlexibilitiesUsed = true,
                BlendedTemporaryFlexibilityUsed = false,
                TemporaryFlexibilities = new List<int?> { 4, 5 }
            };

            InternalApiClient.ProcessIndustryPlacementDetailsAsync(Arg.Is<IndustryPlacementRequest>(x =>
                                x.ProfileId == ViewModel.IpCompletion.ProfileId &&
                                x.RegistrationPathwayId == ViewModel.IpCompletion.RegistrationPathwayId &&
                                x.ProviderUkprn == ProviderUkprn &&
                                x.IndustryPlacementStatus == ViewModel.IpCompletion.IndustryPlacementStatus &&
                                x.IndustryPlacementDetails.IndustryPlacementStatus == _industryPlacementDetails.IndustryPlacementStatus &&
                                x.IndustryPlacementDetails.HoursSpentOnPlacement == _industryPlacementDetails.HoursSpentOnPlacement &&
                                x.IndustryPlacementDetails.SpecialConsiderationReasons.Count == _industryPlacementDetails.SpecialConsiderationReasons.Count &&
                                x.IndustryPlacementDetails.IndustryPlacementModelsUsed == _industryPlacementDetails.IndustryPlacementModelsUsed &&
                                x.IndustryPlacementDetails.MultipleEmployerModelsUsed == _industryPlacementDetails.MultipleEmployerModelsUsed &&
                                x.IndustryPlacementDetails.OtherIndustryPlacementModels.Count == _industryPlacementDetails.OtherIndustryPlacementModels.Count &&
                                x.IndustryPlacementDetails.IndustryPlacementModels.Count == _industryPlacementDetails.IndustryPlacementModels.Count &&
                                x.IndustryPlacementDetails.TemporaryFlexibilitiesUsed == _industryPlacementDetails.TemporaryFlexibilitiesUsed &&
                                x.IndustryPlacementDetails.BlendedTemporaryFlexibilityUsed == _industryPlacementDetails.BlendedTemporaryFlexibilityUsed &&
                                x.IndustryPlacementDetails.TemporaryFlexibilities.Count == _industryPlacementDetails.TemporaryFlexibilities.Count &&
                                x.PerformedBy == $"{Givenname} {Surname}"
                                )).Returns(_expectedApiResult);

            Loader = new IndustryPlacementLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }       
    }
}
