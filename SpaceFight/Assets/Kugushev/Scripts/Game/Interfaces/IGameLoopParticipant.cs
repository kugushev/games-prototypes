namespace Kugushev.Scripts.Game.Interfaces
{
    public interface IGameLoopParticipant
    {
        void NextStep(float deltaTime);
    }
}