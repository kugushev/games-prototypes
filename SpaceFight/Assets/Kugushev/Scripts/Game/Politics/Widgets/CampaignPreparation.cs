using System.Collections.Generic;
using Kugushev.Scripts.App.Core.Enums;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Core.Constants;
using Kugushev.Scripts.Game.Core.Models;
using Kugushev.Scripts.Game.Politics.Interfaces;
using UnityEngine;

namespace Kugushev.Scripts.Game.Politics.Widgets
{
    internal class CampaignPreparation : Poolable<CampaignPreparation.State>
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

        private readonly HashSet<IPolitician> _sponsors = new HashSet<IPolitician>();
        private readonly HashSet<PerkId> _perksBuffer = new HashSet<PerkId>();

        internal IReadOnlyCollection<IPolitician> Sponsors => _sponsors;

        public int CampaignBudget
        {
            get
            {
                int budget = GameConstants.PlayerCampaignBudget;
                foreach (var sponsor in _sponsors)
                    budget += sponsor.Budget.Value;

                return Mathf.Min(budget, GameConstants.MaxCampaignBudget);
            }
        }

        public bool SelectedPoliticianIsReadyToInvest
        {
            get
            {
                var selectedPolitician = ObjectState.PoliticianSelector.SelectedPolitician;
                return selectedPolitician is { } && selectedPolitician.Value.IsReadyToInvest.Value;
            }
        }

        public bool SelectedPoliticianIsAlreadySponsor
        {
            get
            {
                var selectedPolitician = ObjectState.PoliticianSelector.SelectedPolitician;
                return selectedPolitician is { } && _sponsors.Contains(selectedPolitician.Value);
            }
        }

        public void AddSelectedPoliticianAsSponsor()
        {
            var selectedPolitician = ObjectState.PoliticianSelector.SelectedPolitician;
            if (selectedPolitician is { } && selectedPolitician.Value.IsReadyToInvest.Value)
                _sponsors.Add(selectedPolitician.Value);
        }

        public void RemoveSelectedPoliticianFromSponsors()
        {
            if (SelectedPoliticianIsAlreadySponsor)
                // because we check it in SelectedPoliticianIsAlreadySponsor
                _sponsors.Remove(ObjectState.PoliticianSelector.SelectedPolitician.Value);
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

                // todo: send signal
                // sponsor.CollectMoney();
            }

            return (budget, _perksBuffer);
        }

        protected override void OnClear(State state) => _sponsors.Clear();
    }
}