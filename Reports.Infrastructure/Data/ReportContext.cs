using MongoDB.Driver;
using Reports.Domain.Entities;
using Reports.Infrastructure.Data.Abstract;
using Reports.Infrastructure.Settings.Abstract;

namespace Reports.Infrastructure.Data
{
    public class ReportContext : IReportContext
    {
        public ReportContext(IReportDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Reports = database.GetCollection<Report>(settings.CollectionName);
            ReportContextSeed.SeedData(Reports);
        }

        public IMongoCollection<Report> Reports { get; }
    }
}
