using UnityEngine;

namespace Kugushev.Scripts.Common.Modes
{
#nullable disable
    public class ParametersPipeline<T>
    {
        private T _parameters;
        private bool _isSet;

        public virtual void Push(T parameters)
        {
            if (_isSet)
            {
                Debug.LogError($"Parameters {typeof(T)} are already set: {_parameters}");
                return;
            }

            _parameters = parameters;
            _isSet = true;
        }

        public virtual bool TryPop(out T value)
        {
            if (!_isSet)
            {
                value = default;
                return false;
            }

            value = _parameters;
            _parameters = default;
            _isSet = false;
            return true;
        }
    }
#nullable enable
}