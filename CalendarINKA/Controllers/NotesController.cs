using System.Text;
using CalendarINKA.Abstractions;
using CalendarINKA.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalendarINKA.Controllers
{
    /// <summary>
    /// Контроллер для управления заметками через REST API.
    /// </summary>
    [Route("api/notes")] // Явно указываем базовый маршрут для REST-запросов
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        /// <summary>
        /// Конструктор контроллера с внедрением сервиса заметок.
        /// </summary>
        /// <param name="noteService">Сервис для работы с заметками.</param>
        public NotesController(INoteService noteService) => _noteService = noteService;

        /// <summary>
        /// Получает список всех заметок.
        /// </summary>
        /// <returns>Список всех заметок в формате JSON.</returns>
        [HttpGet] // GET /api/notes
        public async Task<IActionResult> GetAll() => Ok(await _noteService.GetAllNotesAsync());

        /// <summary>
        /// Получает заметку по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        /// <returns>Заметка или 404, если заметка не найдена.</returns>
        [HttpGet("{id}")] // GET /api/notes/{id}
        public async Task<IActionResult> GetById(Guid id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            return note == null ? NotFound() : Ok(note);
        }

        /// <summary>
        /// Создает новую заметку.
        /// </summary>
        /// <param name="note">Данные новой заметки.</param>
        /// <returns>Созданная заметка с кодом 201 и ссылкой на неё.</returns>
        [HttpPost] // POST /api/notes
        public async Task<IActionResult> Create([FromBody] Note note)
        {
            note.Id = Guid.NewGuid();
            await _noteService.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        /// <summary>
        /// Обновляет существующую заметку.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        /// <param name="note">Обновленные данные заметки.</param>
        /// <returns>204 при успехе или 400, если ID не совпадает.</returns>
        [HttpPut("{id}")] // PUT /api/notes/{id}
        public async Task<IActionResult> Update(Guid id, [FromBody] Note note)
        {
            if (id != note.Id) return BadRequest();
            await _noteService.UpdateNoteAsync(note);
            return NoContent();
        }

        /// <summary>
        /// Удаляет заметку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        /// <returns>204 при успехе.</returns>
        [HttpDelete("{id}")] // DELETE /api/notes/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Экспортирует все заметки в CSV-файл.
        /// </summary>
        /// <returns>CSV-файл с данными всех заметок.</returns>
        [HttpGet("export")] // GET /api/notes/export
        public async Task<IActionResult> ExportToCsv()
        {
            var notes = await _noteService.GetAllNotesAsync();
            var csv = new StringBuilder();
            csv.AppendLine("Id,Title,Text,CreatedAt,ReminderAt,UserId");
            foreach (var note in notes)
                csv.AppendLine($"{note.Id},\"{note.Title}\",\"{note.Text}\",{note.CreatedAt},{note.ReminderAt}");
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "notes.csv");
        }
    }
}