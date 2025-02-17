using Dapper;
using Microsoft.Data.SqlClient;
using notebodia_api.Db;
using notebodia_api.Types;

namespace notebodia_api.Repositories
{
    public class NoteRepository
    {

        private readonly DapperDbContext _dbContext;
        public NoteRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Note>> GetAllById(Guid UserId, string? keyword)
        {
            try
            {
                var connection = _dbContext.GetConnection();
                var sql = """
                SELECT * FROM Notes
                WHERE user_id = @UserId
                AND (@title IS NULL OR title LIKE @title)
                """;
                var parameters = new { UserId, title = keyword != null ? $"%{keyword}%" : null };

                var notes = await connection.QueryAsync<Note>(sql, parameters);
                return notes.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to Fetch Note", ex);
            }
        }

        public async Task<Note> GetById(Guid NoteId)
        {
            try
            {
                var connection = _dbContext.GetConnection();
                var sql = """
                SELECT * FROM Notes
                WHERE id = @NoteId
                """;
                var note = await connection.QueryFirstOrDefaultAsync<Note>(sql, new { NoteId });
                return note;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to Fetch Note", ex);
            }
        }

        public async Task<List<Note>> GetAllAsync()
        {
            try
            {
                var connection = _dbContext.GetConnection();
                var sql = """
                SELECT * FROM Notes
                """;
                var notes = await connection.QueryAsync<Note>(sql);
                return notes.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to Fetch Note", ex);
            }
        }

        public async Task<Note> CreateAsync(Guid UserId, CreateNotePayload payload)
        {
            try
            {
                var connection = _dbContext.GetConnection();
                var sql = """
                INSERT INTO Notes(user_id, title, content)
                VALUES (@UserId, @Title, @Content)
                RETURNING id, user_id, title, content, updated_at, created_at, published_at
                """;
                var note = await connection.QueryFirstAsync<Note>(sql, new
                {
                    UserId,
                    payload.Title,
                    payload.Content,
                });
                return note;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to Fetch Note", ex);
            }
        }
        public async Task<Note> UpdateAsync(Guid NoteId, CreateNotePayload payload)
        {
            try
            {
                var connection = _dbContext.GetConnection();
                var sql = """
                Update Notes
                SET title = @Title, content = @content, updated_at = @UpdatedAt
                WHERE id = @NoteId;
                RETURNING id, user_id, title, content, updated_at, created_at, published_at
                """;
                var note = await connection.QueryFirstAsync<Note>(sql, new
                {
                    payload.Title,
                    payload.Content,
                    UpdatedAt = DateTime.Now,
                    NoteId,
                });
                return note;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to Update Note", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid NoteId)
        {
            try
            {
                var connection = _dbContext.GetConnection();
                var sql = """
                DELETE FROM Notes WHERE id = @NoteId
                """;
                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    NoteId,
                });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to Update Note", ex);
            }
        }
    }
}