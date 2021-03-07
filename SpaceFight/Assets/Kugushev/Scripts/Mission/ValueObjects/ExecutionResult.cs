using Kugushev.Scripts.Mission.Enums;

namespace Kugushev.Scripts.Mission.ValueObjects
{
    public readonly struct ExecutionResult
    {
        public ExecutionResult(Faction winner)
        {
            Winner = winner;
        }

        public Faction Winner { get; }
    }
}