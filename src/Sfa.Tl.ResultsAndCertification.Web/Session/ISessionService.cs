namespace Sfa.Tl.ResultsAndCertification.Web.Session
{
    public interface ISessionService
    {
        string Get(string key);
        T Get<T>(string key);
        void Set(string key, object value);
        void Set(string key, string value);
        void Remove(string key);        
        bool Exists(string key);
    }
}
