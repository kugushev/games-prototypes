namespace Kugushev.Scripts.Battle.Core.ValueObjects
{
    public readonly struct DeltaTime
    {
        public float Seconds { get; }

        public DeltaTime(float seconds) => Seconds = seconds;
    }
}