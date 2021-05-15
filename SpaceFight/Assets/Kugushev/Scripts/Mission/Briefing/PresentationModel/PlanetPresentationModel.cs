using System;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Core.Models;
using Kugushev.Scripts.Mission.Enums;
using TMPro;
using UniRx;
using UnityEngine;

namespace Kugushev.Scripts.Mission.Briefing.PresentationModel
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PlanetPresentationModel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI armyCaption = default!;
        [SerializeField] private Material playerMaterial = default!;
        [SerializeField] private Material neutralMaterial = default!;
        [SerializeField] private Material enemyMaterial = default!;

        private MeshRenderer? _meshRenderer;

        private Planet? _model;
        
        public void Init(Planet model)
        {
            _model = model;

            UpdateView(model);
        }
 

        protected void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void UpdateView(Planet planet)
        {
            UpdateScale(planet.Size);
            planet.Position.Subscribe(position => transform.position = position.Point);
            planet.Faction.Subscribe(UpdateMesh);
            planet.Power.Select(Mathf.CeilToInt).Select(StringBag.FromInt).SubscribeToTextMeshPro(armyCaption);
        }

        private void UpdateScale(PlanetSize size)
        {
            var scale = size switch
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

        private void UpdateMesh(Faction faction)
        {
            if (_meshRenderer is null)
                return;

            _meshRenderer.material = faction switch
            {
                Faction.Green => playerMaterial,
                Faction.Neutral => neutralMaterial,
                Faction.Red => enemyMaterial,
                _ => null
            };
        }
    }
}