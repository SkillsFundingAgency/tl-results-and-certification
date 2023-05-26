using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RequestReplacementDocumentContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.RequestReplacementDocument;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class RequestReplacementDocumentViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }

        public AddressViewModel ProviderAddress { get; set; }
        public int? PrintCertificateId { get; set; }
        public PrintCertificateType? PrintCertificateType { get; set; }
        public DateTime? LastDocumentRequestedDate { get; set; }

        public SummaryItemModel SummaryDocumentRequested =>
            new SummaryItemModel
            {
                Id = "requestDocumentType",
                Title = RequestReplacementDocumentContent.Title_Document_Requested,
                Value = GetDocumentRequestedValue(PrintCertificateType)
            };

        public SummaryItemModel SummaryLearnerName =>
           new SummaryItemModel
           {
               Id = "learnerName",
               Title = RequestReplacementDocumentContent.Title_Learner_Name,
               Value = LearnerName
           };

        public SummaryItemModel SummaryULN =>
           new SummaryItemModel
           {
               Id = "ULN",
               Title = RequestReplacementDocumentContent.Uln_Text,
               Value = Uln.ToString()
           };

        public SummaryItemModel SummaryDepartment =>
            new SummaryItemModel
            {
                Id = "departmentName",
                Title = RequestReplacementDocumentContent.Title_Department,
                Value = ProviderAddress.DepartmentName,
            };


        public SummaryItemModel SummaryOrganisationAddress =>
            new SummaryItemModel
            {
                Id = "organisationAddress",
                Title = RequestReplacementDocumentContent.Title_Organisation_Address,
                Value = ProviderAddress.ToDisplayValue,
                IsRawHtml = true,
            };

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Learners_Record, RouteName = RouteConstants.LearnerRecordDetails, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } }
                    }
                };
            }
        }

        private static string GetDocumentRequestedValue(PrintCertificateType? printCertificateType)
        {
            if (printCertificateType == ResultsAndCertification.Common.Enum.PrintCertificateType.Certificate)
                return RequestReplacementDocumentContent.Document_Certificate;

            if (printCertificateType == ResultsAndCertification.Common.Enum.PrintCertificateType.StatementOfAchievement)
                return RequestReplacementDocumentContent.Document_Statement_Of_Achievement;

            return string.Empty;
        }
    }
}
