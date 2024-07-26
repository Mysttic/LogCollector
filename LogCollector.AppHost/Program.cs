var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LogCollector_Service>("logcollector-service");

builder.AddProject<Projects.LogCollector_Browser_Server>("logcollector-browser-server");

builder.AddProject<Projects.LogCollector_Monitor_Server>("logcollector-monitor-server");

builder.Build().Run();
