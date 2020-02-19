using System;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest
{
    public interface IBaseTest<T>
    {
        public void Setup();
        public void Given();
        public void When();

    }
}
