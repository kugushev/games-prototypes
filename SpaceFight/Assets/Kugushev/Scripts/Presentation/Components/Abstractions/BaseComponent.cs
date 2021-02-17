using System;
using Kugushev.Scripts.Common.Utils.ComponentInjection;
using Kugushev.Scripts.Presentation.PresentationModels.Abstractions;
using UnityEngine;

namespace Kugushev.Scripts.Presentation.Components.Abstractions
{
    [RequireComponent(typeof(BasePresentationModel))]
    public abstract class BaseComponent<T> : MonoBehaviour
    {
        private BasePresentationModel _presentationModel;

        protected T Model => _presentationModel.GetModelAs<T>();

        protected abstract void OnAwake();
        protected abstract void OnStart();

        private void Awake()
        {
            _presentationModel = GetComponent<BasePresentationModel>();
            OnAwake();
        }

        private void Start()
        {
            InjectComponents(Model);
            OnStart();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                var presentationModel = GetComponent<BasePresentationModel>();
                if (!presentationModel.IsInPrefab)
                {
                    var model = presentationModel.GetModelAs<T>();
                    if (model == null)
                        Debug.LogError($"Unable to get model for component {this}");
                    else
                        InjectComponents(model);
                }
            }
        }

        private void InjectComponents(T model)
        {
            var properties = ComponentInjectionHelper.FindInjectableProperties(model);
            var interfaces = ComponentInjectionHelper.FindInterfaces(this);
            foreach (var property in properties)
                ComponentInjectionHelper.AssignIfPossible(model, property, this, interfaces);
        }
    }
}