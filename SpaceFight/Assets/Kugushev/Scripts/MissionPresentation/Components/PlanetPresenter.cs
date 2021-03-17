using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.MissionPresentation.Components.Abstractions;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.Components
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PlanetPresenter : BaseComponent<Planet>
    {
        [SerializeField] private TextMeshProUGUI armyCaption;
        [SerializeField] private Material playerMaterial;
        [SerializeField] private Material neutralMaterial;
        [SerializeField] private Material enemyMaterial;

        private MeshRenderer _meshRenderer;

        protected override void OnAwake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        protected override void OnStart()
        {
            var scale = Model.Size switch
            {
                PlanetSize.Mercury => 0.05f,
                PlanetSize.Mars => 0.06f,
                PlanetSize.Earth => 0.07f,
                PlanetSize.Uranus => 0.09f,
                PlanetSize.Saturn => 0.10f,
                PlanetSize.Jupiter => 0.11f,
                _ => 0.05f
            };

            transform.localScale = new Vector3(scale, scale, scale);
        }

        private void Update()
        {
            // todo: use UniRx to track changes

            transform.position = Model.Position.Point;

            _meshRenderer.material = Model.Faction switch
            {
                Faction.Green => playerMaterial,
                Faction.Neutral => neutralMaterial,
                Faction.Red => enemyMaterial,
                _ => null
            };

            armyCaption.text = StringBag.FromInt(Mathf.CeilToInt(Model.Power));
        }
    }
}