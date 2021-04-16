using System;
using Kugushev.Scripts.Game.Models;
using UnityEngine.Events;

namespace Kugushev.Scripts.Game.Events
{
    [Serializable]
    public class PoliticianSelectedEvent: UnityEvent<Politician?>
    {
        
    }
}