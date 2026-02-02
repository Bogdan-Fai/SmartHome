using System.Collections.Generic;
using System.Linq;
using SmartHome.Rooms;

namespace SmartHome.Controllers
{
    public abstract class ControllerBase
    {
        protected IReadOnlyList<Room> Rooms { get; }

        protected ControllerBase(IEnumerable<Room> rooms)
        {
            Rooms = rooms.ToList().AsReadOnly();
        }

        public abstract void Execute();
    }
}