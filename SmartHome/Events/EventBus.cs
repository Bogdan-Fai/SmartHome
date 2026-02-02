using System;
using System.Collections.Generic;
using System.IO;

namespace SmartHome.Events
{
    public class EventBus
    {
        private readonly Dictionary<string, List<IEventListener>> _subscribers = new();
        private static EventBus? _instance;

        public static EventBus Instance => _instance ??= new EventBus();

        private EventBus() { }

        public void Subscribe(string eventName, IEventListener listener)
        {
            if (!_subscribers.ContainsKey(eventName))
            {
                _subscribers[eventName] = new List<IEventListener>();
            }
            _subscribers[eventName].Add(listener);
        }

        public void Publish(string eventName, object? payload = null)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Event published: {eventName}";
            if (payload != null)
            {
                logEntry += $", Payload: {payload}";
            }
            
            // Логируем событие ДО обработки
            File.AppendAllText("history.log", logEntry + Environment.NewLine);

            // Обрабатываем подписчиков
            if (_subscribers.ContainsKey(eventName))
            {
                foreach (var listener in _subscribers[eventName])
                {
                    listener.OnEvent(eventName, payload);
                }
            }
        }
    }
}