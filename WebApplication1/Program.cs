using Aplicacao.AppService.Personagem;
using Infra.Repositorio;
using Infra.Repositorio.Entidades.Personagens;
using Infra.Repositorio.Entidades.Personagens.Interface;
using Infra.Repositorio.Personagens.Interface;
using Journey.Api.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Conexão com o banco
var configuration = builder.Configuration;
var stringConexao = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrdemDbContext>(options => options.UseMySql(stringConexao, new MySqlServerVersion(new Version(8, 0))));

// Dependencia de injecao das interfaces e classes
builder.Services.AddScoped<IPersonagemRepositorio, PersonagemRepositorio>();
builder.Services.AddScoped<IPersonagemAppService, PersonagemAppService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tratativa das exceptions
builder.Services.AddMvc(config => config.Filters.Add(typeof(ExceptionFilters)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
