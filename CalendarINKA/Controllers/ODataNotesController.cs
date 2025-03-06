using System.Text;
using CalendarINKA.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CalendarINKA.Controllers
{
    /// <summary>
    /// Контроллер для управления заметками через OData API.
    /// </summary>
    [Route("odata/notes")] // Явно указываем базовый маршрут для OData-запросов
    [ApiController]
    public class ODataNotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        /// <summary>
        /// Конструктор контроллера с внедрением сервиса заметок.
        /// </summary>
        /// <param name="noteService">Сервис для работы с заметками.</param>
        public ODataNotesController(INoteService noteService) => _noteService = noteService;

        /// <summary>
        /// Получает список всех заметок с поддержкой OData-фильтрации.
        /// </summary>
        /// <remarks>
        /// Примеры запросов:
        /// - Получить все заметки: GET /odata/notes
        /// - Фильтрация по заголовку: GET /odata/notes?$filter=Title eq 'Моя заметка'
        /// - Сортировка по дате создания: GET /odata/notes?$orderby=CreatedAt desc
        /// - Получить первые 5 заметок: GET /odata/notes?$top=5
        /// - Комбинированный запрос: GET /odata/notes?$filter=Title eq 'Моя заметка' and CreatedAt gt 2023-01-01&amp;$orderby=CreatedAt asc&amp;$top=10
        /// </remarks>
        /// <returns>Список заметок в формате JSON.</returns>
        [HttpGet]
        [EnableQuery] // Включаем поддержку OData-запросов
        public async Task<IActionResult> Get() => Ok(await _noteService.GetAllNotesAsync());

        /// <summary>
        /// Экспортирует заметки в CSV с поддержкой OData-фильтрации.
        /// </summary>
        /// <remarks>
        /// Пример запроса: GET /odata/notes/export?$filter=Title eq 'Моя заметка'
        /// </remarks>
        /// <returns>CSV-файл с отфильтрованными заметками.</returns>
        [HttpGet("export")]
        [EnableQuery] // Поддержка фильтрации для экспорта
        public async Task<IActionResult> ExportToCsv()
        {
            var notes = await _noteService.GetAllNotesAsync();
            var csv = new StringBuilder();
            csv.AppendLine("Id,Title,Text,CreatedAt,ReminderAt"); // Убрали UserId
            foreach (var note in notes)
                csv.AppendLine($"{note.Id},\"{note.Title}\",\"{note.Text}\",{note.CreatedAt},{note.ReminderAt}");
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "notes.csv");
        }
    }
}