﻿namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddressViewModel
    {
        public int AddressId {get;set;}
        public string Udprn { get; set; }
        public string FormattedAddress { get; set; }
        public string DepartmentName { get; set; }
        public string OrganisationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
    }
}