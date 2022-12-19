namespace MultiPurposeProject.Helpers;

public interface IMongoDBSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}

public class MongoDBSettings: IMongoDBSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}