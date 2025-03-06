using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalendarINKA.Abstractions;
using CalendarINKA.Data;
using CalendarINKA.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarINKA.Repositories
{
    /// <summary>
    /// Репозиторий для работы с данными заметок в базе данных.
    /// </summary>
    public class NoteRepository : INoteRepository
    {
        private readonly CalendarContext _context;

        /// <summary>
        /// Конструктор репозитория с внедрением контекста базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public NoteRepository(CalendarContext context) => _context = context;

        /// <summary>
        /// Получает все заметки из базы данных.
        /// </summary>
        /// <returns>Список всех заметок.</returns>
        public async Task<IEnumerable<Note>> GetAllAsync() => await _context.Notes.ToListAsync();

        /// <summary>
        /// Получает заметку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор заметки.</param>
        /// <returns>Заметка или null, если не найдена.</returns>
        public async Task<Note?> GetByIdAsync(Guid id) => await _context.Notes.FindAsync(id);

        /// <summary>
        /// Создает новую заметку в базе данных.
        /// </summary>
        /// <param name="note">Данные новой заметки.</param>
        public async Task CreateAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет существующую заметку в базе данных.
        /// </summary>
        /// <param name="note">Обновленные данные заметки.</param>
        public async Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет заметку из базы данных.
        /// </summary>
        /// <param name="note">Заметка для удаления.</param>
        public async Task DeleteAsync(Note note)
        {
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получает заметки, у которых наступило время напоминания.
        /// </summary>
        /// <param name="currentTime">Текущее время в UTC.</param>
        /// <returns>Список просроченных заметок.</returns>
        public async Task<IEnumerable<Note>> GetNotesDueAsync(DateTime currentTime)
        {
            return await _context.Notes
                .Where(n => n.ReminderAt <= currentTime && n.ReminderAt >= currentTime.AddMinutes(-1))
                .ToListAsync();
        }
    }
}