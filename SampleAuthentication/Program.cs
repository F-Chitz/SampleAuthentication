using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SampleAuthentication.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SampleAuthenticationContextConnection") ?? throw new InvalidOperationException("Connection string 'SampleAuthenticationContextConnection' not found.");

builder.Services.AddDbContext<SampleAuthenticationContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<SampleAuthenticationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SampleAuthenticationContext>();

builder.Services.AddAuthorization(options =>
    options.AddPolicy("PageI", policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim("HasPageI", bool.TrueString)));
builder.Services.AddAuthorization(options =>
    options.AddPolicy("PageII", policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim("HasPageII", bool.TrueString)));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
