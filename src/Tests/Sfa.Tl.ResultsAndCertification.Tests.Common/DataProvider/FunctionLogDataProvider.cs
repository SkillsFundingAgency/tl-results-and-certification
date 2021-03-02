using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class FunctionLogDataProvider
    {
        public static FunctionLog CreateFunctionLog(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var functionLog = new FunctionLogBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(functionLog);
            }
            return functionLog;
        }

        public static FunctionLog CreateFunctionLog(ResultsAndCertificationDbContext _dbContext, FunctionLog functionLog, bool addToDbContext = true)
        {
            if (functionLog == null)
            {
                functionLog = new FunctionLogBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(functionLog);
            }
            return functionLog;
        }

        public static FunctionLog CreateFunctionLog(ResultsAndCertificationDbContext _dbContext, string name, DateTime startDate, DateTime? endDate, FunctionStatus status, string message, bool addToDbContext = true)
        {
            var functionLog = new FunctionLog
            {
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                Status = status,
                Message = message,
                CreatedBy = "Test User"
            };

            if (addToDbContext)
            {
                _dbContext.Add(functionLog);
            }
            return functionLog;
        }
    }
}
