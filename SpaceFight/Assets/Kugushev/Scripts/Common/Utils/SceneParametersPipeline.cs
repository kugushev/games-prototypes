using UnityEngine;

namespace Kugushev.Scripts.Common.Utils
{
    public abstract class SceneParametersPipeline<T> : ScriptableObject
        where T : struct
    {
        private T? _parameters;

        public void Set(T parameters)
        {
            if (_parameters != null)
            {
                Debug.LogError("Parameters are already set");
                return;
            }

            _parameters = parameters;
        }

        public T Get()
        {
            if (_parameters == null)
            {
                const string message = "Parameters are not set";
                Debug.LogError(message);
                return default;
            }

            var result = _parameters.Value;
            _parameters = null;
            return result;
        }
    }
}