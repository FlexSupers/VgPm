using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using VG.Pm.Data;
using VG.Pm.Data.Services;
using VG.Pm.Hubs;
using VG.Pm.PmDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<PmDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PmDbContext")));

builder.Services.AddMudServices();
builder.Services.AddScoped<LogApplicationErrorService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<TaskTypeService>();
/*builder.Services.AddSignalR();*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

/*app.MapHub<BoardHub>("/boardHub");*/

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
