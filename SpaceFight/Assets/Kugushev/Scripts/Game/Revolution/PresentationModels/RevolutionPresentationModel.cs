using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.Common.ContextManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Revolution.PresentationModels
{
    public class RevolutionPresentationModel : MonoBehaviour
    {
        [SerializeField] private Button finishButton = default!;

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private SignalToTransition<GameExitParameters>.Factory _gameExitSignalFactory = default!;

        private void Awake()
        {
            finishButton.onClick.AddListener(FinishGameClick);
        }

        private void FinishGameClick()
        {
            _signalBus.Fire(_gameExitSignalFactory.Create(new GameExitParameters()));
        }
    }
}