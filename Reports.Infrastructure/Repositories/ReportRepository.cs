using MongoDB.Driver;
using Reports.Domain.Entities;
using Reports.Domain.Repositories.Abstract;
using Reports.Infrastructure.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IReportContext _context;

        public ReportRepository(IReportContext context)
        {
            _context = context;
        }

        public async Task<Report> Add(Report entity)
        {
            await _context.Reports.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> Delete(Report entity)
        {
            var filter = Builders<Report>.Filter.Eq(q => q.Id, entity.Id);
            var result = await _context.Reports.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Report> Get(Expression<Func<Report, bool>> filter)
        {
            return await _context.Reports.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Report>> GetAll(Expression<Func<Report, bool>> filter = null)
        {
            return filter == null ?
                await _context.Reports.Find(m => true).ToListAsync() :
                await _context.Reports.Find(filter).ToListAsync();
        }

        public async Task<bool> Update(Report entity)
        {
            var oldEntity = await Get(x => x.Id.Equals(entity.Id));
            if (oldEntity != null)
            {
                entity.CreatedTime = oldEntity.CreatedTime;
                entity.UpdatedTime = DateTime.Now;
            }
            var result = await _context.Reports.ReplaceOneAsync(filter: q => q.Id.Equals(entity.Id), replacement: entity);
            return result.IsAcknowledged && result.MatchedCount > 0;
        }
    }
}
