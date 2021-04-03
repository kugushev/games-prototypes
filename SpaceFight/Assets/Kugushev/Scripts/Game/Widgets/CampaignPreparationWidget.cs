using System.Text;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Game.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kugushev.Scripts.Game.Widgets
{
    public class CampaignPreparationWidget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI budgetSponsors;
        [SerializeField] private Button addSponsorButton;
        [SerializeField] private Button removeSponsorButton;
        [SerializeField] private TextMeshProUGUI sponsorsList;

        private CampaignPreparation _model;
        private readonly StringBuilder _sponsorsListBuilder = new StringBuilder();

        public void SetUp(CampaignPreparation model)
        {
            _model = model;
        }

        public void UpdateView()
        {
            if (_model == null || !_model.Active)
            {
                Debug.LogError($"Model is not valid {_model}");
                return;
            }

            budgetSponsors.text = StringBag.FromInt(_model.CampaignBudget);

            UpdateSponsorsView();
        }

        private void UpdateSponsorsView()
        {
            // todo: pool this data to avoid allocations
            _sponsorsListBuilder.Clear();
            foreach (var sponsor in _model.Sponsors)
                _sponsorsListBuilder.AppendLine(sponsor.Character.FullName);
            sponsorsList.text = _sponsorsListBuilder.ToString();

            var selectedIsSponsor = _model.SelectedPoliticianIsAlreadySponsor;
            addSponsorButton.interactable = _model.SelectedPoliticianIsReadyToInvest && !selectedIsSponsor;
            removeSponsorButton.interactable = selectedIsSponsor;
        }

        public void AddSelectedPolitician()
        {
            _model?.AddSelectedPoliticianAsSponsor();
            UpdateSponsorsView();
        }

        public void RemoveSelectedPolitician()
        {
            _model?.RemoveSelectedPoliticianFromSponsors();
            UpdateSponsorsView();
        }
    }
}