using DbUp;
using System.Reflection;

namespace notebodia_api.Migrations
{
    public sealed class DbUpMigrator(DbConfiguration _configuration) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stopToken) => await Task.Run(() =>
        {
            var upgrader = DeployChanges.To.SqlDatabase(_configuration.ConnectionString).WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()).LogToConsole().Build();
            var result = upgrader.PerformUpgrade();
        }, stopToken);
    }
}
