var builder = DistributedApplication.CreateBuilder(args);

// Services

builder.AddProject<Projects.LogCollector_Service>("logcollector-service");

builder.AddProject<Projects.LogCollector_Monitor>("logcollector-monitor");

// App

var backend = builder.AddProject<Projects.LogCollector_Browser_Server>("logcollector-browser-server");

var frontend = builder.AddNpmApp("frontend",
	@"../LogCollector.Browser/LogCollector.Browser.Client",
	"dev")
	.WithReference(backend)	
	.WithEndpoint(
	targetPort: 5173,
	port: 5173,
	isProxied: false,
	scheme : "https",
	env: "PORT");


builder.Build().Run();
