using Microsoft.EntityFrameworkCore;
using Mvc.BLL.Repositories;
using Mvc.DAL.Data;
using MvcProject1.BLL.Repositories;
using MvcProject1.BLL.RepositoryInterfaces;
using MvcProject1.PL.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

var Services = builder.Services;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

Services.AddDbContext<AppDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MVC1"))
    );



Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
