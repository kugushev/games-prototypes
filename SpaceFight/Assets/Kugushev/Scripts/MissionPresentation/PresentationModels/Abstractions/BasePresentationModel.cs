using Kugushev.Scripts.Common.Models.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.MissionPresentation.PresentationModels.Abstractions
{
    public abstract class BasePresentationModel : MonoBehaviour
    {
        protected abstract IModel Model { get; }

        public virtual bool IsInPrefab => false;

        public T GetModelAs<T>()
        {
            if (Model is T result)
                return result;

            Debug.LogError($"Unable to cast {Model} to {typeof(T)}");
            return default;
        }
    }
}