using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter
{
    public abstract class ConverterBaseTest<TConverter, TSource, TResult> : BaseTest<TConverter>
        where TConverter : IValueConverter<TSource, TResult>, new()
    {
        protected TConverter Converter;

        protected TSource Source;
        protected TResult Result;

        public override void Setup()
        {
            Converter = new TConverter();
        }

        public override Task When()
        {
            Result = Converter.Convert(Source, null);
            return Task.CompletedTask;
        }
    }
}