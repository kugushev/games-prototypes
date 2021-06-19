using System;
using Kugushev.Scripts.Game.Core.ValueObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kugushev.Scripts.Common.Core.Controllers
{
    public class InputController
    {
        public event Action<Position>? GroundCommand;

        public void OnGroundClick(Vector2 position, PointerEventData.InputButton inputButton)
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