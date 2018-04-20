using System.Data;
using System.Data.SqlClient;

namespace HSG.Exception.Web
{
    public static class ConnectionFactory
    {
        public static IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}