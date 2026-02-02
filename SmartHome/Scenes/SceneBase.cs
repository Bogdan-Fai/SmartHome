using System.Collections.Generic;
using SmartHome.Rooms;

namespace SmartHome.Scenes
{
    public abstract class SceneBase
    {
        public string Name { get; }
        public abstract void Execute(IEnumerable<Room> rooms);

        protected SceneBase(string name)
        {
            Name = name;
        }
    }
}