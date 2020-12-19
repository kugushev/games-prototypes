using System;
using System.Collections.Generic;
using Kugushev.Scripts.Core.Activities;
using Kugushev.Scripts.Core.AI.Objectives;
using Kugushev.Scripts.Core.Interactions;
using UnityEngine;

namespace Kugushev.Scripts.Common.Pooling
{
    [CreateAssetMenu(fileName = "ObjectsPool", menuName = "Game/ObjectsPool", order = 0)]
    public class ObjectsPool : ScriptableObject
    {
        // todo: use concurrent collection once I decide to use this toll in multithreading
        // we don't care about boxing because T is class
        private readonly Dictionary<Type, Queue<object>> _pools = new Dictionary<Type, Queue<object>>();

        private readonly IReadOnlyDictionary<Type, Func<ObjectsPool, object>> _constructors =
            new Dictionary<Type, Func<ObjectsPool, object>>
            {
                {typeof(MovementActivity), pool => new MovementActivity(pool)},
                {typeof(MoveToPositionObjective), pool => new MoveToPositionObjective(pool)}
            };

        public TObj GetObject<TObj, TState>(TState state)
            where TState : struct
            where TObj : class, IPoolable<TState>
        {
            var type = typeof(TObj);
            object obj;
            if (_pools.TryGetValue(type, out var pool) && pool.Count > 0)
                obj =  pool.Dequeue();
            else
            {
                if (!_constructors.TryGetValue(type, out var ctor))
                    throw new NotSupportedException($"No ctor of type {type} in pool");

                obj = ctor(this);
            }

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
    }
}