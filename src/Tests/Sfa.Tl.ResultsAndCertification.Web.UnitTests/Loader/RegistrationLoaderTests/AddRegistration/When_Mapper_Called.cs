using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.AddRegistration
{
    public class When_Mapper_Called : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
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
            result.SpecialismCodes.Should().BeEquivalentTo(RegistrationViewModel.SelectSpecialisms.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).Select(s => s.Code));
            result.PerformedBy.Should().Be($"{Givenname} {Surname}");
        }
    }
}
