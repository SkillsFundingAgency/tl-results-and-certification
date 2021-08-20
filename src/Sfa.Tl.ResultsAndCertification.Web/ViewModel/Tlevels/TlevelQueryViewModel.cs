using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels
{
    public class TlevelQueryViewModel : TlevelSummary
    {
        public TlevelQueryViewModel()
        {
            Specialisms = new List<string>();
        }

        public int TqAwardingOrganisationId { get; set; }
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public int PathwayStatusId { get; set; }

        public bool IsBackToVerifyPage { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.Query), ErrorMessageResourceName = "Query_Required_Validation_Message")]
        [StringLength(10000, ErrorMessageResourceType = typeof(ErrorResource.Query), ErrorMessageResourceName = "Query_CharLimitExceeded_Validation_Message")]
        public string Query { get; set; }
        
        public BackLinkModel BackLink
        {
            get
            {
                if (IsBackToVerifyPage)
                    return new BackLinkModel
                    {
                        RouteName = RouteConstants.ReviewTlevelDetails, // TODO: in next story. 
                        RouteAttributes = new Dictionary<string, string> { { "id", PathwayId.ToString() }, { "isback", "true" } }
                    };

                else
                    return new BackLinkModel
                    {
                        RouteName = RouteConstants.TlevelDetails,
                        RouteAttributes = new Dictionary<string, string> { { "id", PathwayId.ToString() } }
                    };
            }
        }
    }
}