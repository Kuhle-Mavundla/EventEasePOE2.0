﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EventEasePOE2._0.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EventEasePOE2_0Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventEasePOE2_0Context") ?? throw new InvalidOperationException("Connection string 'EventEasePOE2_0Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
