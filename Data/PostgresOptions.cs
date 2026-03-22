namespace Vore.Data
{
    public class PostgresOptions
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = "vore_db";
        public string Username { get; set; } = "vore_user";
        public string Password { get; set; } = "VorePass123!";

        public string GetConnectionString() =>
            $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
    }
}
