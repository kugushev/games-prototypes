using Kugushev.Scripts.Common.FiniteStateMachine;
using Kugushev.Scripts.Game.Models;

namespace Kugushev.Scripts.Game.StatesAndTransitions
{
    internal class ToCampaignTransition: ITransition
    {
        private readonly MainMenu _mainMenu;

        public ToCampaignTransition(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
        }
        
        public bool ToTransition => _mainMenu.StartClicked;
    }
}