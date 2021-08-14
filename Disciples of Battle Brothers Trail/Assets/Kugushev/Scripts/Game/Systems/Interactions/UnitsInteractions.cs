using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.Refs;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Managers;
using Kugushev.Scripts.Game.Models.HeroInfo;
using Kugushev.Scripts.Game.Models.Units;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.Game.Systems.Interactions
{
    public class UnitsInteractions : IEcsRunSystem
    {
        private EcsFilter<UnitMoveOneStepEvent, ModelRef<IInteractable>> _filter;

        private CellsVisitingManager _cellsVisitingManager;
        private GameModeManager _gameModeManager;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var position = ref _filter.Get1(i);
                ref var interactable = ref _filter.Get2(i);

                var actual = position.ActualPosition;
                var previous = position.PreviousPosition;

                _cellsVisitingManager.Unregister(previous, interactable.Ref);

                if (_cellsVisitingManager.Visitors.TryGetValue(actual, out var cellInteractable))
                {
                    if (cellInteractable != interactable.Ref)
                    {
                        Debug.Log($"Interact in {actual}");
                        Interact(interactable.Ref, cellInteractable);
                    }
                }
                else
                    _cellsVisitingManager.Register(actual, interactable.Ref);
            }
        }

        private void Interact(IInteractable active, IInteractable passive)
        {
            switch ((active, passive))
            {
                case (Hero _, BanditModel b):
                    _gameModeManager.ToBattleAsync(b);
                    break;
                case (BanditModel b, Hero _):
                    _gameModeManager.ToBattleAsync(b);
                    break;
                case (BanditModel b1, BanditModel b2):
                    Debug.Log("Bandits meeting");
                    break;
                default:
                    Debug.Log($"No interactions for {active} and {passive}");
                    break;
            }
        }
    }
}