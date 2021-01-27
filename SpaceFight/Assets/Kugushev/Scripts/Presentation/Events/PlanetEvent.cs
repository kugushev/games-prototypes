﻿using System;
using Kugushev.Scripts.Game.Models;
using Kugushev.Scripts.Presentation.Controllers;
using UnityEngine.Events;

namespace Kugushev.Scripts.Presentation.Events
{
    [Serializable]
    public class PlanetEvent: UnityEvent<HandController, Planet>
    {
        
    }
}