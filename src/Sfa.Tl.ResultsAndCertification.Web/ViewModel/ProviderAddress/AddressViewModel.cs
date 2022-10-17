using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddressViewModel
    {
        public int AddressId { get; set; }
        public string Udprn { get; set; }
        public string FormattedAddress { get; set; }
        public string DepartmentName { get; set; }
        public string OrganisationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }

        public string ToDisplayValue
        {
            get
            {
                var result = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(OrganisationName))
                    result.Append($"{OrganisationName}<br/>");

                if (!string.IsNullOrWhiteSpace(AddressLine1))
                    result.Append($"{AddressLine1}<br/>");

                if (!string.IsNullOrWhiteSpace(AddressLine2))
                    result.Append($"{AddressLine2}<br/>");

                if (!string.IsNullOrWhiteSpace(Town))
                    result.Append($"{Town}<br/>");

                if (!string.IsNullOrWhiteSpace(Postcode))
                    result.Append($"{Postcode}<br/>");

                return result.ToString();
            }
        }
    }
}