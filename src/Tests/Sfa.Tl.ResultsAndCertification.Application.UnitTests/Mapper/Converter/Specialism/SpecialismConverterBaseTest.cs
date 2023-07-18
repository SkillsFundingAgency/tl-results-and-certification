using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.Specialism
{
    public abstract class SpecialismConverterBaseTest<TConverter> : ConverterBaseTest<TConverter, TqRegistrationPathway, string>
        where TConverter : IValueConverter<TqRegistrationPathway, string>, new()
    {
        protected TqRegistrationPathway CivilEngineeringRegistration = new()
        {
            TqRegistrationSpecialisms = new[]
            {
                new TqRegistrationSpecialism
                {
                    TlSpecialism = CivilEngineering
                }
            }
        };

        protected static TlSpecialism CivilEngineering = new()
        {
            Id = 1,
            LarId = "ZTLOS002",
            Name = "Civil Engineering"
        };

        protected TqRegistrationPathway PlumbingAndHeatingEngineeringRegistration = new()
        {
            TqRegistrationSpecialisms = new[]
            {
                new TqRegistrationSpecialism
                {
                    TlSpecialism = HeatingEngineering
                },
                new TqRegistrationSpecialism
                {
                    TlSpecialism = PlumbingEngineering
                }
            }
        };

        protected static TlDualSpecialism PlumbingAndHeatingEngineering = new()
        {
            Id = 1,
            LarId = "ZTLOS030",
            Name = "Plumbing and Heating Engineering"
        };

        private static TlSpecialism HeatingEngineering = new()
        {
            Id = 11,
            Name = "Heating Engineering",
            TlDualSpecialismToSpecialisms = new[]
            {
                new TlDualSpecialismToSpecialism
                {
                    Id = 1,
                    TlDualSpecialismId = 1,
                    TlSpecialismId = 11,
                    DualSpecialism = PlumbingAndHeatingEngineering
                },
                new TlDualSpecialismToSpecialism
                {
                    Id = 2,
                    TlDualSpecialismId = 2,
                    TlSpecialismId = 11,
                    DualSpecialism = new TlDualSpecialism
                    {
                        Id = 2,
                        LarId = "ZTLOS031",
                        Name = "Heating Engineering and Ventilation"
                    }
                }
            }
        };

        private static TlSpecialism PlumbingEngineering = new()
        {
            Id = 13,
            Name = "Plumbing Engineering",
            TlDualSpecialismToSpecialisms = new[]
            {
                new TlDualSpecialismToSpecialism
                {
                    Id = 2,
                    TlDualSpecialismId = 1,
                    TlSpecialismId = 13,
                    DualSpecialism = PlumbingAndHeatingEngineering
                }
            }
        };
    }
}