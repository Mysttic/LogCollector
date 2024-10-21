using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
var dbConnection = builder.Configuration.GetConnectionString("LocalConnection");
#else
var dbConnection = builder.Configuration.GetConnectionString("DefaultConnection");
#endif

builder.Services.AddDbContext<LogCollectorDbContext>(options =>
{
	options.UseSqlServer(dbConnection);
});

builder.Services.AddHangfire(config =>
			config
			.UseSimpleAssemblyNameTypeSerializer()
			.UseRecommendedSerializerSettings()
			.UseSqlServerStorage(dbConnection));

builder.Services.AddHangfireServer();

builder.Services.AddScoped<LogMonitoringService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<ISMSService, SMSService>();
builder.Services.AddHttpClient<ICustomApiCallService, CustomApiCallService>();

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHangfireDashboard("/dashboard");

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<LogMonitoringService>(
	"Run every minute",
	service => service.CheckLogsAsync(),
	Cron.Minutely);

app.Run();