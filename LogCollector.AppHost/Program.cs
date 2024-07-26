var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LogCollector_Service>("logcollector-service");

builder.Build().Run();
