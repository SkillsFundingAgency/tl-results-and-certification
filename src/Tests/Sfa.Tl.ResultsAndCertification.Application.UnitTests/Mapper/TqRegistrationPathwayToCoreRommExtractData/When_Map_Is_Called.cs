using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationPathwayToCoreRommExtractData
{
    public class When_Map_Is_Called
    {
        protected IMapper _mapper;

        public When_Map_Is_Called()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(CoreRommExtractMapper).Assembly));
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Then_Map_Without_Throwing()
        {
            _mapper.Map<CoreRommExtractData>(new TqRegistrationPathway());
        }
    }
}