﻿using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public abstract class ResultsBaseViewModel
    {
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string TlevelTitle { get; set; }

        protected string UlnLabel { get; set; }
        protected string LearnerNameLabel { get; set; }
        protected string DateofBirthLabel { get; set; }
        protected string ProviderUkprnLabel { get; set; }
        protected string ProviderNameLabel { get; set; }
        protected string TlevelTitleLabel { get; set; }

        public string LearnerName => $"{Firstname} {Lastname}";

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

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = SearchRegistrationRouteName,
            RouteAttributes = SearchRegistrationRouteAttributes
        };

        public string SearchRegistrationRouteName => RouteConstants.SearchRegistration;

        public Dictionary<string, string> SearchRegistrationRouteAttributes => new()
        {
            [Constants.Type] = SearchRegistrationType.Result.ToString()
        };
    }
}
