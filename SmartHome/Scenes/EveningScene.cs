using System.Linq;
using SmartHome.Devices;
using SmartHome.Rooms;

namespace SmartHome.Scenes
{
    public class EveningScene : SceneBase
    {
        public EveningScene() : base("Вечер") { }

        public override void Execute(System.Collections.Generic.IEnumerable<Room> rooms)
        {
            foreach (var room in rooms)
            {
                foreach (var light in room.GetDevices().OfType<Light>())
                {
                    light.DesiredIsOn = true;
                    light.DesiredBrightness = 70;
                }

                foreach (var heater in room.GetDevices().OfType<Heater>())
                {
                    heater.DesiredTemperature = (int)24.0m;
                }
            }
        }
    }
}
