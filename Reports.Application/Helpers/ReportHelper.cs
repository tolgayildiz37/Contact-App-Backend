using Reports.Application.Constants;
using Reports.Domain.Entities;
using System;

namespace Reports.Application.Helpers
{
    public class ReportHelper
    {
        public static Report CreateNewReport()
        {
            return new Report()
            {
                CreateTime = DateTime.UtcNow,
                Status = (int)ReportStatusConstants.Status.Preparing,
            };
        }
    }
}
