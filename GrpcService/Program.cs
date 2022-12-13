using Core.Repositories;
using GrpcService;
using GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IPersonRepository, PersonInMemoryRepository>();
builder.Services.AddScoped<IRoleRepository, RoleInMemoryRepository>();
builder.Services.AddScoped<ITestDataRepository, TestEntityRepositoryInMemory>();
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseMiddleware<JwtAuthMiddleware>();
app.MapGrpcService<GreeterService>();
app.MapGrpcService<AuthService>();
app.MapGrpcService<RolesService>();
app.MapGrpcService<PersonsService>();
app.MapGrpcService<EnitityService>();

app.Run();
