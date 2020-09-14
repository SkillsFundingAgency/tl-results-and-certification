using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services
{
    public interface IBaseTest<T> : IDisposable
    {
        void Setup();
        void Given();
        Task When();
    }
}
