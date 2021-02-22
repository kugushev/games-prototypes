using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Enums;
using Kugushev.Scripts.Game.Missions.Presets;
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

        protected override void OnStart()
        {
            transform.localScale = Model.Size switch
            {
                PlanetSize.Mercury => new Vector3(0.05f, 0.05f, 0.05f),
                PlanetSize.Mars => new Vector3(0.08f, 0.08f, 0.08f),
                PlanetSize.Earth => new Vector3(0.11f, 0.11f, 0.11f),
                PlanetSize.Uranus => new Vector3(0.14f, 0.14f, 0.14f),
                PlanetSize.Saturn => new Vector3(0.17f, 0.17f, 0.17f),
                PlanetSize.Jupiter => new Vector3(0.2f, 0.2f, 0.2f),
                _ => Vector3.one
            };
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

            armyCaption.text = StringBag.FromInt(Model.Power);
        }
    }
}