using MongoDB.Driver;
using Reports.Domain.Entities;
using Reports.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reports.Infrastructure.Data
{
    public class ReportContextSeed
    {
        public static void SeedData(IMongoCollection<Report> reports)
        {
            bool isExistReport = reports.Find(p => true).Any();
            if (!isExistReport)
                reports.InsertManyAsync(GetConfigureReports());
        }

        private static IEnumerable<Report> GetConfigureReports()
        {
            return new List<Report>
            {
                new Report()
                {
                    CreateTime = DateTime.UtcNow.AddDays(-10),
                    CompleteDate = DateTime.UtcNow,
                    Status = (int)ReportStatusConstants.Status.Completed,
                    ReportDatas = new List<ReportData>()
                    {
                        new ReportData()
                        {
                            Location = "Kastamonu",
                            PersonCount = 15,
                            PhoneNumberCount = 11
                        },
                        new ReportData()
                        {
                            Location = "İstanbul",
                            PersonCount = 150,
                            PhoneNumberCount = 125
                        },
                    }
                },
                new Report()
                {
                    CreateTime = DateTime.UtcNow.AddDays(-10),
                    Status = (int)ReportStatusConstants.Status.Preparing
                },
            };
        }
    }
}
