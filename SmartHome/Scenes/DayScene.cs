using System.Linq;
using SmartHome.Devices;
using SmartHome.Rooms;

namespace SmartHome.Scenes
{
    public class DayScene : SceneBase
    {
        public DayScene() : base("День") { }

        public override void Execute(System.Collections.Generic.IEnumerable<Room> rooms)
        {
            foreach (var room in rooms)
            {
                foreach (var light in room.GetDevices().OfType<Light>())
                {
                    light.DesiredIsOn = true;
                    light.DesiredBrightness = 100;
                }

                foreach (var heater in room.GetDevices().OfType<Heater>())
                {
                    heater.DesiredTemperature = (int)20.0m;
                }
            }
        }
    }
}
