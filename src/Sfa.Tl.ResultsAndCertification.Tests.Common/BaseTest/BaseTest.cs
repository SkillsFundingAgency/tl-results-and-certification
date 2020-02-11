namespace Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest
{
    public abstract class BaseTest<T> : IBaseTest<T> 
    {
        public BaseTest()
        {
            Setup();
            Given();
            When();
        }

        public abstract void Setup();
        public abstract void Given();
        public abstract void When();
    }
}
