using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Interfaces
{
    public interface IPersonalLearningRecordService
    {
        Task ProcessLearnerVerificationAndLearningEvents();
    }
}
