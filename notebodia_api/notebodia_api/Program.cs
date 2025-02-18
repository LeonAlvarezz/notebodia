using notebodia_api.Response;
using notebodia_api.Repositories;
using notebodia_api.Services;
using Dapper;
using notebodia_api.Db;


DefaultTypeMap.MatchNamesWithUnderscores = true;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// DB
builder.Services.AddScoped<DapperDbContext>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new DapperDbContext(connectionString);
});
// Register Repository
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<SessionRepository>();
builder.Services.AddScoped<NoteRepository>();

// Register Service
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<NoteService>();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddHttpsRedirection(options =>
// {
//     options.RedirectStatusCode = Status307TemporaryRedirect;
//     options.HttpsPort = 443;
// });


builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();
    options.AddPolicy(name: "AllowSpecificOrigin",
                      policy =>
                      {
                          policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                      });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "session_token";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Required for HTTPS
        options.Cookie.SameSite = SameSiteMode.None; // Required for cross-origin
        options.Cookie.Domain = ".sator-tech.live"; // Allows subdomains to access the cookie
        options.LoginPath = "/api/auth/login";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// app.UseHttpsRedirection();
app.UseMiddleware<GlobalErrorMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowSpecificOrigin");



app.Run();
