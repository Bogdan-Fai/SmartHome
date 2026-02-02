using System;
using System.IO;
using System.Linq;
using SmartHome.Devices;
using SmartHome.Rooms;

namespace SmartHome.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IEnumerable<Room> rooms) : base(rooms) { }

        public override void Execute()
        {
            ApplyAutomation();
        }

        public void ApplyAutomation()
        {
            foreach (var room in Rooms)
            {
                // Автоматизация света
                foreach (var light in room.GetDevices().OfType<Light>())
                {
                    light.ApplyAutomation();
                }

                // Автоматизация температуры
                var thermostat = room.GetDevices().OfType<Thermostat>().FirstOrDefault();
                var heater = room.GetDevices().OfType<Heater>().FirstOrDefault();

                if (thermostat != null && heater != null)
                {
                    // Мгновенное изменение текущей температуры на желаемую
                    thermostat.CurrentTemperature = heater.DesiredTemperature;
                }
            }

            Log("Автоматизация применена ко всем устройствам");
        }

        private void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText("history.log", logEntry + Environment.NewLine);
        }
    }
}