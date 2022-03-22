using MongoDB.Driver;
using Reports.Domain.Entities;
using Reports.Domain.Repositories.Abstract;
using Reports.Infrastructure.Constants;
using Reports.Infrastructure.Data.Abstract;
using Reports.Infrastructure.Repositories.Abstract;
using System;
using System.Threading.Tasks;

namespace Reports.Infrastructure.Repositories
{
    public class ReportRepository : MongoDbRepository<Report>, IReportRepository
    {
        private readonly IReportContext _context;

        public ReportRepository(IReportContext context) : base(context.Reports)
        {
            _context = context;
        }       

        public async Task<bool> CompleteRequest(string id)
        {
            var entity = await Get(x => x.Id.Equals(id));

            if (entity != null)
            {
                entity.CompleteDate = DateTime.UtcNow;
                entity.UpdatedTime = DateTime.UtcNow;
                entity.Status = (int)ReportStatusConstants.Status.Completed;
            }

            var result = await _context.Reports.ReplaceOneAsync(filter: q => q.Id.Equals(entity.Id), replacement: entity);
            return result.IsAcknowledged && result.MatchedCount > 0;
        }
    }
}
