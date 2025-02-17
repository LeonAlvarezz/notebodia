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
    public class SessionRepository
    {
        private readonly DapperDbContext _dbContext;
        public SessionRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Session> GetSessionByIdAsync(Guid id)
        {
            var connection = _dbContext.GetConnection();
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
            var connection = _dbContext.GetConnection();
            var sql = """
            INSERT INTO Sessions (id, user_id, expires_at) 
            VALUES (@Id, @UserId, @ExpiresAt)
            RETURNING id, user_id, expires_at, created_at
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
            var connection = _dbContext.GetConnection();
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
    }
}
