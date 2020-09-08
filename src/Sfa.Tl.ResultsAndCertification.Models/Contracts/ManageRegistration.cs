namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ManageRegistration : RegistrationRequest
    {
        public int ProfileId { get; set; }

        public bool HasProfileChanged { get; set; }

        public bool HasProviderChanged { get; set; }

        public bool HasSpecialismsChanged { get; set; }

        public string ModifiedBy { get; set; }
    }
}
