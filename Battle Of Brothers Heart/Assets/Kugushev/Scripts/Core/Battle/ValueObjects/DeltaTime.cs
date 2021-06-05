namespace Kugushev.Scripts.Core.Battle.ValueObjects
{
    public readonly struct DeltaTime
    {
        public float Seconds { get; }

        public DeltaTime(float seconds) => Seconds = seconds;
    }
}