using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects
{
    public readonly struct MissionEvent
    {
        public MissionEvent(MissionEventType eventType, Faction active, Faction? passive = null, 
            float? floatPayload1 = null, float? floatPayload2 = null)
        {
            EventType = eventType;
            Active = active;
            Passive = passive;
            FloatPayload1 = floatPayload1;
            FloatPayload2 = floatPayload2;
        }

        public readonly MissionEventType EventType;
        public readonly Faction Active;
        public readonly Faction? Passive;
        public readonly float? FloatPayload1;
        public readonly float? FloatPayload2;
    }
}