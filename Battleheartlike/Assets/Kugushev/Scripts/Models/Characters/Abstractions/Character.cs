using System;
using Kugushev.Scripts.Core.AI.Objectives.Abstractions;
using Kugushev.Scripts.Core.Behaviors;
using Kugushev.Scripts.Core.Providers;
using UnityEngine;

namespace Kugushev.Scripts.Models.Characters.Abstractions
{
    public abstract class Character : ScriptableObject, IMovable
    {
        // todo: it should be NavigationController
        public IPathfindingProvider PathfindingProvider => throw new NotImplementedException();

        public void AppendObjective(IObjective objective)
        {
            // todo: think about saving all this things to separate AI service
            throw new NotImplementedException();
        }
    }
}