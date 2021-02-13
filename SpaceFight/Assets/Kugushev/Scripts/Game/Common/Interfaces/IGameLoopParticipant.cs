namespace Kugushev.Scripts.Game.Common.Interfaces
{
    public interface IGameLoopParticipant
    {
        void NextStep(float deltaTime);
    }
}