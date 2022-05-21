using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Core.Interfaces
{
    public interface INotificationService
    {
        public Task NotifyAll(string header, string content);
        public Task NotifyClient(string userId, string header, string content);
    }
}
