using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories
{
    public interface IBaseTest<T> : IDisposable
    {
        void Setup();
        void Given();
        Task When();
    }
}
