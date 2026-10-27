using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Helpers
{
    internal static class FunctionContextFactory
    {
        private static readonly IServiceProvider ServiceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        public static FunctionContext Create()
        {
            var context = Substitute.For<FunctionContext>();
            var definition = Substitute.For<FunctionDefinition>();

            definition.Name.Returns(string.Empty);
            context.FunctionDefinition.Returns(definition);
            context.InstanceServices.Returns(ServiceProvider);

            return context;
        }
    }
}
