using Kugushev.Scripts.Game;
using Kugushev.Scripts.Game.Components;
using Kugushev.Scripts.Game.Components.Commands;
using Kugushev.Scripts.Game.Components.ViewRefs;
using Kugushev.Scripts.Game.Interfaces;
using Kugushev.Scripts.Game.Views;
using Leopotam.Ecs;
using UnityEngine;

namespace Kugushev.Scripts.City.Systems
{
    public class CitizensSpawner : IEcsInitSystem
    {
        private EcsWorld _world;

        private UnitTransformView _hero;
        private IGrid _grid;

        public void Init()
        {
            var transformView = _hero.GetComponent<UnitTransformView>();
            var playerView = _hero.GetComponent<HeroUnitView>();

            var start = new Vector2Int(1, 1);
            var startWorld = _grid.CellToWorld(start);

            _world.NewEntity()
                .Replace(new UnitTransformViewRef(transformView))
                .Replace(new HeroUnitViewRef(playerView))
                .Replace(new UnitMoveCommand())
                .Replace(new UnitGridPosition {ActualPosition = start}) // todo: take from save file
                .Replace(new UnitTransform {Position = startWorld});
        }
    }
}