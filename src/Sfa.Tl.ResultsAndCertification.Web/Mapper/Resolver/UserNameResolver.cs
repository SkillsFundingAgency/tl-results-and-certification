using AutoMapper;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver
{
    public class UserNameResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserNameResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
        {
            return _httpContextAccessor.HttpContext.User.GetUserName();
        }
    }
}
