namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common
{
    public class PagerViewModel
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int RecordFrom { get; set; }
        public int RecordTo { get; set; }
    }
}