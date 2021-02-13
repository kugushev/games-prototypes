using Kugushev.Scripts.Game.Common.Entities.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.PresentationModels.Abstractions
{
    public abstract class BasePresentationModel : MonoBehaviour
    {
        protected abstract Model Model { get; }

        public T GetModelAs<T>()
        {
            if (Model is T result)
                return result;

            Debug.LogError($"Unable to cast {Model} to {typeof(T)}");
            return default;
        }
    }
}