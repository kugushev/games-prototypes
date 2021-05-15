using Kugushev.Scripts.Campaign.Core.Models;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Campaign.MissionSelection.PresentationModels
{
    public class PerksPanelPresentationModel : MonoBehaviour
    {
        [SerializeField] private Transform achievementsPanel = default!;
        [SerializeField] private GameObject achievementCardPrefab = default!;

        [Inject] private IPlayerPerks _model = default!;

        private void Start()
        {
            foreach (var perk in _model.CommonPerks)
                CreatePerkCard(perk);

            foreach (var perk in _model.EpicPerks.Values)
                CreatePerkCard(perk);
        }

        private void CreatePerkCard(PerkInfo perk)
        {
            Asserting.NotNull(achievementCardPrefab, achievementsPanel);

            var go = Instantiate(achievementCardPrefab, achievementsPanel);
            var widget = go.GetComponent<PerksCardPresentationModel>();
            widget.Init(perk);
        }
    }
}