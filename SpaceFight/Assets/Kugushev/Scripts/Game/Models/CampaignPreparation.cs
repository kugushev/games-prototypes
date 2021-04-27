using System.Collections.Generic;
using Kugushev.Scripts.App.Enums;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Interfaces;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models
{
    public class CampaignPreparation : Poolable<CampaignPreparation.State>
    {
        public readonly struct State
        {
            public State(IPoliticianSelector politicianSelector)
            {
                PoliticianSelector = politicianSelector;
            }

            public IPoliticianSelector PoliticianSelector { get; }
        }

        public CampaignPreparation(ObjectsPool objectsPool) : base(objectsPool)
        {
        }

        private readonly HashSet<Politician> _sponsors = new HashSet<Politician>();
        private readonly HashSet<PerkId> _perksBuffer = new HashSet<PerkId>();

        public IReadOnlyCollection<Politician> Sponsors => _sponsors;

        public int CampaignBudget
        {
            get
            {
                int budget = GameConstants.PlayerCampaignBudget;
                foreach (var sponsor in _sponsors)
                    budget += sponsor.Budget;

                return Mathf.Min(budget, GameConstants.MaxCampaignBudget);
            }
        }

        public bool SelectedPoliticianIsReadyToInvest
        {
            get
            {
                var selectedPolitician = ObjectState.PoliticianSelector.SelectedPolitician;
                return selectedPolitician is { } && selectedPolitician.IsReadyToInvest;
            }
        }

        public bool SelectedPoliticianIsAlreadySponsor
        {
            get
            {
                var selectedPolitician = ObjectState.PoliticianSelector.SelectedPolitician;
                return selectedPolitician is { } && _sponsors.Contains(selectedPolitician);
            }
        }

        public void AddSelectedPoliticianAsSponsor()
        {
            var selectedPolitician = ObjectState.PoliticianSelector.SelectedPolitician;
            if (selectedPolitician is { } && selectedPolitician.IsReadyToInvest)
                _sponsors.Add(selectedPolitician);
        }

        public void RemoveSelectedPoliticianFromSponsors()
        {
            if (SelectedPoliticianIsAlreadySponsor)
                // because we check it in SelectedPoliticianIsAlreadySponsor
                _sponsors.Remove(ObjectState.PoliticianSelector.SelectedPolitician!);
        }

        public void RemoveAllSponsors()
        {
            _sponsors.Clear();
            _perksBuffer.Clear();
        }

        public (int budget, ISet<PerkId> availablePerks) PrepareCampaign()
        {
            var budget = CampaignBudget;

            _perksBuffer.Clear();
            foreach (var sponsor in _sponsors)
            {
                _perksBuffer.Add(sponsor.Character.PerkLvl1.Id);
                sponsor.CollectMoney();
            }

            return (budget, _perksBuffer);
        }

        protected override void OnClear(State state) => _sponsors.Clear();
    }
}