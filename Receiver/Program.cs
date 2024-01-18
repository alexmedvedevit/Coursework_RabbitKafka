using Prometheus;
using Receiver.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFinanceService, FinanceService>();
builder.Services.AddHostedService<RabbitListener>();
builder.Services.AddHostedService<KafkaListener>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMetricServer();

app.MapControllers();

app.Run();
