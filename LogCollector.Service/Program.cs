using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<LogCollectorDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(LogEntryProfile));



var app = builder.Build();



app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseMiddleware<LogCollectorMiddleware>();
}

app.UseHttpsRedirection();

app.MapPost("/api/logs", async (LogCollectorDbContext db, LogEntryPost logEntryPost, IMapper mapper) =>
{
	var logEntry = mapper.Map<LogEntry>(logEntryPost);
	await db.Logs.AddAsync(logEntry);
	await db.SaveChangesAsync();
	return Results.Created($"/api/logs/{logEntry.Id}", logEntry);
});

app.Run();
