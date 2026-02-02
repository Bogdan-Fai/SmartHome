using System.Linq;
using SmartHome.Devices;
using SmartHome.Rooms;

namespace SmartHome.Scenes
{
    public class MorningScene : SceneBase
    {
        public MorningScene() : base("Утро") { }

        public override void Execute(System.Collections.Generic.IEnumerable<Room> rooms)
        {
            foreach (var room in rooms)
            {
                foreach (var light in room.GetDevices().OfType<Light>())
                {
                    light.DesiredIsOn = true;
                    light.DesiredBrightness = 50;
                }

                foreach (var heater in room.GetDevices().OfType<Heater>())
                {
                    heater.DesiredTemperature = (int)22.0m;
                }
            }
        }
    }
}
