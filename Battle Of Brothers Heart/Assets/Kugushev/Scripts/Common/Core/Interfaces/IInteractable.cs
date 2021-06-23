using Kugushev.Scripts.Common.Core.ValueObjects;
using UnityEngine;

namespace Kugushev.Scripts.Common.Core.AI
{
    public interface IInteractable
    {
        Position Position { get; }
        bool IsInteractable { get; }
    }
}