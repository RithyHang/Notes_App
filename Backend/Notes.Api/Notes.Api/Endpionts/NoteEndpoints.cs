using Dapper;
using Microsoft.Data.SqlClient;
using Notes.Api.Models;
using Notes.Api.Services;

namespace Notes.Api.Endpionts
{
    public static class NoteEndpoints
    {
        public static void MapNoteEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("notes", async (SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = "SELECT NoteId, Title, Content, CreatedAt, UpdatedAt FROM tblNote";

                var notes = await connection.QueryAsync<Note>(sql);

                return Results.Ok(notes);
            });

            builder.MapGet("notes/{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = """
                    SELECT NoteId, Title, Content, CreatedAt, UpdatedAt 
                    FROM tblNote WHERE 
                    NoteId = @NoteId
                    """;

                var note = await connection.QuerySingleOrDefaultAsync<Note>(sql, new { NoteId = id });

                if (note is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(note);
            });

            builder.MapPost("note", async (Note note, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = """
                    INSERT INTO tblNote (Title, Content, CreatedAt, UpdatedAt) 
                    VALUES (@Title, @Content, GetDate(), GetDate())
                    """;

                await connection.ExecuteAsync(sql, note);

                return Results.Ok();
            });

            builder.MapPut("note/{id}", async (int id, Note note, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                note.NoteId = id;

                const string sql = """
                    UPDATE tblNote
                    SET Title = @Title, 
                        Content = @Content, 
                        UpdatedAt = GetDate()
                    WHERE NoteId = @NoteId
                    """;
                await connection.ExecuteAsync(sql, note);

                return Results.NoContent();
            }); 

            builder.MapDelete("note/{id}", async (int id, SqlConnectionFactory sqlConnectionFactory) =>
            {
                using var connection = sqlConnectionFactory.Create();

                const string sql = "DELETE FROM tblNote WHERE NoteId = @NoteId";

                await connection.ExecuteAsync(sql, new { NoteId = id });

                return Results.NoContent();
            });
        }
    }
}
