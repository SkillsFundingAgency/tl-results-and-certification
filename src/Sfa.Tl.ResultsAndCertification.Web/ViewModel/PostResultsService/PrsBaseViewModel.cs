using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public abstract class PrsBaseViewModel
    {
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }        
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string TlevelTitle { get; set; }
        public string CoreDisplayName { get; set; }
        public string ExamPeriod { get; set; }
        public string Grade { get; set; }
        public ComponentType ComponentType { get; set; }

        protected string UlnLabel { get; set; }
        protected string LearnerNameLabel { get; set; }
        protected string DateofBirthLabel { get; set; }
        protected string ProviderNameLabel { get; set; }
        protected string ProviderUkprnLabel { get; set; }
        protected string TlevelTitleLabel { get; set; }
        protected string CoreLabel { get; set; }
        protected string ExamPeriodLabel { get; set; }
        protected string GradeLabel { get; set; }

        public string LearnerName => $"{Firstname} {Lastname}";
        public string ProviderDisplayName => $"{ProviderName}<br/>({ProviderUkprn})";

        public SummaryItemModel SummaryUln => new SummaryItemModel
        {
            Id = "uln",
            Title = UlnLabel,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel
        {
            Id = "learnername",
            Title = LearnerNameLabel,
            Value = LearnerName
        };

        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel
        {
            Id = "dateofbirth",
            Title = DateofBirthLabel,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryProvider => new SummaryItemModel
        {
            Id = "providername",
            Title = ProviderNameLabel,
            Value = ProviderDisplayName,
            IsRawHtml = true
        };

        public SummaryItemModel SummaryProviderName => new SummaryItemModel
        {
            Id = "providername",
            Title = ProviderNameLabel,
            Value = ProviderName,
        };

        public SummaryItemModel SummaryProviderUkprn => new SummaryItemModel
        {
            Id = "providerukprn",
            Title = ProviderUkprnLabel,
            Value = ProviderUkprn.ToString(),
        };

        public SummaryItemModel SummaryTlevelTitle => new SummaryItemModel
        {
            Id = "tleveltitle",
            Title = TlevelTitleLabel,
            Value = TlevelTitle
        };

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "core",
            Title = CoreLabel,
            Value = CoreDisplayName
        };

        public SummaryItemModel SummaryExamPeriod => new SummaryItemModel
        {
            Id = "examperiod",
            Title = ExamPeriodLabel,
            Value = ExamPeriod
        };

        public SummaryItemModel SummaryGrade => new SummaryItemModel
        {
            Id = "grade",
            Title = GradeLabel,
            Value = Grade
        };

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsSearchLearner,
            RouteAttributes = new Dictionary<string, string> { { Constants.PopulateUln, true.ToString() } }
        };
    }
}
