using System;
using System.Collections.Generic;
using Kugushev.Scripts.Common.Utils;
using Kugushev.Scripts.Mission.Enums;
using Kugushev.Scripts.Mission.Models;
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
                PlanetSize.Mars => new Vector3(0.06f, 0.06f, 0.06f),
                PlanetSize.Earth => new Vector3(0.07f, 0.07f, 0.07f),
                PlanetSize.Uranus => new Vector3(0.09f, 0.09f, 0.09f),
                PlanetSize.Saturn => new Vector3(0.11f, 0.11f, 0.11f),
                PlanetSize.Jupiter => new Vector3(0.13f, 0.13f, 0.13f),
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