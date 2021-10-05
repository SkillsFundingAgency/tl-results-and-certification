using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.AddRegistration
{
    public class When_Mapper_Called : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var expectedSpecialismCodes = new List<string>();
            RegistrationViewModel.SelectSpecialisms.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).Select(s => s.Code).ToList().ForEach(c => { expectedSpecialismCodes.AddRange(c.Split(Constants.PipeSeperator)); }); ;
            
            var result = Mapper.Map<RegistrationRequest>(RegistrationViewModel, opt => opt.Items["aoUkprn"] = AoUkprn);
            
            result.Should().NotBeNull();
            result.AoUkprn.Should().Be(AoUkprn);
            result.Uln.Should().Be(RegistrationViewModel.Uln.Uln.ToLong());
            result.FirstName.Should().Be(RegistrationViewModel.LearnersName.Firstname);
            result.LastName.Should().Be(RegistrationViewModel.LearnersName.Lastname);
            result.DateOfBirth.Should().Be($"{RegistrationViewModel.DateofBirth.Day}/{RegistrationViewModel.DateofBirth.Month}/{RegistrationViewModel.DateofBirth.Year}".ToDateTime());
            result.ProviderUkprn.Should().Be(RegistrationViewModel.SelectProvider.SelectedProviderUkprn.ToLong());
            result.CoreCode.Should().Be(RegistrationViewModel.SelectCore.SelectedCoreCode);
            result.SpecialismCodes.Should().BeEquivalentTo(expectedSpecialismCodes);
            result.PerformedBy.Should().Be($"{Givenname} {Surname}");
        }
    }
}
