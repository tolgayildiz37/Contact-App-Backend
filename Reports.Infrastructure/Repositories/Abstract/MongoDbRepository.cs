using MongoDB.Driver;
using Reports.Domain.Entities.Abstract;
using Reports.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Reports.Infrastructure.Repositories.Abstract
{
    public class MongoDbRepository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        private readonly IMongoCollection<TEntity> _collection;
        public MongoDbRepository(IMongoCollection<TEntity> collection)
        {
            _collection = collection;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> Delete(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(q => q.Id, entity.Id);
            var result = await _collection.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ?
                await _collection.Find(m => true).ToListAsync() :
                await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> Update(TEntity entity)
        {
            var oldEntity = await Get(x => x.Id.Equals(entity.Id));
            if (oldEntity != null)
            {
                entity.CreatedTime = oldEntity.CreatedTime;
                entity.UpdatedTime = DateTime.Now;
            }
            var result = await _collection.ReplaceOneAsync(filter: q => q.Id.Equals(entity.Id), replacement: entity);
            return result.IsAcknowledged && result.MatchedCount > 0;
        }
    }
}
