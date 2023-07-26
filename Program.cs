// Uncomment below if you want to use HTTP.sys as the server
//#define USE_HTTP_SYS

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

app.Run("http://*:8080");