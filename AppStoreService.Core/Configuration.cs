namespace AppStoreService.Core
{
    public class Configuration
    {
        public string Project { get; set; }
        public string MongoConnectionString { get; set; }
        public string MongoConnectionStringWithProject => MongoConnectionString + Project.Replace(".", "_");
    }
}