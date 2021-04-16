namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ITrainingProviderRepository
    {
        public bool IsSendConfirmationRequiredAsync(int profileId);
    }
}
