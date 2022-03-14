namespace Reports.Infrastructure.Settings.Abstract
{
    public interface IReportDbSettings : IDatabaseSettings
    {
        public string CollectionName { get; }
    }
}
