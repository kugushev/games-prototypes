namespace Kugushev.Scripts.Common.Interfaces
{
    public interface IGameLoopParticipant
    {
        void NextStep(float deltaTime);
    }
}