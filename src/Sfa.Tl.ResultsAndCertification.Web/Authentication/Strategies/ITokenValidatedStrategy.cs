using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Authentication.Strategies
{
    public interface ITokenValidatedStrategy
    {
        Task GetOnTokenValidatedTask(TokenValidatedContext context);
    }
}