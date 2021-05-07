using Kugushev.Scripts.Game.Core.Models;

namespace Kugushev.Scripts.Game.Politics.Interfaces
{
    public interface IParliamentPresentationModel
    {
        void SelectPolitician(IPolitician? politician);
    }
}