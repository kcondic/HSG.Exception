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
            return _connection.QuerySingleOrDefault<User>("SELECT * FROM Users WHERE UserName = @UserName;", new {UserName = userName});
        }

        public bool AddUser(User userToAdd)
        {
            if (GetByUserName(userToAdd.UserName) != null)
                return false;
            _connection.Execute("INSERT INTO Users VALUES (UserName = @user.UserName, FirstName = @user.FirstName, " +
                                "LastName = @user.LastName, Password = @user.Password, Role = @user.Role, Email = @user.Email)",
                                new {user = userToAdd});
            return true;
        }
    }
}
