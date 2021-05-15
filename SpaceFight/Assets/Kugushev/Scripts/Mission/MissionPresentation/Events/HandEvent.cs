using System;
using Kugushev.Scripts.MissionPresentation.Controllers;
using UnityEngine.Events;

namespace Kugushev.Scripts.MissionPresentation.Events
{
    [Serializable]
    public class HandEvent: UnityEvent<HandController>
    {
        
    }
}