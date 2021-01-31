using System;
using System.Collections.Generic;
using Kugushev.Scripts.Game.Enums;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Presentation.Components.Abstractions;
using TMPro;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Components
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PlanetPresenter: BaseComponent<Planet>
    {
        [SerializeField] private TextMeshProUGUI armyCaption;
        [SerializeField] private Material playerMaterial;
        [SerializeField] private Material neutralMaterial;
        [SerializeField] private Material enemyMaterial;

        private MeshRenderer _meshRenderer;
        private int currentArmy = 0;
        private static readonly Dictionary<int, string> ArmyStringBag = new Dictionary<int, string>(64); 

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
                Faction.Player => playerMaterial,
                Faction.Neutral => neutralMaterial,
                Faction.Enemy => enemyMaterial,
                _ => null
            };

            var power = Model.Power;
            if (currentArmy != power)
            {
                if (!ArmyStringBag.TryGetValue(power, out var armyCaptionText))
                    ArmyStringBag[power] = armyCaptionText = power.ToString();

                armyCaption.text = armyCaptionText;

                currentArmy = power;
            }

            if (Model.Selected)
            {
                // todo: highlight the planet
            }
        }
    }
}