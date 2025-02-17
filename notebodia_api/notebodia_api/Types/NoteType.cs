
namespace notebodia_api.Types
{
    public record CreateNotePayload
    {
        public required string Title { get; init; }
        public required string Content { get; init; }
    }
    public record NoteFilter
    {
        public string Keyword { get; init; }
    }
}
