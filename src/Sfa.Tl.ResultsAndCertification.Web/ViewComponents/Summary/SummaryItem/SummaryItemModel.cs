﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem
{
    public class SummaryItemModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string TitleCss { get; set; }
        public string Value { get; set; }
        public string Value2 { get; set; }

        public string ActionText { get; set; }
        public string RouteName { get; set; }
        public Dictionary<string, string> RouteAttributes { get; set; }

        public bool HasTag { get; set; }
        public string TagCssClass { get; set; }
        public bool RenderHiddenActionText { get; set; } = true;
        public string HiddenActionText { get; set; }
        public string HiddenValueText { get; set; }

        public bool RenderEmptyRowForValue2 { get; set; }
        public bool RenderActionColumn { get; set; } = true;
        public bool IsRawHtml { get; set; } = false;
        public bool NeedBorderBottomLine { get; set; } = true;
        public string Value2CustomCssClass { get; set; }
        public string BorderBottomCssClass => NeedBorderBottomLine ? string.Empty : Constants.NoBorderBottomCssClassName;
    }
}
