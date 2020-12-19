using JetBrains.Annotations;
using Kugushev.Scripts.Core.AI.Objectives.Abstractions;

namespace Kugushev.Scripts.Core.Activities.Abstractions
{
    public interface IActivity
    {
        [CanBeNull] IObjective Act();
    }
}