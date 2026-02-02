using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Devices;
using SmartHome.Events;

namespace SmartHome.Rooms
{
    public class Room : IEventListener
    {
        private readonly List<Device> _devices = new();
        public string Name { get; }

        public Room(string name)
        {
            Name = name;
            EventBus.Instance.Subscribe("MotionDetected", this);
        }

        public void AddDevice(Device device)
        {
            _devices.Add(device);
        }

        public bool RemoveDevice(string id)
        {
            var device = _devices.Find(d => d.Id == id);
            if (device != null)
            {
                _devices.Remove(device);
                return true;
            }
            return false;
        }

        public IReadOnlyList<Device> GetDevices()
        {
            return _devices.AsReadOnly();
        }

        public void TurnAllOn()
        {
            Console.WriteLine($"\n🔌 Включение всех устройств в комнате {Name}:");
            
            foreach (var device in _devices)
            {
                if (device is IControllable controllable)
                {
                    controllable.TurnOn();
                    Console.WriteLine($"   ✅ {device.Name} включен через IControllable");
                }
                else
                {
                    // Для устройств без IControllable используем альтернативные методы
                    if (device is Light light)
                    {
                        light.DesiredIsOn = true;
                        light.DesiredBrightness = 80;
                        light.ApplyAutomation();
                        Console.WriteLine($"   💡 {light.Name} включен (яркость: 80%)");
                    }
                    else if (device is Heater heater)
                    {
                        heater.TurnOn();
                        Console.WriteLine($"   🔥 {heater.Name} включен");
                    }
                    else if (device is Thermostat thermostat)
                    {
                        // Термостат обычно не "включается", но можно установить нормальную температуру
                        thermostat.CurrentTemperature = 22.0m;
                        Console.WriteLine($"   🌡️ {thermostat.Name} установлена температура: 22°C");
                    }
                }
            }
            
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] All devices turned on in room: {Name}";
            System.IO.File.AppendAllText("history.log", logEntry + Environment.NewLine);
        }

        public void OnEvent(string eventName, object? payload)
        {
            if (eventName == "MotionDetected" && payload is string roomName && roomName == Name)
            {
                // При движении включаем все светы в комнате
                foreach (var light in _devices.OfType<Light>())
                {
                    // Устанавливаем желаемое состояние: включено с яркостью 70%
                    light.DesiredIsOn = true;
                    light.DesiredBrightness = 70;
                    // Мгновенно применяем автоматизацию
                    light.ApplyAutomation();
                }

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Motion detected in {Name}, all lights turned on";
                System.IO.File.AppendAllText("history.log", logEntry + Environment.NewLine);
                
                Console.WriteLine($"🔦 Свет включен в комнате {Name} из-за обнаружения движения");
            }
        }

        public void DisplayStatus()
        {
            Console.WriteLine($"\n=== {Name} ===");

            var thermostat = _devices.OfType<Thermostat>().FirstOrDefault();
            var heater = _devices.OfType<Heater>().FirstOrDefault();

            if (thermostat != null)
            {
                Console.WriteLine($"  🌡️ {thermostat.GetStatus()}");
            }

            if (heater != null && thermostat != null)
            {
                Console.WriteLine($"  🔥 {heater.GetStatus()} - {heater.GetHeaterStatus((int)thermostat.CurrentTemperature)}");
            }

            foreach (var light in _devices.OfType<Light>())
            {
                Console.WriteLine($"  💡 {light.GetStatus()}");
            }
        }
    }
}