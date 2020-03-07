using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers
{
    public class ClaimsIdentityBuilder<T> where T : ControllerBase
    {
        private readonly T _controller;
        private readonly List<Claim> _claims = new List<Claim>();

        public ClaimsIdentityBuilder(T controller)
        {
            _controller = controller;
        }

        public ClaimsIdentityBuilder<T> Add(string type, string value)
        {
            _claims.Add(new Claim(type, value));
            return this;
        }

        public T Build()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(_claims)) }
            };

            return _controller;
        }
    }
}
