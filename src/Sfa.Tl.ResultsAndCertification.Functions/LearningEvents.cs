using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class LearningEvents
    {       
        [FunctionName("GetLearningEvents")]
        public void GetLearningEventsAsync([TimerTrigger("%LearningEventsTrigger%")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"GetLearningEvents Timer trigger function executed at: {DateTime.Now}");

            log.LogError("Error loading GetLearningEvents Data");
        }
    }
}
