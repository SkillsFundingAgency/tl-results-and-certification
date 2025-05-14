using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpCompletionViewModel
    {
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public int PathwayId { get; set; }
        public int AcademicYear { get; set; }
        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(IpCompletion), ErrorMessageResourceName = "Validation_Message")]
        public IndustryPlacementStatus? IndustryPlacementStatus { get; set; }

        public bool IsChangeJourney { get; set; }

        public bool IsChangeMode { get; set; }

        public string PageTitle { get; set; }

        public string ShowCompletedOptionWithWording =>
            IsPeriodActive(DateTime.Today) ? IpCompletion.Yes_Completed_Option_With_Wording : IpCompletion.Yes_Completed_Option_Text;

        private static bool IsPeriodActive(DateTime date)
        {
            var startDate = new DateTime(date.Year, 05, 10);
            var endDate = new DateTime(date.Year, 07, 31);

            return (date >= startDate && date <= endDate);
        }

        public bool IsValid
        {
            get
            {
                var hasStatusAlready = EnumExtensions.IsValidValue<IndustryPlacementStatus>(IndustryPlacementStatus, exclNotSpecified: true);
                return IsChangeJourney == hasStatusAlready;
            }
        }
        public bool IsIpStatusExists => IndustryPlacementStatus != null && IndustryPlacementStatus != ResultsAndCertification.Common.Enum.IndustryPlacementStatus.NotSpecified;

        public int CompletionAcademicYear { get; set; }

        public virtual BackLinkModel BackLink => new()
        {
            RouteName = IsChangeMode ? RouteConstants.IpCheckAndSubmit : RouteConstants.LearnerRecordDetails,
            RouteAttributes = IsChangeMode ? null : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}