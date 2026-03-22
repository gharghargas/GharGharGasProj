using Dapper;
using Npgsql;

namespace Vore.Data
{
    public class UserRepository
    {
        private readonly PostgresOptions _opts;

        public UserRepository(PostgresOptions opts)
        {
            _opts = opts;
        }

        private NpgsqlConnection GetConnection() => new NpgsqlConnection(_opts.GetConnectionString());

        public async Task<int> CreateUser(string username, string email)
        {
            using var conn = GetConnection();
            var sql = "INSERT INTO public.users (username, email) VALUES (@Username, @Email)";
            return await conn.ExecuteAsync(sql, new { Username = username, Email = email });
        }

        public async Task<IEnumerable<dynamic>> GetUsers()
        {
            using var conn = GetConnection();
            return await conn.QueryAsync("SELECT id, username, email, created_at FROM public.users ORDER BY created_at DESC");
        }
    }
}
