using System.Net.Http;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest
{
    public abstract class BaseTest<T> : IBaseTest<T> 
    {
        protected HttpClient HttpClient { get; set; }
        public BaseTest()
        {
            Setup();
            Given();
            When();
        }

        public abstract void Setup();
        public abstract void Given();
        public abstract Task When();

        public void Dispose()
        {
            HttpClient?.Dispose();
        }
    }
}
