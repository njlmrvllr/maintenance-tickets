using System.Text.Json.Serialization;
using Dapper;
using MaintenanceTickets.API.Maintenance.Application.Internal.CommandServices;
using MaintenanceTickets.API.Maintenance.Application.Internal.QueryServices;
using MaintenanceTickets.API.Maintenance.Application.Services;
using MaintenanceTickets.API.Maintenance.Domain.Repositories;
using MaintenanceTickets.API.Maintenance.Infrastructure.Persistence.Dapper;
using MaintenanceTickets.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using MaintenanceTickets.API.Shared.Infrastructure.Persistence.Dapper;

var builder = WebApplication.CreateBuilder(args);

// Lower case URLs and kebab-case controller routes
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services
    .AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    // Enums travel as text ("Pending", "InProgress") instead of numbers
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// RFC 7807 ProblemDetails payloads for errors
builder.Services.AddProblemDetails();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());

// CORS for the Angular frontend (origins come from configuration)
const string frontendCorsPolicy = "Frontend";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
    options.AddPolicy(frontendCorsPolicy, policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

// Dapper: map snake_case columns (created_at) to PascalCase properties (CreatedAt)
DefaultTypeMap.MatchNamesWithUnderscores = true;

// Shared Bounded Context Injection Configuration
builder.Services.AddSingleton<IDbConnectionFactory, MySqlConnectionFactory>();

// Maintenance Bounded Context Injection Configuration
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketCommandService, TicketCommandService>();
builder.Services.AddScoped<ITicketQueryService, TicketQueryService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

// Swagger UI is enabled in all environments so the evaluators can explore the API.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(frontendCorsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();