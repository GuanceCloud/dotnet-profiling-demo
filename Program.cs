// Uncomment below if you want to use HTTP.sys as the server
//#define USE_HTTP_SYS

#if USE_HTTP_SYS

using Microsoft.AspNetCore.Server.HttpSys;

var builder = WebApplication.CreateBuilder(args);

if (OperatingSystem.IsWindows())
{
    builder.WebHost.UseHttpSys(options =>
    {
        options.AllowSynchronousIO = false;
        options.Authentication.Schemes = AuthenticationSchemes.None;
        options.Authentication.AllowAnonymous = true;
        options.MaxConnections = null;
        options.MaxRequestBodySize = 64 << 20;
        options.UrlPrefixes.Add("http://localhost:8080");
    });
}

#else

var builder = WebApplication.CreateBuilder(args);

#endif

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("http://*:8080");