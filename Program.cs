using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configuração para ler variáveis de ambiente e appsettings.json
builder.Configuration.AddEnvironmentVariables();

// Registrar serviços
builder.Services.AddControllers();
builder.Services.AddHttpClient<CurrencyConverter.Services.CurrencyService>();

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();