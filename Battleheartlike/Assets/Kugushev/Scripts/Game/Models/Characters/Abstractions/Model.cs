using System;
using UnityEngine;

namespace Kugushev.Scripts.Game.Models.Characters.Abstractions
{
    public abstract class Model: ScriptableObject
    {
        protected abstract void Dispose(bool destroying);
        
        private void OnDestroy()
        {
            Dispose(true);
        }
    }
}