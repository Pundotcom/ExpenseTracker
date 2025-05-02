using ExpenseTracker;
using ExpenseTracker.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot",
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory()
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ExpenseDb"));

builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCors("AllowAll");


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseTracker API");
    c.RoutePrefix = "swagger";
});


if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();


app.MapGet("/", () => "ExpenseTracker API is running");

app.Run();
