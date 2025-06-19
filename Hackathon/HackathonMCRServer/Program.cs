using HackathonMCRServer;
using HackathonMCRServer.Clients;
using HackathonMCRServer.Tools;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server; // ⬅️ Adicione isso

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Debug;
});

var serverInfo = new Implementation { Name = "DotnetMCPServer", Version = "1.0.0" };

builder.Services
    .AddMcpServer(mcp =>
    {
        mcp.ServerInfo = serverInfo;
    })
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(ColaboradorTools).Assembly);


builder.Services.AddHttpClient<ColaboradorApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7137/api/");
});

var app = builder.Build();

await app.RunAsync();
