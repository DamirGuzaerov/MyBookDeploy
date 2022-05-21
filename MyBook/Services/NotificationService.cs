using Microsoft.AspNetCore.SignalR;
using MyBook.Core.Interfaces;
using MyBook.Infrastructure.Hubs;
using MyBook.Entities;
using Repositories;
using System.Security.Claims;

namespace MyBook.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;

        public NotificationService(IHubContext<NotificationHub> notificationHubContext, IHubContext<NotificationUserHub> notificationUserHubContext, IUserConnectionManager userConnectionManager)
        {
            _notificationHubContext = notificationHubContext;
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
        }

        public Task NotifyAll(string header, string content)
        {
            return _notificationHubContext.Clients.All.SendAsync("sendToUser", header, content);
        }

        public async Task NotifyClient(string userId, string header, string content)
        {
            var connections = _userConnectionManager.GetUserConnections(userId);
            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    await _notificationUserHubContext.Clients.Client(connectionId).SendAsync("sendToUser", header, content);
                }
            }
        }
    }
}
