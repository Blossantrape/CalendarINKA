using Microsoft.AspNetCore.SignalR;

namespace CalendarINKA.Hubs
{
    /// <summary>
    /// Хаб SignalR для отправки push-уведомлений в браузер.
    /// </summary>
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Добавляет клиента в группу по идентификатору пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        public async Task JoinGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
    }
}