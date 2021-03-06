using UnityEngine;

namespace Kugushev.Scripts.Common.Utils
{
    public abstract class ModelProvider<TModel, TManager> : ScriptableObject
        where TModel : class
    {
        [SerializeReference] private TModel model;

        public bool TryGetModel(out TModel result)
        {
            if (!ReferenceEquals(model, null))
            {
                result = model;
                return true;
            }

            result = null;
            return false;
        }

        public void Set(TManager owner, TModel value) => model = value;

        public void Cleanup(TManager owner) => model = null;
    }
}