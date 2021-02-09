using UnityEngine;

namespace Kugushev.Scripts.Game.Entities.Abstractions
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