using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord
{
    public class AdminAssessmentDetailsViewModel
    {
        public int RegistrationPathwayId { get; set; }

        #region Core component

        public string PathwayDisplayName { get; set; }

        public bool HasAssessmentEntries => !PathwayAssessments.IsNullOrEmpty();

        public IList<AdminAssessmentViewModel> PathwayAssessments { get; set; }

        #endregion

        #region Occupational specialism

        public bool HasSpecialismRegistered => !SpecialismDetails.IsNullOrEmpty();

        public IList<AdminSpecialismViewModel> SpecialismDetails { get; set; }

        #endregion
    }
}