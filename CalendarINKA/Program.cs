using System.Reflection;
using CalendarINKA.Abstractions;
using CalendarINKA.Data;
using CalendarINKA.Hubs;
using CalendarINKA.Models;
using CalendarINKA.Repositories;
using CalendarINKA.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем контроллеры
builder.Services.AddControllers();

// Регистрируем OData с маршрутом /odata для избежания конфликтов с REST API
builder.Services.AddControllers().AddOData(opt =>
    opt.AddRouteComponents("odata", GetEdmModel()) // Префикс "odata" для всех OData-маршрутов
        .Select()
        .Filter()
        .OrderBy()
        .Count()
        .Expand()
        .SetMaxTop(100));

// Настраиваем Swagger для документирования API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calendar API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Регистрируем SignalR для push-уведомлений
builder.Services.AddSignalR();

// Настраиваем подключение к PostgreSQL
builder.Services.AddDbContext<CalendarContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Внедряем зависимости через DI
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddHostedService<ReminderService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

/// <summary>
/// Создает EDM (Entity Data Model) для OData, определяя сущности и маршруты.
/// </summary>
/// <returns>Модель OData для работы с сущностями.</returns>
IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Note>("Notes"); // Определяем сущность Notes для OData
    return builder.GetEdmModel();
}

app.Run();