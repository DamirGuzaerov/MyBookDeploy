using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.OAuth;
using MyBook.Entities;
using MyBook.Infrastructure.Repositories;
using MyBook.Validation;
using Repositories;
using System.Security.Claims;
using MyBook.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication;
using MyBook.Services;
using MyBook.Core.Interfaces;
using MyBook.Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUserValidator<User>, UserValidator>()
    .AddTransient<IPasswordValidator<User>, PasswordValidator>(serv => new PasswordValidator(6));

var defaultConnectionString = string.Empty;

if (builder.Environment.EnvironmentName == "Production") {
    defaultConnectionString = builder.Configuration.GetConnectionString("DefaultString");
}
else
{
    // Use connection string provided at runtime by Heroku.
    var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    
    connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
    var userPassSide = connectionUrl.Split("@")[0];
    var hostSide = connectionUrl.Split("@")[1];

    var user = userPassSide.Split(":")[0];
    var password = userPassSide.Split(":")[1];
    var host = hostSide.Split("/")[0];
    var database = hostSide.Split("/")[1].Split("?")[0];

    defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}
builder.Services.AddDbContext<MyBookContext>(options =>
        options.UseNpgsql(defaultConnectionString,
            options => options.MigrationsAssembly("MyBook")), ServiceLifetime.Transient)
    .AddScoped<IGenericRepository<Book>, EfGenericRepository<Book>>()
    .AddScoped<IGenericRepository<Author>, EfGenericRepository<Author>>()
    .AddScoped<IGenericRepository<Genre>, EfGenericRepository<Genre>>()
    .AddScoped<EfBookRepository>()
    .AddScoped<EFGenreRepository>()
    .AddScoped<EfAuthorRepository>()
    .AddScoped<IGenericRepository<DownloadLink>, EfGenericRepository<DownloadLink>>()
    .AddScoped<IGenericRepository<MyBook.Entities.Type>, EfGenericRepository<MyBook.Entities.Type>>()
    .AddScoped<EFTypeRepository>().AddScoped<IGenericRepository<object>, EfGenericRepository<object>>()
    .AddScoped<IGenericRepository<BookCenter>, EfGenericRepository<BookCenter>>()
    .AddScoped<EFBookCenterRepository>()
    .AddScoped<IGenericRepository<User>, EfGenericRepository<User>>()
    .AddScoped<EFUserRepository>()
    .AddScoped<IGenericRepository<History>, EfGenericRepository<History>>()
    .AddScoped<EFHistoryRepository>()
    .AddScoped<IGenericRepository<UserSubscr>, EfGenericRepository<UserSubscr>>()
    .AddScoped<EFUserSubscrRepository>()
    .AddScoped<ILanguageFilterGetter, LanguageFilterGetter>()
    .AddScoped<IGenresFilterGetter, GenreFilterGetter>()
    .AddScoped<IGenericRepository<Rating>, EfGenericRepository<Rating>>()
    .AddScoped<IGenericRepository<BookCenter>, EfGenericRepository<BookCenter>>()
    .AddScoped<IMailService, MailService>()
    .AddSingleton<IUserConnectionManager, UserConnectionManager>()
    .AddScoped<INotificationService, NotificationService>()
    .AddScoped<IRecommendationsService, RecommendationsService>()
    .AddSingleton<IPaymentService, PaymentService>();

var serviceProvider = builder.Services.BuildServiceProvider();
try
{
    var dbContext = serviceProvider.GetRequiredService<MyBookContext>();
    dbContext.Database.Migrate();
}
catch
{
    /*throw new Exception();*/
}
builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyBook.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
});
builder.Services.AddIdentity<User, IdentityRole>(options => options.User.RequireUniqueEmail = true).AddEntityFrameworkStores<MyBookContext>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadersOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Reader"));
    options.AddPolicy("AdminsOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});
builder.Services.AddAuthentication().AddOAuth("VK", "VKontakte", config =>
{
    config.ClientId = builder.Configuration["VkAuth:AppId"];
    config.ClientSecret = builder.Configuration["VkAuth:AppSecret"];
    config.ClaimsIssuer = "VKontakte";
    config.CallbackPath = new PathString("/signin-vkontakte-token");
    config.AuthorizationEndpoint = "https://oauth.vk.com/authorize";
    config.TokenEndpoint = "https://oauth.vk.com/access_token";
    config.Scope.Add("email");
    config.Scope.Add("first_name");
    config.Scope.Add("last_name");
    config.Scope.Add("bdate");
    config.ClaimActions.MapJsonKey(ClaimTypes.Name, "first_name");
    config.ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
    config.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
    config.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    config.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "bdate");
    config.SaveTokens = true;
    config.Events = new OAuthEvents
    {
        OnCreatingTicket = context =>
        {
            context.RunClaimActions(context.TokenResponse.Response.RootElement);
            return Task.CompletedTask;
        }
    };
});
var app = builder.Build();
app.UseSession();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapHub<NotificationHub>("/NotificationHub");
app.MapHub<NotificationUserHub>("/NotificationUserHub");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
