﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsRommGradeChangeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsRommGradeChange;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsRommGradeChangeViewModel : PrsBaseViewModel
    {
        public PrsRommGradeChangeViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsRommGradeChangeContent.Title_Name_Text;
            UlnLabel = PrsRommGradeChangeContent.Title_Uln_Text;
            DateofBirthLabel = PrsRommGradeChangeContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsRommGradeChangeContent.Title_TLevel_Text;
            CoreLabel = PrsRommGradeChangeContent.Title_Core_Text;
            SpecialismLabel = PrsRommGradeChangeContent.Title_Specialism_Text;
            ExamPeriodLabel = PrsRommGradeChangeContent.Title_ExamPeriod_Text;
            GradeLabel = PrsRommGradeChangeContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime RommEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(PrsRommGradeChangeContent), ErrorMessageResourceName = "Validation_Message")]
        public string SelectedGradeCode { get; set; }

        public List<LookupViewModel> Grades { get; set; }

        public bool IsRommOutcomeJourney { get; set; }
        public bool IsChangeMode { get; set; }

        public bool IsValid => (((PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified)
                                && CommonHelper.IsRommAllowed(RommEndDate)) || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.UnderReview)
                               && CommonHelper.IsValidGradeForPrsJourney(GradeCode, ComponentType);

        public override BackLinkModel BackLink => new()
        {
            RouteName = GetRouteName,
            RouteAttributes = GetRouteAttributes
        };

        private string GetRouteName => IsChangeMode ? RouteConstants.PrsRommCheckAndSubmit : GetRommOutcomeJourneyRoute;

        private string GetRommOutcomeJourneyRoute => IsRommOutcomeJourney ? RouteConstants.PrsAddRommOutcome : RouteConstants.PrsAddRommOutcomeKnown;

        private Dictionary<string, string> GetRouteAttributes =>
            IsChangeMode ? null :
            IsRommOutcomeJourney ?
            new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.RommOutcomeTypeId, ((int)RommOutcomeType.GradeChanged).ToString() }
            }
            :
            new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.RommOutcomeKnownTypeId, ((int)RommOutcomeKnownType.GradeChanged).ToString() }
            };
    }
}
