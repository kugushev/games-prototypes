using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.Entities;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Presentation.Common.Utils;
using Kugushev.Scripts.Presentation.Components.Abstractions;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Components
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

        private void Start()
        {
            transform.localScale = Model.Size switch
            {
                PlanetSize.Mercury => new Vector3(0.05f, 0.05f, 0.05f),
                PlanetSize.Mars => new Vector3(0.1f, 0.1f, 0.1f),
                PlanetSize.Earth => new Vector3(0.15f, 0.15f, 0.15f),
                PlanetSize.Uranus => new Vector3(0.2f, 0.2f, 0.2f),
                PlanetSize.Saturn => new Vector3(0.25f, 0.25f, 0.25f),
                PlanetSize.Jupiter => new Vector3(0.3f, 0.3f, 0.3f),
                _ => Vector3.one
            };
        }

        private void Update()
        {
            // todo: use UniRx to track changes

            _meshRenderer.material = Model.Faction switch
            {
                Faction.Green => playerMaterial,
                Faction.Neutral => neutralMaterial,
                Faction.Red => enemyMaterial,
                _ => null
            };

            armyCaption.text = StringBag.FromInt(Model.Power);
        }
    }
}