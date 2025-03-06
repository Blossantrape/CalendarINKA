using CalendarINKA.Abstractions;
using CalendarINKA.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CalendarINKA.Services
{
    /// <summary>
    /// Фоновая служба для проверки напоминаний и отправки уведомлений через SignalR.
    /// </summary>
    public class ReminderService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Конструктор службы с внедрением зависимостей.
        /// </summary>
        /// <param name="hubContext">Контекст хаба SignalR.</param>
        /// <param name="serviceProvider">Провайдер сервисов для создания scope.</param>
        public ReminderService(IHubContext<NotificationHub> hubContext, IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Основной метод фоновой службы, проверяющий напоминания каждую минуту.
        /// </summary>
        /// <param name="stoppingToken">Токен отмены для остановки службы.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var noteService = scope.ServiceProvider.GetRequiredService<INoteService>();
                var dueNotes = await noteService.GetNotesDueAsync(DateTime.UtcNow);
                foreach (var note in dueNotes)
                {
                    await _hubContext.Clients.Group(note.Id.ToString()).SendAsync("ReceiveNotification",
                        new { note.Id, note.Title, note.ReminderAt }, stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}