using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordViewModel
    {
        public int ChangeLogId { get; set; }

        #region Learner

        public int RegistrationPathwayId { get; set; }

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string PathwayName { get; set; }

        public string CoreCode { get; set; }

        public string SpecialismName { get; set; }

        public string SpecialismCode { get; set; }

        #endregion Learner

        #region Change log

        public string CreatedBy { get; set; }

        public ChangeType ChangeType { get; set; }

        public string ChangeDetails { get; set; }

        public string ChangeRequestedBy { get; set; }

        public string ChangeDateOfRequest { get; set; }

        public string ReasonForChange { get; set; }

        public string ZendeskTicketID { get; set; }

        public string DateAndTimeOfChange { get; set; }

        #endregion Change log

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminSearchChangeLogClear
        };

        public SummaryItemModel SummaryLearner =>
            new()
            {
                Id = AdminViewChangeRecord.Summary_Learner_Id,
                Title = AdminViewChangeRecord.Summary_Learner_Text,
                Value = Learner,
                ActionText = AdminViewChangeRecord.Action_Text_Link_Change,
                RouteName = RouteConstants.AdminLearnerRecord,
                RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } },
                HiddenActionText = AdminViewChangeRecord.Hidden_Action_Text_Learner_Record
            };

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminViewChangeRecord.Summary_ULN_Id, AdminViewChangeRecord.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryCreatedBy
            => CreateSummaryItemModel(AdminViewChangeRecord.Summary_CreatedBy_Id, AdminViewChangeRecord.Summary_CreatedBy_Text, CreatedBy);

        private static SummaryItemModel CreateSummaryItemModel(string id, string title, string value)
         => new()
         {
             Id = id,
             Title = title,
             Value = value
         };
    };
}