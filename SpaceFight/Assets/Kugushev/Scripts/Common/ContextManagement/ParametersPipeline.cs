using Kugushev.Scripts.Common.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Common.ContextManagement
{
#nullable disable
    public class ParametersPipeline<T>
    {
        private T _parameters;
        private bool _isSet;

        public void Push(T parameters)
        {
            if (_isSet)
            {
                Debug.LogError($"Parameters {typeof(T)} are already set: {_parameters}");
                return;
            }

            _parameters = parameters;
            _isSet = true;
        }

        public T Pop()
        {
            if (!_isSet)
                throw new SpaceFightException($"Parameters {typeof(T)} are not set");

            var value = _parameters;
            
            _parameters = default;
            _isSet = false;
            return value;
        }
    }
#nullable enable
}