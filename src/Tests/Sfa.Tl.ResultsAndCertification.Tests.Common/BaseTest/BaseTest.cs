using System.Net.Http;

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
        public abstract void When();

        public void Dispose()
        {
            HttpClient?.Dispose();
        }
    }
}
