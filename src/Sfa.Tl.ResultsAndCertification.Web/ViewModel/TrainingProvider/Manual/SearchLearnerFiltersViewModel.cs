using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerFiltersViewModel
    {
        public IList<FilterLookupData> AcademicYears { get; set; }
        public IList<FilterLookupData> IndustryPlacementStatus { get; set; }
        public IList<FilterLookupData> Tlevels { get; set; }
        public bool IsApplyFiltersSelected { get; set; }

        public IList<string> SelectedFilters
        {
            get
            {
                var selectedFilters = new List<string>();

                if (AcademicYears != null && AcademicYears.Any())
                {
                    selectedFilters.AddRange(AcademicYears.Where(a => a.IsSelected).Select(a => a.Name));
                }

                if (IndustryPlacementStatus != null)
                {
                    selectedFilters.AddRange(IndustryPlacementStatus.Where(s => s.IsSelected).Select(s => s.Name));
                }

                if (Tlevels != null && Tlevels.Any())
                {
                    selectedFilters.AddRange(Tlevels.Where(t => t.IsSelected).Select(t => t.Name));
                }

                return selectedFilters;
            }
        }
    }
}
