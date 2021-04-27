using Kugushev.Scripts.Common.Utils;
using UnityEngine;

namespace Kugushev.Scripts.Common.Modes
{
#nullable disable
    public class ParametersPipeline<T>
    {
        private T _parameters;

        public virtual void Push(T parameters)
        {
            if (_parameters != null)
            {
                Debug.LogError("Parameters are already set");
                return;
            }

            _parameters = parameters;
        }

        public virtual bool TryPop(out T value)
        {
            if (_parameters == null)
            {
                const string message = "Parameters are not set";
                Debug.LogError(message);
                value = default;
                return false;
            }

            value = _parameters;
            _parameters = default;
            return true;
        }
    }
#nullable restore
}