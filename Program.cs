using SharpSeer.Interfaces;
using SharpSeer.Models;
using SharpSeer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<SharpSeerDbContext>();
builder.Services.AddTransient<IService<Cohort>, CohortService>();
builder.Services.AddTransient<IService<Exam>, ExamService>();
//var conString = builder.Configuration.GetConnectionString("BloggingDatabase") ??
//     throw new InvalidOperationException("Connection string 'BloggingDatabase'" +
//    " not found.");
//builder.Services.AddDbContext<BloggingDatabase>(options =>
//    options.UseSqlServer(conString));

var app = builder.Build();


// Configure the HTTP request pipeline. 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
