using System;
using Kugushev.Scripts.Game.Models;
using UnityEngine.Events;

namespace Kugushev.Scripts.Presentation.Events
{
    [Serializable]
    public class PlanetEvent: UnityEvent<Planet>
    {
        
    }
}