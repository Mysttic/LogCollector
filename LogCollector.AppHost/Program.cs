using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Services
builder.AddProject<LogCollector_Service>("logcollector-service");

builder.AddProject<LogCollector_Monitor>("logcollector-monitor");

builder.AddProject<LogCollector_DockerManager>("logcollector-dockermanager");

// App
var backend = builder.AddProject<LogCollector_Browser_Server>("logcollector-browser-server");

var frontend = builder.AddNpmApp("frontend",
	@"../LogCollector.Browser/LogCollector.Browser.Client",
	"dev")
	.WithReference(backend)
	.WithEndpoint(
	targetPort: 5173,
	port: 5173,
	isProxied: false,
	scheme: "https",
	env: "PORT");

builder.Build().Run();
