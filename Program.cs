using Microsoft.AspNetCore.Identity;
using ReACT;
using ReACT.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddAreaPageRoute("Home", "/Landing", "/");
    options.Conventions.AddAreaPageRoute("Admin", "/ViewCollections", "/admin");
    options.Conventions.AddAreaPageRoute("Company", "/PickupLocations", "/company");
    options.Conventions.AddAreaPageRoute("User", "/Dashboard", "/user");
});

builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();
builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Home/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.RunTailwind("./Styles/input.css", "./wwwroot/css/output.css");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();


app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}"
);

app.Run();