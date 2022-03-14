using Reports.Domain.Entities;
using System.Threading.Tasks;

namespace Reports.Domain.Repositories.Abstract
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<bool> CompleteRequest(string id);
    }
}
