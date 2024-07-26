using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<LogCollectorDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ILogEntryRepository, LogEntryRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IMonitorRepository, MonitorRepository>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader();
		});
});

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "YourAPI v1"));
}
else
{
	app.UseMiddleware<LogCollectorMiddleware>();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.Run();

