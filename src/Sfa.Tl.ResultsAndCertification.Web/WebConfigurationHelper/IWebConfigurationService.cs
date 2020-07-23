namespace Sfa.Tl.ResultsAndCertification.Web.WebConfigurationHelper
{
    public interface IWebConfigurationService
    {
        string GetTechnicalSupportEmailAddress();
        string GetFeedbackEmailAddress();
        string GetSignOutPath();
        string GetFormattedTitle(string pageTitle, bool isModelValid);
    }
}
