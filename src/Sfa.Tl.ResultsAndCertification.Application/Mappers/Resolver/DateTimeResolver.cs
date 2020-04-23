using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver
{
    public class DateTimeResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, DateTime?>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DateTimeResolver(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public DateTime? Resolve(TSource source, TDestination dest, DateTime? destMember, ResolutionContext context)
        {
            return _dateTimeProvider.GetUtcNow();
        }
    }
}
