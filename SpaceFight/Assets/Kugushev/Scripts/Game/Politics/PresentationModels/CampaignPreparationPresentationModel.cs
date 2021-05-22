using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Kugushev.Scripts.App.Core.ContextManagement.Parameters;
using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.ContextManagement;
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
        [SerializeField] private Button startCampaignButton = default!;

        [Inject] private IPoliticianSelector _politicianSelector = default!;

        [Inject] private SignalBus _signalBus = default!;
        [Inject] private SignalToTransition<CampaignParameters>.Factory _startCampaignFactory = default!;

        private readonly ReactiveCollection<IPolitician> _sponsors = new ReactiveCollection<IPolitician>();

        private readonly StringBuilder _sponsorsListBuilder = new StringBuilder();

        private void Awake()
        {
            _sponsors.ObserveCountChanged()
                .Select(_ => CalculateSponsorsBudget())
                .Select(StringBag.FromInt)
                .SubscribeToTextMeshPro(budgetSponsors)
                .AddTo(this);

            _sponsors.ObserveCountChanged()
                .Select(_ => GetSponsorsList())
                .SubscribeToTextMeshPro(sponsorsList)
                .AddTo(this);

            _politicianSelector.SelectedPolitician
                .CombineLatest(_sponsors.ObserveCountChanged(), (selected, _) => selected)
                .Subscribe(UpdateSponsorsButtons)
                .AddTo(this);

            startCampaignButton.onClick.AddListener(OnStartCampaignClick);
        }

        private void OnStartCampaignClick()
        {
            var budget = GameConstants.PlayerCampaignBudget + CalculateSponsorsBudget();

            // todo: pooling
            var perks = new HashSet<PerkId>(_sponsors.Select(s => s.Character.PerkLvl1.Id));

            var parameters = new CampaignParameters(
                Random.Range(0, 100), // for test purposes
                budget,
                perks,
                false,
                false
            );

            _signalBus.Fire(_startCampaignFactory.Create(parameters));
        }

        private int CalculateSponsorsBudget()
        {
            int budget = 0;
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