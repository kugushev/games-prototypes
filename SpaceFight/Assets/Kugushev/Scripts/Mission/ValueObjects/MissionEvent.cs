using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects
{
    public readonly struct MissionEvent
    {
        public MissionEvent(MissionEventType eventType, Faction active, Faction? passive, int? intPayload)
        {
            EventType = eventType;
            Active = active;
            Passive = passive;
            IntPayload = intPayload;
        }

        public readonly MissionEventType EventType;
        public readonly Faction Active;
        public readonly Faction? Passive;
        public readonly int? IntPayload;
    }
}