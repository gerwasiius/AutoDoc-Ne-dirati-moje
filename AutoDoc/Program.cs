//using Blazored.Toast;
using AutoDocFront.Auth;
using AutoDocFront.Components;
using AutoDocFront.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Splunk;
using System.IdentityModel.Tokens.Jwt;

const string OIDC_SCHEME = "MicrosoftOidc";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 2 * 1024 * 1024; // 2MB
});

builder.Host.UseSerilog((ctx, lc) => lc

    .Enrich.FromLogContext() // To get some key information like user id/request id
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Debug()
    .WriteTo.Console(new ElasticsearchJsonFormatter())
    //.WriteTo.SplunkViaTcp(
    //    new SplunkTcpSinkConnectionInfo(builder.Configuration.GetSection("Splunk:Host")?.Value?.ToString(), builder.Configuration.GetValue<int>("Splunk:Port")),
    //    //OPTIONALS (defaults shown)
    //    restrictedToMinimumLevel: LevelAlias.Minimum)
    );

builder.Services.AddAuthentication(OIDC_SCHEME)
    .AddOpenIdConnect(OIDC_SCHEME, options =>
    {
        builder.Configuration.GetSection("OpenIDConnectSettings").Bind(options);
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
        options.MapInboundClaims = false;
        options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
        options.TokenValidationParameters.RoleClaimType = "roles";
        options.MaxAge = TimeSpan.FromHours(8);
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("roles");
        options.Scope.Add("offline_access");
    })
 .AddCookie(options =>
 {
     options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
 });

builder.Services.ConfigureCookieOidcRefresh(CookieAuthenticationDefaults.AuthenticationScheme, OIDC_SCHEME);
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

// configuring clients
DependencyContainer.RegisterClients(builder.Services, builder.Configuration);
// configuring services
DependencyContainer.RegisterService(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
