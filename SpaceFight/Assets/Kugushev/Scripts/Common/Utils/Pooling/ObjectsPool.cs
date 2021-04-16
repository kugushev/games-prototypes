using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Kugushev.Scripts.Common.Utils.Pooling
{
    [CreateAssetMenu(menuName = CommonConstants.MenuPrefix + "ObjectsPool")]
    public class ObjectsPool : ScriptableObject
    {
        // todo: use concurrent collection once I decide to use this toll in multithreading
        // we don't care about boxing because T is class
        private readonly Dictionary<Type, Queue<object>> _pools = new Dictionary<Type, Queue<object>>();

        private readonly Type[] _poolableObjectCtorParameters = {typeof(ObjectsPool)};
        private object[]? _poolableObjectCtorParametersValues;
        private readonly Dictionary<Type, ConstructorInfo> _constructors = new Dictionary<Type, ConstructorInfo>();

        public TObj GetObject<TObj, TState>(TState state)
            where TState : struct
            where TObj : class, IPoolable<TState>
        {
            var type = typeof(TObj);
            object obj;
            if (_pools.TryGetValue(type, out var pool) && pool.Count > 0)
                obj = pool.Dequeue();
            else
                obj = InstantiateObject(type);

            if (obj is TObj result)
            {
                result.SetState(state);
                return result;
            }

            throw new NotSupportedException($"Object type {obj} is not assignable to target {type}");
        }

        public void GiveBackObject<TObj>(TObj obj)
            where TObj : class, IPoolable
        {
            obj.ClearState();

            var type = typeof(TObj);
            if (!_pools.TryGetValue(type, out var pool))
                _pools[type] = pool = new Queue<object>();
            pool.Enqueue(obj);
        }

        private object InstantiateObject(Type type)
        {
            if (!_constructors.TryGetValue(type, out var ctor))
            {
                var findCtor = FindCtor(type);
                if (findCtor != null)
                    ctor = _constructors[type] = findCtor;
                else
                    throw new NotSupportedException($"Ctor of type {type} has not found");
            }

            _poolableObjectCtorParametersValues ??= new object[] {this};
            return ctor.Invoke(_poolableObjectCtorParametersValues);
        }

        private ConstructorInfo? FindCtor(Type type) => type.GetConstructor(_poolableObjectCtorParameters);
    }
}