using System.Collections.Generic;
using Kugushev.Scripts.Common.ValueObjects;
using Kugushev.Scripts.Mission.Core.Models;
using UnityEngine;
using Zenject;

namespace Kugushev.Scripts.Mission.Core.ValueObjects
{
    public readonly struct Order
    {
        private readonly List<Vector3> _path;

        public Order(Planet source, Planet target, Percentage power, List<Vector3> path)
        {
            SourcePlanet = source;
            TargetPlanet = target;
            Power = power;
            _path = path;
        }

        public Planet SourcePlanet { get; }
        public Planet TargetPlanet { get; }
        public Percentage Power { get; }
        public IReadOnlyList<Vector3> Path => _path;

        public void Dispose()
        {
            ListPool<Vector3>.Instance.Despawn(_path);
        }
    }
}