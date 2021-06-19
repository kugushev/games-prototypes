namespace Kugushev.Scripts.Game.Core.ValueObjects
{
    public readonly struct DeltaTime
    {
        public float Seconds { get; }

        public DeltaTime(float seconds) => Seconds = seconds;
    }
}