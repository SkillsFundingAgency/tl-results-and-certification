using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest
{
    public interface IBaseTest<T> : IDisposable
    {
        void Setup();
        void Given();
        Task When();
    }
}
