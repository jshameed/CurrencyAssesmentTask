using AspNetCoreRateLimit;
using DemoCurrency;
using DemoCurrency.Filters;
using DemoCurrency.Helper;
using DemoCurrency.Services;
using Microsoft.Extensions.Configuration;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(configure => configure.AddDebug().AddConsole());
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddHttpClient("FrankfurterClient",
     configureClient =>
     {
         configureClient.BaseAddress = new Uri("https://api.frankfurter.app");
         configureClient.Timeout = new TimeSpan(0, 0, 30);
     })
     .AddPolicyHandler(Policy.HandleResult<HttpResponseMessage>(
         response => !response.IsSuccessStatusCode).RetryAsync(3))
     .ConfigurePrimaryHttpMessageHandler(() =>
     {
         var handler = new SocketsHttpHandler();
         return handler;
     });

builder.Services.AddScoped<BlockCurrencyActionFilter>();
builder.Services.AddScoped<ICurrencyServices, CurrencyServices>();
builder.Services.AddScoped<FrankfurterAPIClient>();
builder.Services.AddSingleton<JsonSerializerOptionsHelper>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddResponseCaching();
builder.Services.AddControllers();


// Add Memory Cache
builder.Services.AddMemoryCache();

// Load Rate Limit configurations
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

// Inject Rate Limiting services
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ProblemDetailsMiddleware>();
app.UseResponseCaching();
app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
