using System;
using Kugushev.Scripts.Core.Battle.Interfaces;
using Kugushev.Scripts.Core.Battle.Models.Units;
using Kugushev.Scripts.Core.Battle.ValueObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kugushev.Scripts.Presentation.Battle.Controllers
{
    public class InputController : MonoBehaviour, IInputController
    {
        public event Action<PlayerUnit>? PlayerUnitSelected;
        public event Action<Position>? GroundCommand;
        public event Action<EnemyUnit>? EnemyUnitCommand;

        internal void OnUnitClick(BaseUnit unit, PointerEventData.InputButton inputButton)
        {
            switch (inputButton, unit)
            {
                case (PointerEventData.InputButton.Left, PlayerUnit playerUnit):
                    PlayerUnitSelected?.Invoke(playerUnit);
                    break;
                case (PointerEventData.InputButton.Right, EnemyUnit enemyUnit):
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