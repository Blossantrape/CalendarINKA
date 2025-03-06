using CalendarINKA.Models;

namespace CalendarINKA.Abstractions
{
    /// <summary>
    /// Интерфейс сервиса для управления заметками.
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Получает все заметки.
        /// </summary>
        Task<IEnumerable<Note>> GetAllNotesAsync();

        /// <summary>
        /// Получает заметку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        Task<Note?> GetNoteByIdAsync(Guid id);

        /// <summary>
        /// Создает новую заметку.
        /// </summary>
        /// <param name="note">Данные новой заметки.</param>
        Task CreateNoteAsync(Note note);

        /// <summary>
        /// Обновляет существующую заметку.
        /// </summary>
        /// <param name="note">Обновленные данные заметки.</param>
        Task UpdateNoteAsync(Note note);

        /// <summary>
        /// Удаляет заметку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        Task DeleteNoteAsync(Guid id);

        /// <summary>
        /// Получает заметки, у которых наступило время напоминания.
        /// </summary>
        /// <param name="currentTime">Текущее время в UTC.</param>
        Task<IEnumerable<Note>> GetNotesDueAsync(DateTime currentTime);
    }
}