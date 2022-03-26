using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsComponentExamViewModel
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentSeries { get; set; }
        public string Grade { get; set; }        
        public string LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime RommEndDate { get; set; }
        public DateTime AppealEndDate { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public ComponentType ComponentType { get; set; }
        public string PrsDisplayText { get { return CommonHelper.GetPrsStatusDisplayText(PrsStatus, RommEndDate, AppealEndDate); } }
        public bool IsAddRommAllowed => IsGradeExists && (PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) 
                                     && CommonHelper.IsRommAllowed(RommEndDate);
        public bool IsAddRommOutcomeAllowed => PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.UnderReview;
        public bool IsAddAppealAllowed => PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Reviewed && CommonHelper.IsAppealsAllowed(AppealEndDate);
        public bool IsRequestChangeAllowed => ((PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) && !CommonHelper.IsRommAllowed(RommEndDate))
                                           || (PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Reviewed && !CommonHelper.IsAppealsAllowed(AppealEndDate))
                                           || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Final;

        public string RommRouteName { get { return RouteConstants.PrsAddRomm; } }
        public string RommOutcomeRouteName { get { return RouteConstants.PrsAddRommOutcome; } }
        public string AppealRouteName { get { return RouteConstants.PrsAddAppeal; } }
        public string PrsGradeChangeRequestRouteName { get { return RouteConstants.PrsGradeChangeRequest; } }

        public Dictionary<string, string> RommRouteAttributes 
        { 
            get 
            { 
                return new Dictionary<string, string> 
                { 
                    { Constants.ProfileId, ProfileId.ToString() },
                    { Constants.AssessmentId, AssessmentId.ToString() },
                    { Constants.ComponentType, ((int)ComponentType).ToString() }
                };
            }
        }

        private bool IsGradeExists => AssessmentId > 0 && !string.IsNullOrWhiteSpace(Grade);
    }
}