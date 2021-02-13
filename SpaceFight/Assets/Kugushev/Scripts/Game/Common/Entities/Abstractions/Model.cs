using UnityEngine;

namespace Kugushev.Scripts.Game.Common.Entities.Abstractions
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