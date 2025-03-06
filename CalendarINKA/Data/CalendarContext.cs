using CalendarINKA.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarINKA.Data
{
    /// <summary>
    /// Контекст базы данных для работы с заметками через Entity Framework.
    /// </summary>
    public class CalendarContext : DbContext
    {
        /// <summary>
        /// Конструктор контекста с внедрением опций.
        /// </summary>
        /// <param name="options">Опции конфигурации контекста.</param>
        public CalendarContext(DbContextOptions<CalendarContext> options) : base(options)
        {
        }

        /// <summary>
        /// Набор данных заметок в базе данных.
        /// </summary>
        public DbSet<Note> Notes { get; set; }
    }
}