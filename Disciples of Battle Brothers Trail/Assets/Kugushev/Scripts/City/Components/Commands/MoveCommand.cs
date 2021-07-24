using Kugushev.Scripts.Game.Enums;

namespace Kugushev.Scripts.City.Components.Commands
{
    public readonly struct MoveCommand
    {
        public MoveCommand(Direction2d direction) => Direction = direction;

        public readonly Direction2d Direction;
    }
}