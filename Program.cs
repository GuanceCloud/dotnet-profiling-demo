// Uncomment below if you want to use HTTP.sys as the server
//#define USE_HTTP_SYS

using System.Collections;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


#if USE_HTTP_SYS

if (OperatingSystem.IsWindows())
{
    builder.WebHost.UseHttpSys();
}
else
{
    builder.WebHost.UseKestrel();
}
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

app.MapGet("/",  () =>
{
    StringBuilder sb = new StringBuilder();

    var environmentVariables = Environment.GetEnvironmentVariables();

    foreach (DictionaryEntry variable in environmentVariables)
    {
        sb.AppendFormat("{0}\t\t{1}\n", variable.Key, variable.Value);
    }
    

    return sb.ToString();
});

app.Run();
