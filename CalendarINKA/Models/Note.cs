using System.ComponentModel.DataAnnotations;

namespace CalendarINKA.Models
{
    /// <summary>
    /// Модель заметки в календаре.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Уникальный идентификатор заметки (GUID).
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Заголовок заметки (обязательное поле).
        /// </summary>
        [Required]
        public required string Title { get; set; }

        /// <summary>
        /// Текст заметки (опционально).
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Дата и время создания заметки (в UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата и время напоминания (обязательное поле, в UTC).
        /// </summary>
        [Required]
        public DateTime ReminderAt { get; set; }
    }
}