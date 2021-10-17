using System;
using Kugushev.Scripts.Battle.Core.Interfaces;
using Kugushev.Scripts.Battle.Core.Models.Fighters;
using Kugushev.Scripts.Battle.Core.ValueObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kugushev.Scripts.Battle.Presentation.Controllers
{
    public class InputController : MonoBehaviour, IInputController
    {
        public event Action<PlayerFighter>? PlayerUnitSelected;
        public event Action<Position>? GroundCommand;
        public event Action<EnemyFighter>? EnemyUnitCommand;

        internal void OnUnitClick(BaseFighter fighter, PointerEventData.InputButton inputButton)
        {
            switch (inputButton, unit: fighter)
            {
                case (PointerEventData.InputButton.Left, PlayerFighter playerUnit):
                    PlayerUnitSelected?.Invoke(playerUnit);
                    break;
                case (PointerEventData.InputButton.Right, EnemyFighter enemyUnit):
                    EnemyUnitCommand?.Invoke(enemyUnit);
                    break;
                default:
                    Debug.Log("Miss");
                    break;
            }
        }

        internal void OnGroundClick(Vector2 position, PointerEventData.InputButton inputButton)
        {
            switch (inputButton)
            {
                case PointerEventData.InputButton.Right:
                    GroundCommand?.Invoke(new Position(position));
                    break;
                
                default:
                    Debug.Log("Miss ground");
                    break;
            }
        }
    }
}