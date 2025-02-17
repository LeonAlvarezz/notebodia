using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using notebodia_api.Dto;
using notebodia_api.Response;
using notebodia_api.Models;
using notebodia_api.Types;
using notebodia_api.Util;

namespace notebodia_api.Repositories
{
    public class SessionRepository
    {
        private readonly IConfiguration _configuration;
        public SessionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Session> GetSessionByIdAsync(Guid id)
        {
            var connection = GetConnection();
            var sql = @"
            SELECT s.*, u.*
            FROM Sessions s
            JOIN Users u
            ON s.user_id = u.id
            ";
            sql += " WHERE s.id = @id";

            var session = await connection.QueryAsync<Session, UserDto, Session>(sql,
            (session, user) =>
            {
                session.User = user;
                return session;
            },
            new { id }
            );
            return session.FirstOrDefault();
        }

        public async Task<Session> CreateSessionAsync(Session payload)
        {
            var connection = GetConnection();
            var sql = """
            INSERT INTO Sessions (id, user_id, expires_at) 
            OUTPUT INSERTED.id, INSERTED.user_id, INSERTED.expires_at, INSERTED.created_at
            VALUES (@Id, @UserId, @ExpiresAt)
            """;
            try
            {
                var parameter = new
                {
                    Id = payload.Id,
                    UserId = payload.UserId,
                    ExpiresAt = payload.ExpiresAt

                };
                var session = await connection.QuerySingleAsync<Session>(sql, parameter);
                return session;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create session", ex);
            }
        }

        public async Task<bool> DeleteSessionAsync(Guid Id)
        {
            var connection = GetConnection();
            var sql = """
            DELETE FROM Sessions WHERE id = @id
            """;

            try
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id });
                return rowsAffected > 0; // Returns true if something was deleted
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to delete session", ex);
            }
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }


    }
}
