using AutoDocService.DL.DBContext;
using AutoDocService.Helpers.Mapper;
using AutoDocService.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prometheus;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "AutoDoc Service",
        Description = "*This API is used to get relevant data for AutoDoc application.* \n\n",
        Contact = new OpenApiContact
        {
            Name = "Zeljko Djakovic",
            Email = "zeljko.djakovic@raiffeisengroup.ba"
        },
        License = new OpenApiLicense
        {
            Name = "Raiffeisen BANK dd Bosnia and Herzegovina",
            Url = new Uri("https://raiffeisenbank.ba/")
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//database connection
//builder.Services.AddDbContextPool<ContractGenerationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("autoDocDB"), e => e.UseCompatibilityLevel(120)));
builder.Services.AddDbContext<ContractGenerationContext>(options =>
    options.UseSqlite("Data Source=local_autodoc.db"));

//health check
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("TimeTrackerDB"), "select 1");

//healtCheck
builder.Services.AddHealthChecks();

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add services to the container.
builder.Host.UseSerilog((ctx, lc) => lc

    .Enrich.FromLogContext() // To get some key information like user id/request id
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Debug()
    .WriteTo.Console(new ElasticsearchJsonFormatter())
    //To do check with Security team
    //.WriteTo.SplunkViaTcp(
    //    new SplunkTcpSinkConnectionInfo(builder.Configuration.GetSection("Splunk:Host")?.Value?.ToString(), builder.Configuration.GetValue<int>("Splunk:Port")),
    //    //OPTIONALS (defaults shown)
    //    restrictedToMinimumLevel: LevelAlias.Minimum)
    );

//Add [Authorize] attribute to all Controller endpoints if you want to restrict access to your API
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(async options =>
//{
//    var issuer = builder.Configuration.GetSection("JwtIssuerOptions:Issuer")?.Value?.ToString();
//    var audience = builder.Configuration.GetSection("JwtIssuerOptions:Audience")?.Value?.ToString();
//    var wellKnown = builder.Configuration.GetSection("JwtIssuerOptions:WellKnown")?.Value?.ToString();

//    var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
//        issuer + wellKnown,
//        new OpenIdConnectConfigurationRetriever());

//    var config = await configurationManager.GetConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
//    //var discoveryDocument = await configurationManager.GetConfigurationAsync();
//    //var signingKeys = discoveryDocument.IdTokenSigningAlgValuesSupported;

//    //options.RefreshInterval = new TimeSpan(0, 0, 0, 2); //2 seconds

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        TryAllIssuerSigningKeys = true,
//        RequireExpirationTime = true,
//        ValidateAudience = false,
//        RequireSignedTokens = true,
//        ValidateIssuer = true,
//        ValidIssuer = issuer,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKeys = config.SigningKeys,
//        ValidateLifetime = true,
//        // Allow for some drift in server time
//        // (a lower value is better; we recommend two minutes or less)
//        ClockSkew = System.TimeSpan.Zero
//    };
//    options.AutomaticRefreshInterval = new TimeSpan(0, 0, 10, 0);  //10 minutes});
//    options.RefreshInterval = new TimeSpan(0, 0, 0, 10);
//    options.Events = new JwtBearerEvents
//    {
//        OnAuthenticationFailed = context =>
//        {
//            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
//            {
//                context.Response.Headers.Append("Token-Expired", "true");
//            }

//            if (context.Exception.GetType() == typeof(SecurityTokenInvalidSigningKeyException))
//            {
//                context.Response.Headers.Append("SigningKey", "true");
//            }
//            return Task.CompletedTask;
//        }
//    };
//});

// configuring clients
DependencyContainer.RegisterClients(builder.Services, builder.Configuration);
// configuring services
DependencyContainer.RegisterService(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMetricServer();
app.UseHttpMetrics(options =>
{
    options.RequestCount.Enabled = true;

    options.RequestDuration.Histogram = Metrics.CreateHistogram($"{Assembly.GetExecutingAssembly().GetName().Name?.Replace(" ", "_")}http_request_duration_seconds", $"{Assembly.GetExecutingAssembly().GetName().Name?.Replace(".", "_")} Metrika",
        new HistogramConfiguration
        {
            Buckets = Histogram.LinearBuckets(start: 0, width: 0.2, count: 50),
            LabelNames = new[] { "code", "method", "controller", "action" },
        });
});

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(report));
        await context.Response.Body.WriteAsync(bytes);
    },
    ResultStatusCodes =
                             {
                               [HealthStatus.Healthy] = StatusCodes.Status200OK,
                               [HealthStatus.Degraded] = StatusCodes.Status200OK,
                               [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                             }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
