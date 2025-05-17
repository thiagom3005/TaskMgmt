using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskMgmt.Application.Services;
using TaskMgmt.Domain.Interfaces;
using TaskMgmt.Domain.Validators;
using TaskMgmt.Infrastructure.Data;
using TaskMgmt.Infrastructure.Repositories;

SQLitePCL.Batteries_V2.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<TarefaInputDtoValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TaskMgmt.API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Popula o banco de dados com 1000+ tarefas se estiver vazio
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Tarefas.Any())
    {
        var tarefas = Enumerable.Range(1, 1100).Select(i => new TaskMgmt.Domain.Entities.Tarefa
        {
            Titulo = $"Tarefa {i}",
            Descricao = $"Descrição da tarefa {i}",
            Status = (TaskMgmt.Domain.Enums.StatusTarefa)(i % 3), // Alterna entre os status
            DataVencimento = DateTime.Today.AddDays(i % 30)
        }).ToList();

        db.Tarefas.AddRange(tarefas);
        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
