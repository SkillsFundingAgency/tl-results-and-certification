namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddProviderAddressViewModel
    {
        public bool IsManual { get; set; }
        public AddAddressPostcodeViewModel AddAddressPostcode { get; set; }
        public AddAddressSelectViewModel AddAddressSelect { get; set; }
        public AddPostalAddressManualViewModel Manual { get; set; }
    }
}