using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using System.Collections.Generic;
using Xunit;

using IndustryPlacementContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IndustryPlacementBanner;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetBannerSuccessMessage
{
    public class When_Method_Called : TestSetup
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        IndustryPlacementStatus.Completed, 
                        new NotificationBannerModel
                        {
                            HeaderMessage = IndustryPlacementContent.Banner_HeaderMesage,
                            Message = IndustryPlacementContent.Success_Message_Completed,
                            DisplayMessageBody = true,
                            IsRawHtml = true
                        },
                    },
                    new object[]
                    {
                        IndustryPlacementStatus.CompletedWithSpecialConsideration,
                        new NotificationBannerModel
                        {
                            HeaderMessage = IndustryPlacementContent.Banner_HeaderMesage,
                            Message = IndustryPlacementContent.Success_Message_Completed_With_Special_Consideration,
                            DisplayMessageBody = true,
                            IsRawHtml = true
                        }
                    },
                    new object[]
                    {
                        IndustryPlacementStatus.WillNotComplete,
                        new NotificationBannerModel
                        {
                            HeaderMessage = IndustryPlacementContent.Banner_HeaderMesage,
                            Message = IndustryPlacementContent.Success_Message_Will_Not_Complete,
                            DisplayMessageBody = true,
                            IsRawHtml = true
                        }
                    },
                    new object[]
                    {
                        IndustryPlacementStatus.NotCompleted,
                        new NotificationBannerModel
                        {
                            HeaderMessage = IndustryPlacementContent.Banner_HeaderMesage,
                            Message = IndustryPlacementContent.Success_Message_Still_Need_To_Complete,
                            DisplayMessageBody = true,
                            IsRawHtml = true
                        }
                    },
                    new object[]
                    {
                        IndustryPlacementStatus.NotSpecified,
                        new NotificationBannerModel
                        {
                            HeaderMessage = IndustryPlacementContent.Banner_HeaderMesage,
                            Message = string.Empty,
                            DisplayMessageBody = true,
                            IsRawHtml = true
                        }
                    },
                    new object[]
                    {
                        null,
                        new NotificationBannerModel
                        {
                            HeaderMessage = IndustryPlacementContent.Banner_HeaderMesage,
                            Message = string.Empty,
                            DisplayMessageBody = true,
                            IsRawHtml = true
                        }
                    },
                };
            }
        }

        public override void Given() {}

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IndustryPlacementStatus? industryPlacementStatus, NotificationBannerModel expectedNotitificationBanner)
        {
            var actualResult = Loader.GetSuccessNotificationBanner(industryPlacementStatus);
            actualResult.Should().BeEquivalentTo(expectedNotitificationBanner);
        }
    }
}