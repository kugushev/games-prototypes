using System;
using Kugushev.Scripts.MissionPresentation.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Kugushev.Scripts.MissionPresentation.Events
{
    [Serializable]
    public class MovingEvent: UnityEvent<HandController, Vector3>
    {
        
    }
}