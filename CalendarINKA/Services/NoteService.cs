using CalendarINKA.Abstractions;
using CalendarINKA.Models;

namespace CalendarINKA.Services
{
    /// <summary>
    /// Сервис для управления бизнес-логикой заметок.
    /// </summary>
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        /// <summary>
        /// Конструктор сервиса с внедрением репозитория.
        /// </summary>
        /// <param name="noteRepository">Репозиторий заметок.</param>
        public NoteService(INoteRepository noteRepository) => _noteRepository = noteRepository;

        /// <summary>
        /// Получает все заметки из хранилища.
        /// </summary>
        /// <returns>Список заметок.</returns>
        public async Task<IEnumerable<Note>> GetAllNotesAsync() => await _noteRepository.GetAllAsync();

        /// <summary>
        /// Получает заметку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        /// <returns>Заметка или null, если не найдена.</returns>
        public async Task<Note?> GetNoteByIdAsync(Guid id) => await _noteRepository.GetByIdAsync(id);

        /// <summary>
        /// Создает новую заметку, устанавливая время создания и конвертируя ReminderAt в UTC.
        /// </summary>
        /// <param name="note">Данные новой заметки.</param>
        public async Task CreateNoteAsync(Note note)
        {
            note.CreatedAt = DateTime.UtcNow;
            if (note.ReminderAt.Kind != DateTimeKind.Utc)
                note.ReminderAt = note.ReminderAt.ToUniversalTime();
            await _noteRepository.CreateAsync(note);
        }

        /// <summary>
        /// Обновляет существующую заметку.
        /// </summary>
        /// <param name="note">Обновленные данные заметки.</param>
        public async Task UpdateNoteAsync(Note note) => await _noteRepository.UpdateAsync(note);

        /// <summary>
        /// Удаляет заметку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        public async Task DeleteNoteAsync(Guid id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note != null) await _noteRepository.DeleteAsync(note);
        }

        /// <summary>
        /// Получает заметки, у которых наступило время напоминания.
        /// </summary>
        /// <param name="currentTime">Текущее время в UTC.</param>
        /// <returns>Список просроченных заметок.</returns>
        public async Task<IEnumerable<Note>> GetNotesDueAsync(DateTime currentTime)
        {
            return await _noteRepository.GetNotesDueAsync(currentTime);
        }
    }
}