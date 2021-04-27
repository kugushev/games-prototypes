using System;
using Kugushev.Scripts.Game.Models;
using UnityEngine.Events;

namespace Kugushev.Scripts.Game.Events
{
    [Serializable]
    internal class PoliticianSelectedEvent: UnityEvent<IPolitician?>
    {
        
    }
}