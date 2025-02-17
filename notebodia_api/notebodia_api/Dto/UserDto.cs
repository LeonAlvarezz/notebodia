namespace notebodia_api.Dto
{
    public record UserDto
    {
        public Guid Id { get; init; }
        public required string Username { get; init; }
        public required string Email { get; init; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
