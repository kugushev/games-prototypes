using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Factories.Abstractions
{
    public abstract class BaseEntitiesFactory : ScriptableObject
    {
        public abstract EcsEntity Create(EcsWorld world);
    }
}