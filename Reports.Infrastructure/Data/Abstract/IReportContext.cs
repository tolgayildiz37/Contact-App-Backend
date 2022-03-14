using MongoDB.Driver;
using Reports.Domain.Entities;

namespace Reports.Infrastructure.Data.Abstract
{
    public interface IReportContext : IContext
    {
        IMongoCollection<Report> Reports { get; }
    }
}
