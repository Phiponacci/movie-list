
using Microsoft.EntityFrameworkCore;
using movie_list.ApiClient;
using Library.Models;
using Library.DataSource.Store;
using Library.DataSource;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MovieDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MovieDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<MovieApiClient<Movie>>();

builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<AppUser>()
    .AddEntityFrameworkStores<MovieDbContext>();

builder.Services.AddScoped<IStore<Movie>, MovieStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
