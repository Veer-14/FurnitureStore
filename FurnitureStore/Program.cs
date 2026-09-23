using FurnitureStore.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

static string NormalizeNpgsqlConnectionString(string? raw)
{
    if (string.IsNullOrWhiteSpace(raw))
        throw new InvalidOperationException(
            "Connection string 'DefaultConnection' is missing or empty. " +
            "Set it as an environment variable named ConnectionStrings__DefaultConnection.");

    raw = raw.Trim().Trim('"');

    // Accept the raw postgres://user:pass@host:port/db?query style URL
    // (e.g. what Neon gives you) and convert it to the key-value format
    // Npgsql actually expects, instead of requiring it to already be correct.
    if (raw.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        raw.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var uri = new Uri(raw);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var database = uri.AbsolutePath.TrimStart('/');

        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        var sslMode = string.Equals(query["sslmode"], "disable", StringComparison.OrdinalIgnoreCase)
            ? SslMode.Disable
            : SslMode.Require;

        var csb = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Username = username,
            Password = password,
            Database = database,
            SslMode = sslMode,
            TrustServerCertificate = true
        };

        return csb.ConnectionString;
    }

    // Already key-value format (Host=...;Database=...;...) — validate it
    // parses, so a malformed value fails fast with a clear error at startup
    // instead of surfacing as an obscure exception mid-request.
    return new NpgsqlConnectionStringBuilder(raw).ConnectionString;
}

var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = NormalizeNpgsqlConnectionString(rawConnectionString);

builder.Services.AddDbContext<FurnitureDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/Login";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();