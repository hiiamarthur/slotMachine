using Blazesoft.SlotMachine.Common.Data;
using Blazesoft.SlotMachine.Common.Interfaces;
using Blazesoft.SlotMachine.Domain.BusinessObjects;
using Blazesoft.SlotMachine.Domain.Common;
using Blazesoft.SlotMachine.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<MatrixSize>(builder.Configuration.GetSection("MatrixSize"));
builder.Services.AddSingleton<ISpinToWin,SpinToWin>();
builder.Services.AddSingleton<IMongoClient>(provider =>
{
    return new MongoClient(builder.Configuration.GetConnectionString("SlotMachineDb"));
});
    builder.Services.AddScoped(provider => {
    var client = provider.GetRequiredService<IMongoClient>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    return client.GetDatabase(configuration.GetValue<string>("DatabaseName"));
});
var repositoryInterfaceType = typeof(IBaseRepository<>).MakeGenericType(typeof(Player));
var repositoryImplementationType = typeof(BaseRepository<>).MakeGenericType(typeof(Player));
builder.Services.AddScoped(repositoryInterfaceType, repositoryImplementationType);
builder.Services.AddScoped<ISpinWheelFactory, SpinWheelFactory>();
builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer().AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Blazesoft Slot Machine API", Version = "v1" });
});




builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy.
//    options.FallbackPolicy = options.DefaultPolicy;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


using StringWriter sw = new();
app.Services.GetRequiredService<ISwaggerProvider>().GetSwagger("v1").SerializeAsV3(new OpenApiYamlWriter(sw));
File.WriteAllText("Blazesoft.SlotMachine.Api.v1.yaml", sw.ToString());
app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
