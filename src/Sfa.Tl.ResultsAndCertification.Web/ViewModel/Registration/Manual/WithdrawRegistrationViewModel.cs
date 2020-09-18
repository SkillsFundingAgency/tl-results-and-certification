using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class WithdrawRegistrationViewModel
    {
        public int ProfileId { get; set; }

        public long Uln { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.WithdrawRegistration), ErrorMessageResourceName = "Select_Withdraw_Validation_Message")]
        public bool? CanWithdraw { get; set; }

        public int? WithdrawBackLinkOptionId { get; set; }
        public bool IsRequestFromProviderAndCorePage => WithdrawBackLinkOption != null && WithdrawBackLinkOption != WithdrawBackLinkOptions.AmendActiveRegistrationPage;
        public WithdrawBackLinkOptions BackLinkOptions { get; set; }

        public BackLinkModel BackLink => GetBackLinkModel;

        private bool IsValidWithdrawBackLinkOption => EnumExtensions.IsValidValue<WithdrawBackLinkOptions>(WithdrawBackLinkOptionId);
        private WithdrawBackLinkOptions? WithdrawBackLinkOption => IsValidWithdrawBackLinkOption ? (WithdrawBackLinkOptions?)WithdrawBackLinkOptionId : null;
        private BackLinkModel GetBackLinkModel
        {
            get
            {
                var backLink = new BackLinkModel();
                switch (WithdrawBackLinkOption)
                {
                    case WithdrawBackLinkOptions.ChangeCorePage:
                        backLink.RouteName = RouteConstants.ChangeRegistrationCore;
                        backLink.RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } };
                        break;
                    case WithdrawBackLinkOptions.CannotChangeProviderAndCorePage:
                        backLink.RouteName = RouteConstants.ChangeRegistrationProviderAndCoreNeedToWithdraw;
                        backLink.RouteAttributes = null;
                        break;
                    case WithdrawBackLinkOptions.AmendActiveRegistrationPage:
                    default:
                        backLink.RouteName = RouteConstants.AmendActiveRegistration;
                        backLink.RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.ChangeStatusId, ((int)RegistrationChangeStatus.Withdraw).ToString() } }; ;
                        break;                   
                }
                return backLink;
            }
        }
    }
}
