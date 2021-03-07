using System;
using Kugushev.Scripts.Mission.Models;
using Kugushev.Scripts.MissionPresentation.Controllers;
using UnityEngine.Events;

namespace Kugushev.Scripts.MissionPresentation.Events
{
    [Serializable]
    public class PlanetEvent: UnityEvent<HandController, Planet>
    {
        
    }
}