using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Kugushev.Scripts
{
    public class Unit : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var transformPosition = transform.position;
            dstManager.AddComponentData(entity, new Translation
            {
                Value = transformPosition
            });
        }
    }
}