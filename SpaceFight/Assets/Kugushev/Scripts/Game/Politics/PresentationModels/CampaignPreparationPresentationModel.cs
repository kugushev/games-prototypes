using System.Diagnostics.CodeAnalysis;
using System.Text;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Politics.Interfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kugushev.Scripts.Game.Politics.PresentationModels
{
    public class CampaignPreparationPresentationModel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI budgetSponsors = default!;
        [SerializeField] private Button addSponsorButton = default!;
        [SerializeField] private Button removeSponsorButton = default!;
        [SerializeField] private TextMeshProUGUI sponsorsList = default!;

        [Inject] private IPoliticianSelector _politicianSelector = default!;

        private readonly ReactiveCollection<IPolitician> _sponsors = new ReactiveCollection<IPolitician>();

        private readonly StringBuilder _sponsorsListBuilder = new StringBuilder();

        private void Awake()
        {
            _sponsors.ObserveCountChanged()
                .Select(_ => CalculateSponsorsBudget())
                .Select(StringBag.FromInt)
                .SubscribeToTextMeshPro(budgetSponsors);

            _sponsors.ObserveCountChanged()
                .Select(_ => GetSponsorsList())
                .SubscribeToTextMeshPro(sponsorsList);

            _politicianSelector.SelectedPolitician
                .CombineLatest(_sponsors.ObserveCountChanged(), (selected, _) => selected)
                .Subscribe(UpdateSponsorsButtons);
        }

        private int CalculateSponsorsBudget()
        {
            int budget = GameConstants.PlayerCampaignBudget;
            foreach (var sponsor in _sponsors)
                budget += sponsor.Budget.Value;

            return Mathf.Min(budget, GameConstants.MaxCampaignBudget);
        }

        private string GetSponsorsList()
        {
            // todo: pool this data to avoid allocations
            _sponsorsListBuilder.Clear();
            foreach (var sponsor in _sponsors)
                _sponsorsListBuilder.AppendLine(sponsor.Character.FullName);
            return _sponsorsListBuilder.ToString();
        }

        private void UpdateSponsorsButtons(IPolitician? selectedPolitician)
        {
            var selectedIsSponsor = PoliticianIsSponsor(selectedPolitician);
            addSponsorButton.interactable = !selectedIsSponsor && selectedPolitician?.IsReadyToInvest.Value == true;
            removeSponsorButton.interactable = selectedIsSponsor;
        }

        private bool PoliticianIsSponsor([NotNullWhen(true)] IPolitician? selectedPolitician) =>
            selectedPolitician is { } && _sponsors.Contains(selectedPolitician);

        public void AddSelectedPolitician()
        {
            var selectedPolitician = _politicianSelector.SelectedPolitician.Value;
            if (selectedPolitician != null && selectedPolitician.IsReadyToInvest.Value)
                _sponsors.Add(selectedPolitician);
        }

        public void RemoveSelectedPolitician()
        {
            var selectedPolitician = _politicianSelector.SelectedPolitician.Value;
            if (PoliticianIsSponsor(selectedPolitician))
                _sponsors.Remove(selectedPolitician);
        }
    }
}