var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LogCollector_Service>("logcollector-service");

builder.AddProject<Projects.LogCollector_Browser_Backend>("logcollector-browser-backend");

builder.AddProject<Projects.LogCollector_Browser_Server>("logcollector-browser-server");

builder.Build().Run();
