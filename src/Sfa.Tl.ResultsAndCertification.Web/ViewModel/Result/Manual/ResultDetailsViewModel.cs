using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using System;
using System.Collections.Generic;
using System.Linq;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ResultDetailsViewModel : ResultsBaseViewModel
    {
        public ResultDetailsViewModel()
        {
            // Base Profile Summary
            UlnLabel = ResultDetailsContent.Title_Uln_Text;
            DateofBirthLabel = ResultDetailsContent.Title_DateofBirth_Text;
            ProviderUkprnLabel = ResultDetailsContent.Title_Provider_Ukprn_Text;
            ProviderNameLabel = ResultDetailsContent.Title_Provider_Name_Text;
            TlevelTitleLabel = ResultDetailsContent.Title_TLevel_Text;

            CoreComponentExams = new List<ComponentExamViewModel>();
            SpecialismComponents = new List<SpecialismComponentViewModel>();
        }

        public int ProfileId { get; set; }

        // Core Component
        public string CoreComponentDisplayName { get; set; }
        public bool IsCoreAssessmentEntryRegistered { get { return CoreComponentExams.Any(x => x.AssessmentId > 0); } }
        public IList<ComponentExamViewModel> CoreComponentExams { get; set; }

        // Specialism Components
        public IList<SpecialismComponentViewModel> SpecialismComponents { get; set; }
        public IList<SpecialismComponentViewModel> RenderSpecialismComponents
        {
            get
            {
                // If MultiSpecialisms && NoneHasAssessments && ContainsCouplets then ShowCoupletsTogether
                var showCoupletsTogether = SpecialismComponents.Count > 1 && 
                                           SpecialismComponents.All(x => !x.IsSpecialismAssessmentEntryRegistered) &&
                                           SpecialismComponents.Any(x => x.IsCouplet);
                if (!showCoupletsTogether)
                    return SpecialismComponents;

                var specialismToDisplay = new List<SpecialismComponentViewModel>();
                var processedLarIds = new List<string>();
                foreach (var specialism in SpecialismComponents)
                {
                    if (specialism.IsCouplet)
                    {
                        foreach (var spCombination in specialism.TlSpecialismCombinations)
                        {
                            // Initialize first item
                            var combinedSpecialismLarId = new List<string> { specialism.LarId.ToString() };
                            var combinedDisplayName = specialism.SpecialismComponentDisplayName;

                            // Find partners to join.
                            var isPairFound = false;
                            var otherLarIdsInGroup = spCombination.Value.Split(Constants.PipeSeperator).Except(new List<string> { specialism.LarId }, StringComparer.InvariantCultureIgnoreCase);
                            foreach (var otherLarId in otherLarIdsInGroup)
                            {
                                var otherSpecialism = SpecialismComponents.FirstOrDefault(s => s.LarId.Equals(otherLarId, StringComparison.InvariantCultureIgnoreCase));
                                if (otherSpecialism != null)
                                {
                                    isPairFound = true;
                                    combinedSpecialismLarId.Add(otherSpecialism.LarId);
                                    combinedDisplayName = $"{combinedDisplayName}{Constants.AndSeperator}{otherSpecialism.SpecialismComponentDisplayName}";
                                }
                            }

                            var isAddedAlready = processedLarIds.Any(s => combinedSpecialismLarId.Contains(s));
                            if (isPairFound && !isAddedAlready)
                            {
                                processedLarIds.AddRange(combinedSpecialismLarId);
                                specialismToDisplay.Add(new SpecialismComponentViewModel
                                {
                                    SpecialismComponentDisplayName = combinedDisplayName
                                });
                            }
                        }
                    }
                    else
                    {
                        specialismToDisplay.Add(specialism);
                    }
                }

                return specialismToDisplay;
            }
        }

        public NotificationBannerModel SuccessBanner { get; set; }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Result_Dashboard, RouteName = RouteConstants.ResultsDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Results, RouteName = RouteConstants.SearchResults }
                    }
                };
            }
        }
    }
}