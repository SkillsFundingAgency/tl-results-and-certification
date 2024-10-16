using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;
using System.Collections.Generic;
using RequestReplacementDocumentContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.AdminRequestReplacementDocument;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminRequestReplacementDocumentViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public long Uln { get; set; }

        public string LearnerName { get; set; }

        public string ProviderName { get; set; }

        public long ProviderUkprn { get; set; }

        public AddressViewModel ProviderAddress { get; set; }

        public int? PrintCertificateId { get; set; }

        public PrintCertificateType? PrintCertificateType { get; set; }

        public DateTime? LastDocumentRequestedDate { get; set; }

        public SummaryItemModel SummaryDocumentRequested =>
            new()
            {
                Id = "requestDocumentType",
                Title = RequestReplacementDocumentContent.Title_Document_Requested,
                Value = GetDocumentRequestedValue(PrintCertificateType)
            };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = RequestReplacementDocumentContent.Title_Provider_Ukprn_Name_Text,
            Value = $"{ProviderName} ({ProviderUkprn})"
        };

        public SummaryItemModel SummaryDepartment =>
            new()
            {
                Id = "departmentName",
                Title = RequestReplacementDocumentContent.Title_Department,
                Value = ProviderAddress.DepartmentName,
            };

        public SummaryItemModel SummaryOrganisationAddress =>
            new()
            {
                Id = "organisationAddress",
                Title = RequestReplacementDocumentContent.Title_Organisation_Address,
                Value = ProviderAddress.ToDisplayValue,
                IsRawHtml = true,
            };

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

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