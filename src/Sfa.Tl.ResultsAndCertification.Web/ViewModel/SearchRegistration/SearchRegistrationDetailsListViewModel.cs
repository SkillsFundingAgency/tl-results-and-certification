using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration
{
    public class SearchRegistrationDetailsListViewModel
    {
        public IList<SearchRegistrationDetailsViewModel> RegistrationDetails { get; set; } = new List<SearchRegistrationDetailsViewModel>();

        public PagerViewModel PagerInfo { get; set; }

        public int TotalRecords { get; set; }
    }
}