var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Benteler_WorkPlan_Web>("benteler-workplan-web");

builder.AddProject<Projects.Benteler_WorkPlan_Api>("benteler-workplan-api");

builder.Build().Run();
