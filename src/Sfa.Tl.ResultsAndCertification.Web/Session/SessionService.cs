using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Session
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _keyPrefix;

        public SessionService(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _httpContextAccessor = httpContextAccessor;
            _keyPrefix = env.EnvironmentName;
        }

        public string Get(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(GetFormatedKey(key));
        }

        public T Get<T>(string key)
        {
            var contextSession = _httpContextAccessor.HttpContext.Session;
            key = GetFormatedKey(key);

            if (!Exists(key))
            {
                return default;
            }

            var value = contextSession.GetString(key);
            return string.IsNullOrWhiteSpace(value) ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public void Set(string key, object value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(GetFormatedKey(key), JsonConvert.SerializeObject(value));
        }

        public void Set(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(GetFormatedKey(key), value);
        }

        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(GetFormatedKey(key));
        }        

        public bool Exists(string key)
        {
            return _httpContextAccessor.HttpContext.Session.Keys.Any(k => k == GetFormatedKey(key));
        }

        private string GetFormatedKey(string key)
        {
            return $"{_keyPrefix}_{key}";
        }
    }
}
