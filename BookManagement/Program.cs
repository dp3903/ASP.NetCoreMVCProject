using Microsoft.EntityFrameworkCore;
using BookManagement.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(
    options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(10);
    }
);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<AppDBContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("BookManageDBConnection"));
    }
);
builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
builder.Services.AddScoped<IBookRepository, SQLBookRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
