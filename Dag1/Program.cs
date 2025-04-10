using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Dag1.Components;
using Dag1.Components.Account;
using Dag1.Data;
using Dag1.Code;
using Dag1.Data.Todo;

var builder = WebApplication.CreateBuilder(args);

// ✅ Kestrel konfiguration – henter certifikat via user secrets / appsettings
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<HashingHandler>();
builder.Services.AddScoped<SynmetriE>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

// ✅ Identity-database (Nice)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ NYT: Todo-database (Todo)
var todoConnection = builder.Configuration.GetConnectionString("TodoMSSQLConnection") ??
    throw new InvalidOperationException("Connection string 'TodoMSSQLConnection' not found.");

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(todoConnection));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("RequiredAdminStraterRole", policy =>
    {
        policy.RequireRole("Admin");
    });

    option.AddPolicy("AuthenticatedUser", policy =>
    {
        policy.RequireRole("User");
    });
});

async Task EnsureRoles(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User" };

    foreach (var role in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

var app = builder.Build();

// ✅ Tjek: kører vi på Kestrel? Hvis ikke → smid fejl (IIS er ikke tilladt)
var serverType = app.Services
    .GetRequiredService<Microsoft.AspNetCore.Hosting.Server.IServer>()
    .GetType().Name;

// Smid kun fejl hvis serveren er IIS Express
if (serverType == "IISHttpServer")
{
    throw new InvalidOperationException("❌ Denne app er ikke konfigureret til at køre under IIS Express.");
}

// ✅ Roller initialiseres
await EnsureRoles(app.Services.CreateScope().ServiceProvider);

// ✅ Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
