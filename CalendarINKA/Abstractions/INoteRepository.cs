using CalendarINKA.Models;

namespace CalendarINKA.Abstractions
{
    /// <summary>
    /// Интерфейс репозитория для работы с данными заметок.
    /// </summary>
    public interface INoteRepository
    {
        /// <summary>
        /// Получает все заметки.
        /// </summary>
        Task<IEnumerable<Note>> GetAllAsync();

        /// <summary>
        /// Получает заметку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        Task<Note?> GetByIdAsync(Guid id);

        /// <summary>
        /// Создает новую заметку.
        /// </summary>
        /// <param name="note">Данные новой заметки.</param>
        Task CreateAsync(Note note);

        /// <summary>
        /// Обновляет существующую заметку.
        /// </summary>
        /// <param name="note">Обновленные данные заметки.</param>
        Task UpdateAsync(Note note);

        /// <summary>
        /// Удаляет заметку.
        /// </summary>
        /// <param name="note">Заметка для удаления.</param>
        Task DeleteAsync(Note note);

        /// <summary>
        /// Получает заметки, у которых наступило время напоминания.
        /// </summary>
        /// <param name="currentTime">Текущее время в UTC.</param>
        Task<IEnumerable<Note>> GetNotesDueAsync(DateTime currentTime);
    }
}