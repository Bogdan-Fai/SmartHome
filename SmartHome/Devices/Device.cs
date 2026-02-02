using System;
using System.IO;

namespace SmartHome.Devices
{
    public abstract class Device
    {
        private string _id;
        private string _name;
        private bool _isOn;

        public string Id => _id;
        public string Name => _name;
        public bool IsOn => _isOn;
        public abstract string Type { get; }

        protected Device(string name)
        {
            _id = Guid.NewGuid().ToString();
            _name = name ?? throw new ArgumentException("Device name cannot be null");
            _isOn = false;
        }

        protected void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText("history.log", logEntry + Environment.NewLine);
        }

        public virtual void TurnOn()
        {
            _isOn = true;
            Log($"{Name} turned on");
        }

        public virtual void TurnOff()
        {
            _isOn = false;
            Log($"{Name} turned off");
        }

        public virtual string GetStatus()
        {
            return $"{Name} ({Type}): {(_isOn ? "On" : "Off")}";
        }
    }

    public interface IControllable
    {
        void TurnOn();
        void TurnOff();
    }
}