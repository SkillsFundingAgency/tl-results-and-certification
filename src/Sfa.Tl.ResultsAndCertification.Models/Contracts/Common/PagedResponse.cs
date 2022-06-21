using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Common
{
    public class PagedResponse<T>
    {
        public int TotalRecords { get; set; }
        public IList<T> Records { get; set; }
        public Pager PagerInfo { get; set; }

        public static PagedResponse<T> ToPagedList(IEnumerable<T> source, int totalRecords, int pageNumber, int pageSize)
        {
            var pager = new Pager(source.Count(), pageNumber, pageSize);
            var items = source.Skip((pager.CurrentPage - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResponse<T> { Records = items, TotalRecords = totalRecords, PagerInfo = pager };
        }
    }
}