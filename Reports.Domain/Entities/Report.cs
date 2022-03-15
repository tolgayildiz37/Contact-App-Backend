using Reports.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace Reports.Domain.Entities
{
    public class Report : Entity
    {
        public DateTime CreateTime { get; set; }
        public DateTime CompleteDate { get; set; }
        public int Status { get; set; }
        public List<ReportData> ReportDatas { get; set; }
    }
}
