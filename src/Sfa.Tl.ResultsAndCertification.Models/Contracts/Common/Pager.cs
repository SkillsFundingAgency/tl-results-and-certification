using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Common
{
    public class Pager
    {
        public Pager(int totalItems, int? currentPage, int pageSize)
        {
            var totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);
            var currentPageNumber = currentPage != null ? (int)currentPage : 1;
            var startPage = 1;

            if (currentPageNumber > totalPages)
                currentPageNumber = totalPages;

            var currentPageItemsCount = currentPageNumber * pageSize;
            //var showingRecordFrom = currentPage <= 1 ? 1 : ((currentPage - 1) * pageSize) + 1;
            //var showingRecordTo = currentPageItemsCount > totalItems ? totalItems : currentPageItemsCount;

            TotalItems = totalItems;
            CurrentPage = currentPageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            RecordFrom = currentPageNumber <= 1 ? 1 : ((currentPageNumber - 1) * pageSize) + 1; ;
            RecordTo = currentPageItemsCount > totalItems ? totalItems : currentPageItemsCount;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }

        public int RecordFrom { get; private set; }
        public int RecordTo { get; private set; }
    }
}
