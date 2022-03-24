using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Services
{
    public interface ILocalNotificationService
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, DateTime notifyTime);
        void ReceiveNotification(string title, string message);
    }
}
