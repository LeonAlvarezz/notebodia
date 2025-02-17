namespace notebodia_api.Migrations
{
    public sealed class DbConfiguration(IConfiguration _configuration)
    {
        public string ConnectionString => _configuration.GetRequiredSection("ConnectionString").Get<ConnectionString>()?.DefaultConnection ?? throw new InvalidOperationException("Missing Connection String");
    }
}
