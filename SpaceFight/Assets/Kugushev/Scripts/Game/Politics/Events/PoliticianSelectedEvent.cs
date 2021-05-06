using System;
using Kugushev.Scripts.Game.Core.Models;
using UnityEngine.Events;

namespace Kugushev.Scripts.Game.Politics.Events
{
    [Serializable]
    internal class PoliticianSelectedEvent: UnityEvent<IPolitician>
    {
        
    }
}