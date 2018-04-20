using System;
using System.Data;
using Dapper;

namespace HSG.Exception.DAL.Repositories
{
    public class UserRepository
    {
        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        private readonly IDbConnection _connection;
        public User GetByUserName(string userName)
        {
            try
            {
                return _connection.QuerySingleOrDefault<User>("SELECT * FROM Users WHERE UserName = @UserName;", new {UserName = userName});
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
