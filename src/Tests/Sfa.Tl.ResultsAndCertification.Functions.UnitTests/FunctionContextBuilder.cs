using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests
{
    public static class FunctionContextBuilder
    {
        public static FunctionContext Create(string functionName = null)
        {
            var functionDefinition = Substitute.For<FunctionDefinition>();
            functionDefinition.Name.Returns(functionName);

            var context = Substitute.For<FunctionContext>();
            context.FunctionDefinition.Returns(functionDefinition);
            context.InstanceServices.Returns(new ServiceCollection().AddLogging().BuildServiceProvider());

            return context;
        }
    }
}
