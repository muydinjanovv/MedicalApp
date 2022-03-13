using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Medical.Data;
using Microsoft.EntityFrameworkCore;
using Medical.Entity;
using Microsoft.AspNetCore.Identity;
using Medical;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<AppDbContext>(options => 
{ 
    options.UseSqlServer(builder.Configuration.GetConnectionString("HealthConnection")); 
}); 
 
builder.Services.AddIdentity<User, IdentityRole>(options => 
{ 
    options.Password.RequiredLength = 4; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequireLowercase = false; 
 
    options.User.RequireUniqueEmail = true; 
}).AddEntityFrameworkStores<AppDbContext>(); 
 
builder.Services.ConfigureApplicationCookie(options => 
{ 
    options.Cookie.Name = "Health.identity"; 
    options.LoginPath = "/account/signin"; 
    options.LogoutPath = "/account/signout"; 
 
    options.Cookie.MaxAge = TimeSpan.FromDays(7); 
});
builder.Services.AddHostedService<Seed>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
