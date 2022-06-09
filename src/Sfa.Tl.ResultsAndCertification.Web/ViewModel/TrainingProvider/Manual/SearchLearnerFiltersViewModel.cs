using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerFiltersViewModel
    {
        public IList<FilterLookupData> AcademicYears { get; set; }
        public IList<FilterLookupData> Status { get; set; }
        public IList<FilterLookupData> Tlevels { get; set; }
        public bool IsApplyFiltersSelected { get; set; }

        public IList<string> SelectedFilters
        {
            get
            {
                var selectedFilters = new List<string>();

                if(AcademicYears != null && AcademicYears.Any())
                {
                    selectedFilters.AddRange(AcademicYears.Where(a => a.IsSelected).Select(a => a.Name));
                }

                if (Status != null && Status.Any())
                {
                    selectedFilters.AddRange(Status.Where(s => s.IsSelected).Select(s => s.Name));
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
