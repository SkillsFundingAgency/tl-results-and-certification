using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Data.Factory
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity, new();
    }
}