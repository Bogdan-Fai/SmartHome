using System.Linq;
using SmartHome.Devices;
using SmartHome.Rooms;

namespace SmartHome.Scenes
{
    public class NightScene : SceneBase
    {
        public NightScene() : base("Ночь") { }

        public override void Execute(System.Collections.Generic.IEnumerable<Room> rooms)
        {
            foreach (var room in rooms)
            {
                foreach (var light in room.GetDevices().OfType<Light>())
                {
                    light.DesiredIsOn = false;
                }

                foreach (var heater in room.GetDevices().OfType<Heater>())
                {
                    heater.DesiredTemperature = (int)18.0m;
                }
            }
        }
    }
}
