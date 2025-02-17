using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using notebodia_api.Dto;
using notebodia_api.Response;
using notebodia_api.Models;
using notebodia_api.Types;
using notebodia_api.Util;
using notebodia_api.Db;

namespace notebodia_api.Repositories
{
    public class UserRepository
    {
        // private readonly IConfiguration _configuration;
        private readonly DapperDbContext _dbContext;
        public UserRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllUsers()
        {
            using var connection = _dbContext.GetConnection();
            var users = await connection.QueryAsync<User>(
                """
                SELECT * FROM Users
                """
                );
            return users.ToList();

        }

        public async Task<User> GetUserById(Guid id)
        {
            using var connection = _dbContext.GetConnection();
            var sql = """
                SELECT u.* FROM Users u  WHERE id = @id
                """;

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                sql, new { id }
                );

            return user;
        }


        public async Task<UserDto> UserSignup(CreateUserPayload payload)
        {
            using var connection = _dbContext.GetConnection();
            var sql = """
        INSERT INTO Users (username, password, email)
        VALUES (@Username, @Password, @Email)
        RETURNING id, username, email, created_at, updated_at
        """;

            try
            {
                string hashedPassword = PasswordHasher.HashPassword(payload.Password);

                var parameters = new
                {
                    payload.Username,
                    Password = hashedPassword,
                    payload.Email
                };

                var user = await connection.QuerySingleAsync<UserDto>(sql, parameters);
                PrintUtils.PrettyPrint(user);
                return user;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    throw new DuplicateResourceException();
                }
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create user", ex);
            }

        }
        public async Task<User> CheckEmailAndUsername(string Username, string Email)
        {
            using var connection = _dbContext.GetConnection();
            var sql =
            """
            SELECT * FROM Users WHERE username = @Username AND email = @Email
            """;
            try
            {

                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new
                {
                    Email,
                    Username,
                });
                return user;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to fetch user", ex);
            }
        }
        // private SqlConnection GetConnection()
        // {
        //     return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        // }


    }
}
