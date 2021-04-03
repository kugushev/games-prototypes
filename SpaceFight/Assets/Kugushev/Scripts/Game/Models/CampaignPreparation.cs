using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils.Pooling;
using Kugushev.Scripts.Game.Constants;
using Kugushev.Scripts.Game.Interfaces;

namespace Kugushev.Scripts.Game.Models
{
    public class CampaignPreparation : Poolable<CampaignPreparation.State>
    {
        public struct State
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

        public IReadOnlyCollection<Politician> Sponsors => _sponsors;

        public int CampaignBudget
        {
            get
            {
                int budget = GameConstants.PlayerCampaignBudget;
                foreach (var sponsor in _sponsors)
                    budget += sponsor.Budget;
                return budget;
            }
        }

        public bool SelectedPoliticianIsReadyToInvest => ObjectState.PoliticianSelector.SelectedPolitician != null &&
                                       ObjectState.PoliticianSelector.SelectedPolitician.IsReadyToInvest;

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

        public void RemoveAllSponsors() => _sponsors.Clear();

        protected override void OnClear(State state) => _sponsors.Clear();
    }
}