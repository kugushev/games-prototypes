﻿using Kugushev.Scripts.Game.Common.Entities.Abstractions;
using Kugushev.Scripts.Game.Missions.Entities;
using Kugushev.Scripts.Game.Missions.Presets;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PresentationModels
{
    public class PlanetPresentationModel: BasePresentationModel
    {
        [SerializeField] private PlanetPreset model;
        protected override Model Model => model;

        public PlanetPreset Planet
        {
            get => model;
            set
            {
                if (!ReferenceEquals(model, null) && !model.IsStub)
                {
                    Debug.LogError($"Unable to set planet {value} because {model} is already set");
                    return;
                }
                
                model = value;
            }
        }
    }
}