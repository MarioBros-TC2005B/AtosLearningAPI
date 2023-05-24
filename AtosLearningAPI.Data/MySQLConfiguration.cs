namespace AtosLearningAPI.Data;

public class MySQLConfiguration
{
    public string ConnectionString { get; }
    
    public MySQLConfiguration(string connectionString)
    {
        ConnectionString = connectionString;
    }
}