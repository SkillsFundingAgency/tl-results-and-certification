namespace Sfa.Tl.ResultsAndCertification.Web.WebConfigurationHelper
{
    public interface IWebConfigurationService
    {
        string GetFeedbackEmailAddress();
        string GetSignOutPath();
        string GetFormattedTitle(string pageTitle, bool isModelValid);
    }
}
