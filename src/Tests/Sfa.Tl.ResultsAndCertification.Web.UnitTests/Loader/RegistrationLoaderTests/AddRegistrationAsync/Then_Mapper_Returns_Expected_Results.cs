using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.AddRegistrationAsync
{
    public class Then_Mapper_Returns_Expected_Results : When_AddRegistrationAsync_Is_Called
    {
        [Fact]
        public void Then_Mapper_Has_Expected_Results()
        {
            var result = Mapper.Map<RegistrationRequest>(RegistrationViewModel, opt => opt.Items["aoUkprn"] = AoUkprn);

            result.Should().NotBeNull();

            result.AoUkprn.Should().Be(AoUkprn);
            result.Uln.Should().Be(RegistrationViewModel.Uln.Uln.ToLong());
            result.FirstName.Should().Be(RegistrationViewModel.LearnersName.Firstname);
            result.LastName.Should().Be(RegistrationViewModel.LearnersName.Lastname);
            result.DateOfBirth.Should().Be($"{RegistrationViewModel.DateofBirth.Day}/{RegistrationViewModel.DateofBirth.Month}/{RegistrationViewModel.DateofBirth.Year}".ToDateTime());
            result.ProviderUkprn.Should().Be(RegistrationViewModel.SelectProvider.SelectedProviderUkprn.ToLong());
            result.CoreCode.Should().Be(RegistrationViewModel.SelectCore.SelectedCoreCode);
            result.SpecialismCodes.Should().BeEquivalentTo(RegistrationViewModel.SelectSpecialism.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).Select(s => s.Code));
            result.CreatedBy.Should().Be($"{Givenname} {Surname}");
        }
    }
}
