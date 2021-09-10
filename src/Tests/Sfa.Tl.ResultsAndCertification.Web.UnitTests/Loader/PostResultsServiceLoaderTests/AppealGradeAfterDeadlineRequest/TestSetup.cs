using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.AppealGradeAfterDeadlineRequest
{
    public abstract class TestSetup : PostResultsServiceLoaderTestBase
    {
        protected AppealGradeAfterDeadlineConfirmViewModel ViewModel;
        protected bool ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.AppealGradeAfterDeadlineRequestAsync(ViewModel);
        }

        public override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(PostResultsServiceMapper).Assembly);
                c.ConstructServicesUsing(type =>
                                 type.Name.Contains("UserEmailResolver") ? (object)new UserEmailResolver<AppealGradeAfterDeadlineConfirmViewModel, Models.Contracts.PostResultsService.AppealGradeAfterDeadlineRequest>(HttpContextAccessor) :
                                null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
