namespace notebodia_api.Types
{
    public record CreateUserPayload
    {
        public required string Username { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
