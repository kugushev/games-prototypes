using Kugushev.Scripts.Game.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Game.Widgets
{
    public class PoliticsWidget : MonoBehaviour
    {
        [SerializeField] private GameModelProvider gameModelProvider;

        [SerializeField] private ParliamentWidget parliament;
        [SerializeField] private PoliticalActionsWidget politicalActions;

        private void Start()
        {
            if (gameModelProvider.TryGetModel(out var model))
            {
                parliament.Setup(model.Parliament);
                politicalActions.Setup(model.PoliticalActions, model.Parliament);
            }
            else
                Debug.LogError("Unable to get model");
        }

        public void UpdateView()
        {
            parliament.UpdateView();
        }
    }
}