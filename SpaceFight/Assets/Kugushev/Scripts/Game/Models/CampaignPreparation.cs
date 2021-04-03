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
        private readonly List<PerkId> _perksBuffer = new List<PerkId>(9);

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
                return selectedPolitician != null && selectedPolitician.IsReadyToInvest;
            }
        }

        public bool SelectedPoliticianIsAlreadySponsor
        {
            get
            {
                var selectedPolitician = ObjectState.PoliticianSelector.SelectedPolitician;
                return selectedPolitician != null && _sponsors.Contains(selectedPolitician);
            }
        }

        public void AddSelectedPoliticianAsSponsor()
        {
            var selectedPolitician = ObjectState.PoliticianSelector.SelectedPolitician;
            if (selectedPolitician != null && selectedPolitician.IsReadyToInvest)
                _sponsors.Add(selectedPolitician);
        }

        public void RemoveSelectedPoliticianFromSponsors()
        {
            if (SelectedPoliticianIsAlreadySponsor)
                _sponsors.Remove(ObjectState.PoliticianSelector.SelectedPolitician);
        }

        public void RemoveAllSponsors()
        {
            _sponsors.Clear();
            _perksBuffer.Clear();
        }

        public (int budget, IReadOnlyList<PerkId> availablePerks) PrepareCampaign()
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