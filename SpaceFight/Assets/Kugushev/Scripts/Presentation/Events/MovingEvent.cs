﻿using System;
using Kugushev.Scripts.Presentation.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Kugushev.Scripts.Presentation.Events
{
    [Serializable]
    public class MovingEvent: UnityEvent<HandController, Vector3>
    {
        
    }
}