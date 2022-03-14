using Reports.Infrastructure.Constants;
using Reports.Infrastructure.Settings.Abstract;

namespace Reports.Infrastructure.Settings
{
    public class ReportDbSettings : IReportDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; }

        public ReportDbSettings()
        {
            CollectionName = DbSettingsConstants.COLLECTION_REPORTS;
        }
    }
}
