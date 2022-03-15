using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.PrsRommActivity
{
    public class When_RommOutcomeKnown_CheckSubmit_IsSuccess : TestSetup
    {
        private int _newGradeLookupId;
        private PrsRommCheckAndSubmitViewModel _model;

        public override void Given()
        {
            _model = new PrsRommCheckAndSubmitViewModel
            {
                ProfileId = 1,
                AssessmentId = 2,
                ResultId = 3,
                NewGrade = "A",
                ComponentType = ComponentType.Core
            };

            _newGradeLookupId = 11;
            var lookupGrades = new List<LookupData> { new LookupData { Id = _newGradeLookupId, Code = "A", Value = "A" } };
            InternalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).
                Returns(lookupGrades);

            InternalApiClient.PrsActivityAsync(Arg.Is<PrsActivityRequest>(x =>
                                x.ProfileId == _model.ProfileId &&
                                x.AssessentId == _model.AssessmentId &&
                                x.ResultId == _model.ResultId &&
                                x.ComponentType == ComponentType.Core &&
                                x.PrsStatus == PrsStatus.Reviewed &&
                                x.ResultLookupId == _newGradeLookupId &&
                                x.AoUkprn == AoUkprn &&
                                x.PerformedBy == $"{Givenname} {Surname}"
                                ))
                .Returns(true);
        }

        public async override Task When()
        {
            ActualResult = await Loader.PrsRommActivityAsync(AoUkprn, _model);
        }

        [Fact]
        public void Then_True_Returned()
        {
            ActualResult.Should().BeTrue();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
            InternalApiClient.Received(1).PrsActivityAsync(Arg.Is<PrsActivityRequest>(x =>
                                x.ProfileId == _model.ProfileId &&
                                x.AssessentId == _model.AssessmentId &&
                                x.ResultId == _model.ResultId &&
                                x.ComponentType == ComponentType.Core &&
                                x.PrsStatus == PrsStatus.Reviewed &&
                                x.ResultLookupId == _newGradeLookupId &&
                                x.AoUkprn == AoUkprn &&
                                x.PerformedBy == $"{Givenname} {Surname}"
                                ));
        }

        public override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(PostResultsServiceMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<PrsRommCheckAndSubmitViewModel, PrsActivityRequest>(HttpContextAccessor) : null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
